using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/BaseAbility")]
public abstract class Ability : ScriptableObject
{
    public string abilityName;
    public float cooldown;
    public int ManaCost;
    public bool CanActivate = true;
    public Sprite icon;
    public abstract void Activate(GameObject user, MonoBehaviour runner);
    public abstract IEnumerator CoolDown(float time);
}



