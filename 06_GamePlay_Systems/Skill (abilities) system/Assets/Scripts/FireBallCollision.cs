using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallCollision : MonoBehaviour
{
    public int damage;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<DummyHealth>())
        {
            collision.gameObject.GetComponent<DummyHealth>().TakeDamage(damage);
        }
        if (collision != null)
        {
            Destroy(gameObject);
        }

    }
}
