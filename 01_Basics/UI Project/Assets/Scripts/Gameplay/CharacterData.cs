using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Timeline.DirectorControlPlayable;

public class CharacterData : MonoBehaviour
{

    public AbilitySystem abilitySystem;

    public string currentQuest = "Find the captain of the guard and steal the key.";
    private int HP = 100;
    private int MaxHP = 100;
    private int MaxStamina = 100;
    private int Stamina = 100;
    private int level = 3;
    private bool tooltip = false;


    public Action<float> OnHealthChange;
    public Action<float> OnStaminaChange;
    public Action<int> OnLevelChange;
    public Action<string> OnQuestChange;

    public Action<string, bool> TriggerPromptText;

    public void TakeDamage(InputAction.CallbackContext context)
    {
        int damage = UnityEngine.Random.Range(10, MaxHP / 2);
        if(HP - damage <= 0)
        {
            HP = 0;
        }
        else
        {
            HP -= damage;
        }
        Debug.Log(GetHPPercentage());
        OnHealthChange?.Invoke(GetHPPercentage());
    }

    public void AddHealth(InputAction.CallbackContext context)
    {
        int heal = UnityEngine.Random.Range(10, MaxHP / 2);
        if (HP + heal >= MaxHP)
        {
            HP = MaxHP;
        }
        else
        {
            HP += heal;
        }
        Debug.Log(GetHPPercentage());
        OnHealthChange?.Invoke(GetHPPercentage());
    }

    public void TakeStamina(InputAction.CallbackContext context)
    {
        int stamina = UnityEngine.Random.Range(10, MaxStamina / 2);
        if (Stamina - stamina <= 0)
        {
            Stamina = 0;
        }
        else
        {
            Stamina -= stamina;
        }
        Debug.Log(GetStaminaPercentage());
        OnStaminaChange?.Invoke(GetStaminaPercentage());
    }

    public void AddStamina(InputAction.CallbackContext context)
    {
        int stamina = UnityEngine.Random.Range(10, MaxStamina / 2);
        if (Stamina + stamina >= MaxStamina)
        {
            Stamina = MaxStamina;
        }
        else
        {
            Stamina += stamina;
        }
        Debug.Log(GetStaminaPercentage());
        OnStaminaChange?.Invoke(GetStaminaPercentage());
    }

    public void IncreaseLevel(InputAction.CallbackContext context)
    {
        level ++;

        OnLevelChange?.Invoke(level);
    }

    public void UpdateQuest(string newQuest)
    {
        currentQuest = newQuest;
        OnQuestChange?.Invoke(newQuest);
    }
    public void ToggleTooltip(InputAction.CallbackContext context)
    {
        tooltip = !tooltip;

        TriggerPromptText.Invoke("Use the key to escape.", tooltip);

    }

    public void ActivateAbility1(InputAction.CallbackContext context)
    {
        abilitySystem.ActivateAbility(0);
    }

    public void ActivateAbility2(InputAction.CallbackContext context)
    {
        abilitySystem.ActivateAbility(1);
    }

    public void ActivateAbility3(InputAction.CallbackContext context)
    {
        abilitySystem.ActivateAbility(2);
    }


    public int GetHP() { return HP; }
    public int GetMaxHP() { return MaxHP; }
    public int GetStamina() { return Stamina; }
    public int GetMaxStamina() {return MaxStamina; }
    public int GetLevel() { return level; }
    public float GetHPPercentage() {  return (float)HP / (float)MaxHP; }
    public float GetStaminaPercentage() {  return (float)Stamina / (float)MaxStamina; }

    


}
