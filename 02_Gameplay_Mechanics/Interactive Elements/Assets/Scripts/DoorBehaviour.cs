using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    public Rigidbody rb;
    private GameObject door;
    private bool isOpen = false;
    // Start is called before the first frame update
    void Start()
    {
        door = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeDoorState(float yAngle)
    {
        if (isOpen)
        {
            float xLength = -1*door.transform.localScale.x;
            rb.MovePosition(new Vector3(xLength * Mathf.Sin(yAngle) + door.transform.position.x, 0 + door.transform.position.y, xLength * Mathf.Cos(yAngle) + door.transform.position.z));
            isOpen = false;
        }
        else
        {
            float xLength = door.transform.localScale.x;
            rb.MovePosition(new Vector3(xLength * Mathf.Sin(yAngle) + door.transform.position.x, 0 + door.transform.position.y, xLength * Mathf.Cos(yAngle) + door.transform.position.z));
            isOpen = true;
        }
        
        
    }
}
