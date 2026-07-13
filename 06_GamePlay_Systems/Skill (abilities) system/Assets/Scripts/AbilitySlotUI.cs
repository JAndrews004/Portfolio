using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AbilitySlotUI : MonoBehaviour
{
    [Header("UI References (optional, auto-filled)")]
    public Image iconImage;          // Icon for the ability
    public Image cooldownOverlay;    // Radial cooldown fill

    private Ability ability;

    private void Awake()
    {
        // Auto-assign iconImage if not set
        if (iconImage == null)
        {
            // Assumes the icon Image is a child of this slot
            iconImage = GetComponentInChildren<Image>();
        }

        if (cooldownOverlay == null)
        {
            // Find the Image named "CooldownOverlay" in children (or first one)
            cooldownOverlay = transform.Find("CooldownOverlay")?.GetComponent<Image>();
        }
    }

    public void SetAbility(Ability newAbility)
    {
        ability = newAbility;

        if (ability != null)
        {
            if (iconImage != null)
            {
                iconImage.sprite = ability.icon;
                iconImage.enabled = true;
            }

            if (cooldownOverlay != null)
                cooldownOverlay.fillAmount = 0f;
        }
        else
        {
            if (iconImage != null) iconImage.enabled = false;
            if (cooldownOverlay != null) cooldownOverlay.fillAmount = 0f;
        }
    }

    public IEnumerator RunCooldown(float duration)
    {
        if (cooldownOverlay == null) yield break;

        float timeRemaining = duration;
        while (timeRemaining > 0f)
        {
            cooldownOverlay.fillAmount = timeRemaining / duration;
            timeRemaining -= Time.deltaTime;
            yield return null;
        }
        cooldownOverlay.fillAmount = 0f;
    }
}
