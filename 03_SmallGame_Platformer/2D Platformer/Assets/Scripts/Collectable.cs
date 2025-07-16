using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{

    [SerializeField]
    public int value;
    private bool collected = false;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponentInParent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CollectableManager CM = collision.GetComponent<CollectableManager>();
            if (CM != null && collected == false)
            {
                CM.Collected += value;
                audioSource.Play();
                collected = true;
                gameObject.SetActive(false);
            }

        }
    }
}
