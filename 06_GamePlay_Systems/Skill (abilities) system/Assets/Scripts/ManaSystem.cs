using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class ManaSystem : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("UI")]
    public Slider manaSlider;
    [Header("Settings")]
    public int MaxMana = 100;
    public int CurrentMana;
    public float RegenTime = 0.5f;

    Coroutine ManaRegenCoroutine;
    void Start()
    {
        CurrentMana = MaxMana;
        manaSlider.maxValue = MaxMana;
    }

    // Update is called once per frame
    void Update()
    {
        manaSlider.value = CurrentMana;
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (EnoughMana(10))
            {
                UseMana(10);
            }
        }
    }

    public void UseMana(int mana)
    {
        StopRegen();
        if (CurrentMana - mana >= 0)
        {
            CurrentMana -= mana;
        }
        StartRegen();
    }

    public bool EnoughMana(int mana)
    {
        if (CurrentMana - mana >= 0)
        {
            return true;
        }
        else { return false; }
    }

    IEnumerator regenManaAfterTime(float time, int mana)
    {
        yield return new WaitForSeconds(time);
        if (CurrentMana + mana >= MaxMana)
        {
            CurrentMana = MaxMana;
        }
        else
        {
            CurrentMana += mana;
            StartRegen();
        }
    }
    public void StopRegen()
    {
        if (ManaRegenCoroutine != null)
        {
            StopCoroutine(ManaRegenCoroutine);
            ManaRegenCoroutine = null;
        }
    }

    void StartRegen()
    {
        ManaRegenCoroutine = StartCoroutine(regenManaAfterTime(RegenTime, (int)(MaxMana * 0.1f)));
    }
}
