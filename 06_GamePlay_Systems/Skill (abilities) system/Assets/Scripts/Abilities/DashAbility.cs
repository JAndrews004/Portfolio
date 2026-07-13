using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Dash")]
public class DashAbility : Ability
{
    public float DashForce;

    public void OnEnable()
    {
        CanActivate = true;
    }

    public override void Activate(GameObject user, MonoBehaviour runner)
    {
        Rigidbody rb = user.GetComponent<Rigidbody>();
        //Debug.Log("dash activated " + CanActivate + " "+ ManaCost);
        if (user.GetComponent<ManaSystem>().EnoughMana(ManaCost) && CanActivate)
        {
            user.GetComponent<ManaSystem>().UseMana(ManaCost);
            CanActivate = false;
            runner.StartCoroutine(CoolDown(cooldown));

            var dashEffect = Camera.main.GetComponent<DashEffectController>();
            if (dashEffect != null)
            {
                dashEffect.PlayDashEffect(0.3f, 0.7f);
            }

            // add force in direction x and z velocity is eg if moving backwards dash backwards
            rb.AddForce(new Vector3(rb.velocity.x,0,rb.velocity.z).normalized * DashForce, ForceMode.Impulse);

            CameraShake Shake = Camera.main.GetComponent<CameraShake>();
            if (Shake != null)
            {
                Shake.StartShake(0.2f, 0.15f);
            }
            
        }
       
    }

    public override IEnumerator CoolDown(float time)
    {
        yield return new WaitForSeconds(time);
        CanActivate = true;
    }
    
}