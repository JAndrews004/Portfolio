using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;


public class Health : MonoBehaviour
{
    [SerializeField] private Slider _slider; // slider component in unity project
    [SerializeField] private AnimationCurve _AddCurve; // curve component in unity project
    [SerializeField] private AnimationCurve _DamageCurve;
    [SerializeField] float time; //time to reach full health

    public Player player;
    int state = 0;
    float timeElapsed = 0.0f;
    float curvePoint = 0.0f;
    float ValueLeft = 20.0f;

    // Start is called before the first frame update
    void Start()
    {
        player = new Player();
        _slider.maxValue = player.GetHealth(); //setting max value of slider to players max hp
    }

    // Update is called once per frame
    void Update()
    {
        
        _slider.value = player.GetHealth();

        
        

        if (state == 1)
        {
            timeElapsed += Time.deltaTime;
            if (timeElapsed < time && ValueLeft >=0)
            {
                curvePoint = _AddCurve.Evaluate(timeElapsed / time);

                curvePoint -= player.previousChange;
                player.AddHealth(curvePoint * 20);
                player.previousChange = _AddCurve.Evaluate(timeElapsed / time);
                ValueLeft -= curvePoint * 20;
            }
            else
            {
                state = 0;
                timeElapsed = 0.0f;
                curvePoint = 0.0f;
                player.previousChange = 0;
                ValueLeft = 20.0f;
            }
        }
        if (state == 2)
        {
            timeElapsed += Time.deltaTime;
            if (timeElapsed < time && ValueLeft >= 0)
            {
                curvePoint = _DamageCurve.Evaluate(timeElapsed / time);

                curvePoint -= player.previousChange;
                player.TakeDamage(curvePoint * 20);
                player.previousChange = _DamageCurve.Evaluate(timeElapsed / time);
                ValueLeft -= curvePoint * 20;
            }
            else
            {
                state = 0;
                timeElapsed = 0.0f;
                curvePoint = 0.0f;
                player.previousChange = 0;
                ValueLeft = 20.0f;
            }
        }



    } 
}

public class Player //main player class//
{
    private float hp = 100.0f;
    public float previousChange =0;

    public void TakeDamage(float damage)
    {
        this.hp -= damage;
    }
    public float GetHealth()
    {
        return this.hp;
    }
    public void SetHealth(float value)
    {
        this.hp = value;
    }
    public void AddHealth(float health)
    {
        this.hp += health;
    }

}
