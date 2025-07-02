using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public GameObject healthBar;
    private Health health;

    // Start is called before the first frame update
    void Start()
    {
        health = healthBar.GetComponent<Health>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, Vector3.down, out hitInfo, 10f))
        {
            if(hitInfo.transform.tag == "Damage")
            {
                health.player.TakeDamage(0.1f);
            }
        }
    }
}
