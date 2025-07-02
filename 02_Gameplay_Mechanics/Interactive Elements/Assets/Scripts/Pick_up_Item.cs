using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Pick_up_Item : MonoBehaviour
{

    private bool playerInRange = false;
    private bool isCollected = false;
    private GameObject Item;
    public GameObject promptUI;
    public GameObject healthBar;
    private Health health;

    // Start is called before the first frame update
    void Start()
    {
        promptUI.SetActive(false);
        Item = this.gameObject;
        health = healthBar.GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
       
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !isCollected)
        {
            Item.gameObject.SetActive(false);
            promptUI.SetActive(false);
            isCollected = true;
            health.player.AddHealth(20);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Triggered with: " + other.name);
        if (other.CompareTag("Player") && !isCollected)
        {
            playerInRange = true;
            promptUI.SetActive(true);

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            promptUI.SetActive(false);
        }
    }
}
