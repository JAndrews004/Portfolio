using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class DoorOpening : MonoBehaviour
{
    public float RaycastDistance = 10f;
    public GameObject promptUI;

    // Start is called before the first frame update
    void Start()
    {
        promptUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(this.gameObject.transform.position, this.gameObject.transform.forward);
        RaycastHit hitInfo;
        promptUI.SetActive(false);
        if (Physics.Raycast(ray, out hitInfo, RaycastDistance))
        {

            if(hitInfo.transform.tag == "Door")
            {
                //show ui to open door
                //allow for input and check to play animation
                promptUI.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    if(hitInfo.rigidbody != null)
                    {
                        float yAngle = (90+hitInfo.transform.rotation.eulerAngles.y)*Mathf.Deg2Rad;
                        DoorBehaviour myScript = hitInfo.collider.GetComponent<DoorBehaviour>();
                        myScript.changeDoorState(yAngle);
                    }

                }
            }
        }

        

    }

    
        
    
}
