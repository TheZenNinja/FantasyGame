using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCMovement : MonoBehaviour
{
    Vector3 inputVector;
    public Transform center;
    public float speed = 5;
    public float airSpeed = 0.4f;
    public float airComboSpeed = 2;
    public float sprintSpeed = 16;
    public float acceleration;

    public bool canMove = true;
    public bool grounded;

    [Header("Jumping")]
    public bool canDoubleJump;
    public float jumpForce;
    public float doubleJumpForce;
    public bool jumpCharging;
    [Tooltip("x = current, y = max time")]
    public Vector2 chargeJumpTime = new Vector2(0,10);
    [Tooltip("x = min, y = max")]
    public Vector2 chargeJumpForce;
    public ParticleSystem chargeJumpParticle;
    public ParticleSystem jumpParticle;
    public ParticleSystem doubleJumpParticle;
    [Space]
    public Vector3 attackMove;
    public bool airCombo;
    public float gravCancelBoost = 1;
    public float grav;
    public bool useGrav;
    public Vector3 velocity;

    public Entity targetEntity;

    [Space]
    public bool dodging;
    public float dodgeSpeed = 2;

    public CameraMovement camMovement;


    CharacterController cc;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        canMove = true;
    }

    void Update()
    {
        inputVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        grounded = cc.isGrounded;

        //TurnCharacter();
        //Move();
        Move();
    }

    public void Move()
    {
        //look rotation
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, camMovement.camCenter.eulerAngles.y, transform.eulerAngles.z);

        if (useGrav)
            velocity.y -= grav * Time.deltaTime;
        else
        {
            if (velocity.y > 0)
                velocity.y = Mathf.Clamp(velocity.y - grav * Time.deltaTime, 0, Mathf.Infinity);
        }



        if (grounded)
        {
            velocity = inputVector;
            velocity *= Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : speed;
            velocity.y = -0.1f;

            if (Input.GetKey(KeyCode.LeftControl))
            {
                if (jumpCharging)
                {
                    chargeJumpTime.x += Time.deltaTime;
                    if (chargeJumpTime.x >= chargeJumpTime.y)
                    {
                        ChargeJump();
                    }
                    if (Input.GetKeyUp(KeyCode.Space))
                        ChargeJump();
                }
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    chargeJumpParticle.Play();
                    jumpCharging = true;
                }

            }
            else if (Input.GetKey(KeyCode.Space))
            {
                velocity.y = jumpForce;
                jumpParticle.Play();
                canDoubleJump = true;
            }
        }
        else
        {
            if (velocity.x > speed && inputVector.x > 0) inputVector = new Vector3(0, inputVector.y, inputVector.z);
            if (velocity.x < -speed && inputVector.x < 0) inputVector = new Vector3(0, inputVector.y, inputVector.z);

            if (velocity.z > speed && inputVector.z > 0) inputVector = new Vector3(inputVector.x, inputVector.y, 0);
            if (velocity.z < -speed && inputVector.z < 0) inputVector = new Vector3(inputVector.x, inputVector.y, 0);

            velocity += inputVector * airSpeed;

            if (Input.GetKeyDown(KeyCode.Space) && canDoubleJump)
            {
                velocity.y = doubleJumpForce;
                canDoubleJump = false;
                doubleJumpParticle.Play();
            }
        }
        if (attackMove.magnitude > Mathf.Epsilon)
        {
            if (grounded)
                cc.Move(center.TransformVector(attackMove - Vector3.up * 0.1f) * Time.deltaTime);
            else
                cc.Move(center.TransformVector(attackMove) * Time.deltaTime);

        }
        else if (canMove && !jumpCharging)
            cc.Move(center.TransformVector(velocity) * Time.deltaTime);
    }

    void ChargeJump()
    {
        chargeJumpParticle.Stop();

        if (chargeJumpTime.x >= chargeJumpTime.y)
            velocity.y = chargeJumpForce.y;
        else
            velocity.y = chargeJumpForce.x;
        chargeJumpTime.x = 0;
        jumpCharging = false;

        canDoubleJump = true;
    }

    public void Dodge(bool start)
    {
        dodging = start;
    }


    public void ToggleGrav(bool active, bool verticalBoost = false)
    {
        airCombo = !active;
        useGrav = active;
        velocity.y = (!active && verticalBoost) ? gravCancelBoost : 0;
    }

    public void AttackMove(Vector3 dir, float duration)
    {
        StartCoroutine(AttackMoveTimer(dir, duration));
    }
    private IEnumerator AttackMoveTimer(Vector3 dir, float dur)
    {
        attackMove += dir;
        yield return new WaitForSeconds(dur);
        attackMove -= dir;
    }

    public void StopAttackMove()
    {
        attackMove = Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        Entity[] entities = FindObjectsOfType<Entity>();

        Gizmos.color = Color.blue;
        if (entities.Length > 0)
            for (int i = 0; i < entities.Length; i++)
            {
                Vector3 dir = (transform.position - entities[i].position).normalized;
                dir *= 2;
                dir.y = -1;
                Gizmos.DrawSphere(entities[i].position + dir, 0.25f);
            }
    }
}
