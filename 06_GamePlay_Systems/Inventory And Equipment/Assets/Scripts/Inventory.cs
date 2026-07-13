using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    

    public GameObject slotPrefab;
    public int slotCount = 20;

    public List<Items> items;
    [HideInInspector] public List<GameObject> slots = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < slotCount; i++)
        {
            GameObject newSlot = Instantiate(slotPrefab, transform);
            slots.Add(newSlot);
        }
        for(int i = 0; i< items.Count; i++)
        {
            AddItemToSlot(i, items[i].Icon);
        }
    }

    
    void Update()
    {
        
    }

    public void AddItemToSlot(int slotIndex, Sprite icon)
    {
        Transform iconTransform = slots[slotIndex].transform.Find("Icon");
        if (iconTransform == null) return;

        Image iconImage = iconTransform.GetComponent<Image>();
        iconImage.sprite = icon;
        iconImage.enabled = true;
    }

    public void ClearSlot(int slotIndex)
    {
        Transform iconTransform = slots[slotIndex].transform.Find("Icon");
        if (iconTransform == null) return;

        Image iconImage = iconTransform.GetComponent<Image>();
        iconImage.sprite = null;
        iconImage.enabled = false;
    }
}
