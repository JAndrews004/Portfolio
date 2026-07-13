using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Shield")]
public class DefenseAbility : Ability
{
    public float ShieldedTime;
    public string ShieldObjectName;
    private Transform Shield;
    public void OnEnable()
    {
        CanActivate = true;
        
    }

    public override void Activate(GameObject user, MonoBehaviour runner)
    {
        Shield = user.transform.Find(ShieldObjectName);
        if (user.GetComponent<ManaSystem>().EnoughMana(ManaCost) && CanActivate)
        {
            
            user.GetComponent<ManaSystem>().UseMana(ManaCost);

            CanActivate = false;
            Shield.gameObject.SetActive(true);

            runner.StartCoroutine(CoolDown(cooldown));
            //set user to take no damage
            runner.StartCoroutine(ShieldTime(ShieldedTime));
            
        }
        
    }

    public override IEnumerator CoolDown(float time)
    {
        yield return new WaitForSeconds(time);
        CanActivate = true;
        
    }
    IEnumerator ShieldTime(float time)
    {
        yield return new WaitForSeconds(time);
        Shield.gameObject.SetActive(false);
        //set user to take damage again
    }
}
