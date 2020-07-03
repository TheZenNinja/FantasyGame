using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Vector3 inputVector;
    [Header("State")]
    public bool canMove = true;
    public bool sprint;
    public bool crouched;
    public bool sliding;
    //public bool wallrunning;
    public bool useGrav;
    [Header("Crouching")]
    public float crouchedCamHeight;
    public float baseCamHeight;
    [Header("Horz Movement")]
    public float speed = 8;
    public float crouchedSpeed = 4;
    public float sprintSpeed = 12;
    public float acceleration;
    public float airAccel;
    public float horzDrag;
    [Header("Dodge")]
    public bool canDodge;
    public float dodgeInputWindow = 0.25f;
    float dodgeInputTime = 0;
    public float dodgeDuration = 0.5f;
    public float dodgeSpeed = 20;
    public float dodgeCooldown = 1;
    float dodgeCooldownTime = 0;
    /*[Header("Wallrun")]
    public float wallrunDistance;
    public float wallrunSize;
    public float wallrunSpeed;
    public float wallrunStickForce;
    public LayerMask wallrunMask;*/
    [Header("Sliding")]
    public bool gaveSlideBoost;
    public float slideSpeed = 16;
    public float slideThreshold = 8;
    public float slideDrag = 0.01f;

    [Header("Vertical Movement")]
    public float jumpForce;
    public float grav;
    public float fastFallGrav;
    public float normalGrav;
    [Header("Interact")]
    public Transform cam;
    public float interactDistance = 4;
    public LayerMask interactableLayers;

    [Space]
    public Vector3 velocity;
    public Vector3 externalForce;
    CharacterController cc;
    CapsuleCollider capsuleCol;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Health>().Die = Die;
        cc = GetComponent<CharacterController>();
        capsuleCol = GetComponent<CapsuleCollider>();
        grav = normalGrav;
    }

    // Update is called once per frame
    void Update()
    {
        if (dodgeCooldownTime > 0)
            dodgeCooldownTime -= Time.deltaTime;
        canDodge = dodgeCooldownTime <= 0;

        if (dodgeInputTime > 0)
            dodgeInputTime -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.position, cam.forward, out hit, interactDistance, interactableLayers, QueryTriggerInteraction.Ignore))
            {
                Debug.Log(hit.collider.gameObject.name);
                hit.collider.gameObject.GetComponent<IInteractable>()?.Interact();
            }
        }

        Move();
    }
    public void Move()
    {
        inputVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 goalDir = transform.TransformVector(Vector3.ClampMagnitude(inputVector, 1));

        #region Gravity
        if (useGrav)
        {
            if (!cc.isGrounded)
                velocity.y -= grav * Time.deltaTime;
            else
            {
                if (cc.isGrounded)
                    velocity.y = -0.1f;
                else if (velocity.y > 0)
                    velocity.y = Mathf.Clamp(velocity.y - grav * Time.deltaTime, 0, Mathf.Infinity);
            }
        }
        #endregion

        if (canMove)
        {

            #region Crouched and Sliding
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                crouched = true;
                cc.height = 1;
                capsuleCol.height = 1;
                velocity.y = -10;//transform.position = transform.position - transform.up * 1f;
            }
            else if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                crouched = false;
                cc.height = 2;
                capsuleCol.height = 2;
                gaveSlideBoost = false;
            }
            #endregion

            if (cc.isGrounded)
            {
                Vector3 goalSpeed = goalDir;

                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    sprint = true;
                    dodgeInputTime = dodgeInputWindow;
                }
                else if (Input.GetKeyUp(KeyCode.LeftShift))
                {
                    sprint = false;
                    if (canDodge && dodgeInputTime > 0)
                    {
                        dodgeCooldownTime = dodgeCooldown + dodgeDuration;
                        StartCoroutine(Dodge());
                    }
                }
                if (crouched)
                    goalSpeed *= crouchedSpeed;
                else if (sprint)
                    goalSpeed *= sprintSpeed;
                else
                    goalSpeed *= speed;

                if (crouched && velocity.magnitude > slideThreshold && !sliding && !gaveSlideBoost)
                {
                    sliding = true;
                    gaveSlideBoost = true;
                    velocity = goalDir * slideSpeed;
                }
                else if (sliding && (!crouched || velocity.magnitude < slideThreshold))
                    sliding = false;


                if (Input.GetKeyDown(KeyCode.Space))
                {
                    //Debug.Log("Jump");
                    velocity.y = jumpForce;
                    grav = normalGrav;
                }



                if (canMove)
                {
                    if (sliding)
                        velocity = SlerpXZ(velocity, goalSpeed, slideDrag * Time.deltaTime);
                    else if (Mathf.Abs(inputVector.sqrMagnitude) > Mathf.Epsilon)
                        velocity = LerpXZ(velocity, goalSpeed, acceleration * Time.deltaTime);
                    else
                        velocity = LerpXZ(velocity, Vector3.zero, horzDrag * Time.deltaTime);
                }
                else
                    velocity = LerpXZ(velocity, Vector3.zero, horzDrag * Time.deltaTime);
            }

            else // airborne & wallrun
            {
                /*bool wallrunRight = Physics.CheckSphere(transform.position + transform.right * wallrunDistance, wallrunSize, wallrunMask);
                bool wallrunLeft = Physics.CheckSphere(transform.position - transform.right * wallrunDistance, wallrunSize, wallrunMask);

                wallrunning = (wallrunRight || wallrunLeft) && Input.GetKey(KeyCode.LeftShift);

                if (wallrunning)
                {
                    Vector3 goalSpeed;
                    RaycastHit hit;

                    if (wallrunRight)
                        Physics.Raycast(transform.position, transform.right, out hit, 3, wallrunMask);
                    else
                        Physics.Raycast(transform.position, -transform.right, out hit, 3, wallrunMask);

                    Vector3 stickForce = -wallrunStickForce * hit.normal;

                    goalSpeed = stickForce + goalDir * wallrunSpeed + Vector3.up * grav;

                    if (canMove && Mathf.Abs(inputVector.sqrMagnitude) > Mathf.Epsilon )
                        velocity = LerpXZ(velocity, goalSpeed, airAccel * Time.deltaTime);

                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        //Debug.Log("Jump");
                        velocity.y = jumpForce;
                        //velocity += stickForce;
                        grav = normalGrav;
                    }
                }
                else // airborne*/
                {
                    if (grav == normalGrav)
                    {
                        if (velocity.y > 0 && Input.GetKeyUp(KeyCode.Space))
                            grav = fastFallGrav;

                        if (Input.GetKeyDown(KeyCode.LeftControl))
                            grav = fastFallGrav;
                    }


                    Vector3 goalAirVel = goalDir * speed;

                    //fix air movement conflicting with sliding in air
                    if (canMove && Mathf.Abs(inputVector.sqrMagnitude) > Mathf.Epsilon && Mathf.Abs(HorzMagnitude(velocity)) < 6)
                        velocity = LerpXZ(velocity, goalAirVel, airAccel * Time.deltaTime);
                }
            }
        }
        else //cant move
        {
            velocity = LerpXZ(velocity, Vector3.zero, horzDrag * Time.deltaTime);
        }

        cc.Move((velocity + externalForce) * Time.deltaTime);
    }
    IEnumerator Dodge()
    {
        Debug.Log("Dodge");
        PlayerStatus.instance.invuln = true;
        Vector3 speed;
        if (inputVector.magnitude > Mathf.Epsilon)
            speed = transform.TransformVector(inputVector.normalized) * dodgeSpeed;
        else
            speed = transform.forward * -dodgeSpeed;

        externalForce += speed;
        yield return new WaitForSeconds(dodgeDuration);
        externalForce -= speed;
        PlayerStatus.instance.invuln = false;
    }
    private Vector3 LerpXZ(Vector3 a, Vector3 b, float t)
    {
        float y = a.y;
        Vector3 v = Vector3.Lerp(a, b, t);
        v.y = y;
        return v;
    }
    private Vector3 SlerpXZ(Vector3 a, Vector3 b, float t)
    {
        float y = a.y;
        Vector3 v = Vector3.Slerp(a, b, t);
        v.y = y;
        return v;
    }
    private float HorzMagnitude(Vector3 v)
    {
        v.y = 0;
        return Vector3.Magnitude(v);
    }

    private void OnDrawGizmos()
    {
        if (cam)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(cam.position, interactDistance);
        }

        /*{
            Gizmos.color = Color.green;

            Gizmos.DrawWireSphere(transform.position + transform.right * wallrunDistance, wallrunSize);
            Gizmos.DrawWireSphere(transform.position - transform.right * wallrunDistance, wallrunSize);
        }*/
    }

    public void AddForce(Vector3 dir)
    {
        velocity += dir;
    }

    public void Die()
    {
        FindObjectOfType<SceneLoader>().LoadScene(0);
        this.enabled = false;
    }

}

