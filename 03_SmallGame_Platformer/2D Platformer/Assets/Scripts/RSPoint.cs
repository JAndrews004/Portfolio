using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RSPoint : MonoBehaviour
{
    private TextMeshPro text;
    private AudioSource audioSource;

    public AudioClip clip;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponentInChildren<TextMeshPro>();
        text.gameObject.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            RespawnManager RSPM = collision.GetComponent<RespawnManager>();
            if (RSPM != null)
            {
                RSPM.NewRSP(gameObject);
                text.gameObject.SetActive(true);
                audioSource.PlayOneShot(clip);
                StartCoroutine(DisableTextAfterDelay(2f));
            }
            
        }
    }

    IEnumerator DisableTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        text.gameObject.SetActive(false); // or text.enabled = false;
    }
}
