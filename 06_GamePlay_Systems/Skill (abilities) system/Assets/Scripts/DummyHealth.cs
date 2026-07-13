using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DummyHealth : MonoBehaviour
{

    [Header("Health Settings")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("UI Settings")]
    public Slider healthSlider;       // Assign the Slider in the Inspector
    public Transform uiTransform;     // The Canvas that holds the Slider
    public float verticalOffset = 2f; // How high above the object the bar should sit

    private Transform cam;

    void Start()
    {
        cam = Camera.main.transform;

        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
        
    }

    private void Update()
    {
        if (currentHealth == 0)
        {
            StartCoroutine(Heal());
        }
    }
    void LateUpdate()
    {
        // Position UI just above object
        if (uiTransform != null)
        {
            uiTransform.position = transform.position + Vector3.up * verticalOffset;

            // Always face the camera
            uiTransform.LookAt(uiTransform.position + cam.forward);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

        healthSlider.value = currentHealth;
    }

    public IEnumerator Heal()
    {
        yield return new WaitForSeconds(5f);

        currentHealth = maxHealth;
        healthSlider.value = currentHealth;
    }
}
