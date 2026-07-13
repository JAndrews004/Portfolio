using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float BaseAttack = 10;
    public float BaseDefense = 10;
    public float BaseHealth = 100;

    public float Attack;
    public float Defense;
    public float Health;

    public GameObject StatPageUI;
    public TextMeshProUGUI AttackStat;
    public TextMeshProUGUI DefenseStat;
    public TextMeshProUGUI HealthStat;

    
    // Start is called before the first frame update
    void Start()
    {
        StatPageUI.SetActive(false);
        Attack = BaseAttack; 
        Defense = BaseDefense;
        Health = BaseHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && StatPageUI.activeSelf)
        {
            StatPageUI.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.P) && !StatPageUI.activeSelf)
        {
            StatPageUI.SetActive(true);
        }
        AttackStat.text = ("Attack: " + Attack);
        DefenseStat.text = ("Defense:" + Defense);
        HealthStat.text = ("Health" + Health);
    }

    public void SetStats(float attack, float defense, float health)
    {
        Attack = attack;
        Defense = defense;
        Health = health;
    }

    
}
