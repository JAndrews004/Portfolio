using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Fireball")]
public class FireBallAbility : Ability
{
    public int Damage;
    public float LifeSpan;
    public float ProjSpeed;
    public GameObject prefab;

    public void OnEnable()
    {
        CanActivate = true;
    }
    public override void Activate(GameObject user, MonoBehaviour runner)
    {
        //Debug.Log("fireball activated " + CanActivate + " " + ManaCost);
        if (user.GetComponent<ManaSystem>().EnoughMana(ManaCost) && CanActivate)
        {
            user.GetComponent<ManaSystem>().UseMana(ManaCost);
            CanActivate = false;
            runner.StartCoroutine(CoolDown(cooldown));

            //user.GetComponent<Transform>().Position()
            //Camera.main.GetComponent<Transform>().Rotation()

            Spawn(user.GetComponent<Transform>().position + Camera.main.transform.forward * 2f, Camera.main.GetComponent<Transform>().rotation,runner);
            

        }
       
    }
    public override IEnumerator CoolDown(float time)
    {
        yield return new WaitForSeconds(time);
        CanActivate = true;
    }
    public IEnumerator DelayDespawn(float time,GameObject obj)
    {
        yield return new WaitForSeconds(time);
        if (obj != null)
        {
            Despawn(obj);
        }
    }
    public void Spawn(Vector3 pos, Quaternion rot, MonoBehaviour runner)
    {
        var obj = Instantiate(prefab, pos, rot);
        
        if (obj.GetComponent<FireBallCollision>())
        {
            obj.GetComponent<FireBallCollision>().damage = Damage;
        }
        runner.StartCoroutine(DelayDespawn(LifeSpan, obj));
        AddVelocity(obj);
    }

    public void Despawn(GameObject obj)
    {
        if (obj != null)
        {
            Destroy(obj);
        }
    }
    public void AddVelocity(GameObject obj)
    {
        obj.GetComponent<Rigidbody>().velocity += Camera.main.transform.forward * ProjSpeed;
    }
}


//on Activate Spawn prefab -> from player pos + in direction camera is looking, start coroutine to despawn after time, Add velocity in direction camera is looking, (check collision take health and despawn) ->script on prefab