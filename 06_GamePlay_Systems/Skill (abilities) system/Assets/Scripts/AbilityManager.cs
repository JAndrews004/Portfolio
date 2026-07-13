using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public Ability[] abilities;
    public KeyCode[] abilityKeys;
    public AbilitySlotUI[] abilitySlots;


    void Start()
    {
        // Hook up UI at start
        for (int i = 0; i < abilitySlots.Length; i++)
        {
            if (i < abilities.Length)
                abilitySlots[i].SetAbility(abilities[i]);
        }
    }

    void Update()
    {
        //Debug.Log(abilities[0].CanActivate);
        for (int i = 0; i < abilities.Length; i++)
        {
            if (abilities[i] != null && Input.GetKeyDown(abilityKeys[i]))
            {
                abilities[i].Activate(gameObject,this);
                StartCoroutine(abilitySlots[i].RunCooldown(abilities[i].cooldown));
            }
        }

    }
    
}
