using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform center, camCenter, player, camPos, cam;
    public float height = 1.75f;
    public float lookDistance = 100;
    [Header("Camera Positioning")]
    [SerializeField] private int _cm;
    public int CamMode
    {
        get
        { return _cm; }
        set
        {
            _cm = value;

            switch (_cm)
            {
                case 1:
                    goalOffset = offsetClose;
                    break;
                case 2:
                    goalOffset = offsetCombat;
                    break;
                case 3:
                    goalOffset = offsetFar;
                    break;
                default:
                    goalOffset = offsetDefault;
                    break;
            }
        }
    }
    public float camSpeed = 10;
    public Vector3 offsetDefault = new Vector3(.75f, .75f, -5f);
    public Vector3 offsetClose = new Vector3(0.5f, 0.5f, -4.5f);
    public Vector3 offsetCombat = new Vector3(0.5f, 0.5f, -4.5f);
    public Vector3 offsetFar = new Vector3(2, 2, -6f);

    public Vector3 goalOffset;
    public Vector3 currentOffset;
    [Space]
    [Header("Camera Look")]
    public Vector2 current;
    public Vector2 sensitivity = Vector2.one * 3;
    public Vector2 clamp = new Vector2(80, -80);

    private void Start()
    {
        goalOffset = offsetDefault;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        CamMode = 2;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            CamMode++;
            if (CamMode >= 4)
                CamMode = 0;
        }
        center.position = player.transform.position + player.transform.up * height;
        center.eulerAngles = new Vector3(center.eulerAngles.x, camCenter.eulerAngles.y, center.eulerAngles.z);
        //center.eulerAngles += new Vector3(0, Input.GetAxis("Mouse X"), 0);


        current.x += Input.GetAxis("Mouse X") * sensitivity.x;
        current.y += Input.GetAxis("Mouse Y") * sensitivity.y;

        current.y = Mathf.Clamp(current.y, clamp.y, clamp.x);

        if (current.x > 360)
            current.x -= 360;
        if (current.x < -360)
            current.x += 360;

        camCenter.eulerAngles = new Vector3(0, current.x, 0);
        camCenter.localEulerAngles = new Vector3(current.y, 0, 0);
    }

    private void LateUpdate()
    {
        Quaternion rotation = Quaternion.Euler(current.y, current.x, 0);
        camCenter.rotation = rotation;

        if ((camPos.localPosition - currentOffset).sqrMagnitude <= 0.001f)
            currentOffset = Vector3.Lerp(currentOffset, goalOffset, Time.deltaTime * camSpeed);
        else
            currentOffset = goalOffset;

        camPos.localPosition = currentOffset;

        //cam.LookAt(camPos.forward * lookDistance);
        /*center.rotation = Quaternion.Euler(0, current.x, 0);

        transform.position = center.position + center.TransformVector(camOffset);*/
    }
}
