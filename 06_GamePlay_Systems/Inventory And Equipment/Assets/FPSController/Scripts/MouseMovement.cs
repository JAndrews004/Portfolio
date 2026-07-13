using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{

    public float senseX = 1000f;
    public float senseY = 1000f;
    public Transform orientation;

    float xRotation;
    float YRotation;

    void Start()
    {
        //Locking the cursor to the middle of the screen and making it invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * senseX * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * senseY * Time.deltaTime;

        //control rotation around x and y axis (Look up and down)
        xRotation -= mouseY;
        YRotation += mouseX;

        //we clamp the rotation so we cant Over-rotate (like in real life)
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //applying both rotations
        transform.rotation = Quaternion.Euler(xRotation, YRotation, 0f);
        orientation.rotation = Quaternion.Euler(0, YRotation, 0f);

    }
}