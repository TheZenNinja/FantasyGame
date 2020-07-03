using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Transform player;
    public bool canlook = true;
    public float height = 1.75f;
    public float lookDistance = 100;
    public float camSpeed = 10;
    [Space]
    [Header("Camera Look")]
    public Vector2 current;
    public Vector2 sensitivity = Vector2.one * 3;
    public Vector2 clamp = new Vector2(80, -80);

    private void Start()
    {
        MakeCursorActive(false);
    }
    void Update()
    {
        if (canlook)
        {
            current.x += Input.GetAxis("Mouse X") * sensitivity.x;
            current.y += Input.GetAxis("Mouse Y") * sensitivity.y;
        }

        current.y = Mathf.Clamp(current.y, clamp.y, clamp.x);

        if (current.x > 360)
            current.x -= 360;
        if (current.x < -360)
            current.x += 360;

        player.localEulerAngles = new Vector3(0, current.x, 0);
        transform.localEulerAngles = new Vector3(-current.y, 0, 0);
    }
    public void MakeCursorActive(bool active)
    {
        Cursor.lockState = active ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = active;
        canlook = !active;
    }
    private void LateUpdate()
    {
        //Quaternion rotation = Quaternion.Euler(current.y, current.x, 0);

        //cam.LookAt(camPos.forward * lookDistance);
        /*center.rotation = Quaternion.Euler(0, current.x, 0);

        transform.position = center.position + center.TransformVector(camOffset);*/
    }
    private void OnDestroy()
    {
        MakeCursorActive(true);
    }
}

