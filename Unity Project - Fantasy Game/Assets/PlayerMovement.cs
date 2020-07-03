using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    Vector2 keyInput;
    public Transform center;
    public float speed = 5;
    public float sprintSpeed = 16;

    public float maxSpeed = 12;
    public float maxSprintSpeed = 16;

    public float drag = 0.5f;

    public float jumpHeight = 2;

    public float groundDistance = 0.05f;
    public bool grounded;
    public bool canLook = true;
    public bool isCountingDown;
    public float countdownTimer = 1;

    public CameraMovement camMovement;
    [Space]
    public float grav = 1;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        keyInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // From the jump height and gravity we deduce the upwards speed 
            // for the character to reach at the apex.
            if (rb.velocity.y > Mathf.Epsilon)
                rb.velocity += Vector3.up * Mathf.Sqrt(2 * (jumpHeight + 1));
            else
                rb.velocity = new Vector3(rb.velocity.x, Mathf.Sqrt(2 * (jumpHeight + 1) * -Physics.gravity.y), rb.velocity.z);
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position + transform.up * 0.5f, -transform.up, out hit, groundDistance + 0.5f))
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }

    private void FixedUpdate()
    {
        Vector3 worldInput = center.TransformVector(keyInput.x, 0, keyInput.y);

        float max = Input.GetKey(KeyCode.LeftShift) ? maxSprintSpeed : maxSpeed;
        float multi = 1;
        float dragVal = 1/drag;
        if (grounded)
        {
            //https://answers.unity.com/questions/1518803/add-force-to-a-max-speed.html
            //https://answers.unity.com/questions/819273/force-to-velocity-scaling.html?childToView=819474#answer-819474
            //https://answers.unity.com/questions/233850/rigidbody-making-drag-affect-only-horizontal-speed.html
            //https://www.youtube.com/watch?v=BJBSMUDDAoE

            Vector3 localVel = transform.InverseTransformVector(rb.velocity);


            if (rb.velocity.x > 0 && worldInput.x <= 0) rb.velocity = Vector3.Scale(rb.velocity, new Vector3(dragVal,1,1));
            if (rb.velocity.x < 0 && worldInput.x >= 0) rb.velocity = Vector3.Scale(rb.velocity, new Vector3(dragVal, 1, 1));

            if (rb.velocity.z > 0 && worldInput.z <= 0) rb.velocity = Vector3.Scale(rb.velocity, new Vector3(1, 1, dragVal));
            if (rb.velocity.z < 0 && worldInput.z >= 0) rb.velocity = Vector3.Scale(rb.velocity, new Vector3(1, 1, dragVal));

            //rb.drag = 3;
            /*if (dragVal > 0)
            {
                Debug.Log("drag");
                Vector3 vel = rb.velocity;
                vel.x *= dragVal;
                vel.z *= dragVal;
                vel = rb.velocity;
            }*/
        }
        else
        {
            //rb.drag = 0;
            multi = 0.5f;

            if (!Input.GetKey(KeyCode.Space) && rb.velocity.y <= 0)
            {
                rb.velocity += Vector3.up * -grav * Time.deltaTime;
            }
        }


        if (rb.velocity.x > 0 && rb.velocity.x > max) worldInput.x = 0;
        if (rb.velocity.x < 0 && rb.velocity.x < -max) worldInput.x = 0;

        if (rb.velocity.z > 0 && rb.velocity.z > max) worldInput.z = 0;
        if (rb.velocity.z < 0 && rb.velocity.z < -max) worldInput.z = 0;


        rb.AddForce(worldInput * multi * speed, ForceMode.Impulse);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position + transform.up * 0.5f, transform.position + transform.up * -groundDistance);
    }
}
