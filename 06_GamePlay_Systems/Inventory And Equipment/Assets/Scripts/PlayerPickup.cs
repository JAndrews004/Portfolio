using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using static UnityEditor.Progress;

public class PlayerPickup : MonoBehaviour
{

    public GameObject PickupUI;
    public GameObject InventoryPageUI;

    private bool CanPickup = false;
    private ItemPickup pickup;
    public void PickupItem(ItemPickup Item)
    {
        Items item = Item.GetItem();
        Debug.Log("Picked up: " + item.Name);
        CanPickup = false;
        PickupUI.SetActive(false);
        // TODO: Add to inventory list later
        Destroy(Item.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        pickup = other.GetComponent<ItemPickup>();
        if (pickup != null)
        {
            PickupUI.SetActive(true);
            CanPickup = true;
            
        }
    }
    private void OnTriggerExit(Collider other) 
    {
        pickup = other.GetComponent<ItemPickup>();
        if (pickup != null)
        {
            PickupUI.SetActive(false);
            CanPickup = false;
            
        }
    }

    private void Update()
    {
        if (CanPickup && Input.GetKeyDown(KeyCode.E))
        {
            PickupItem(pickup);
        }

        if (Input.GetKeyDown(KeyCode.I) && InventoryPageUI.activeSelf)
        {
            InventoryPageUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Camera.main.GetComponent<MouseMovement>().enabled = true;
        }
        else if (Input.GetKeyDown(KeyCode.I) && !InventoryPageUI.activeSelf)
        {
            InventoryPageUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Camera.main.GetComponent<MouseMovement>().enabled = false;
        }

    }


}
