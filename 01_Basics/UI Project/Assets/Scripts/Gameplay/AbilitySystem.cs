using System;
using UnityEngine;

public class AbilitySystem : MonoBehaviour
{

    [SerializeField]
    private float[] abilityCooldowns = { 2f, 5f, 8f };

    private float[] abilityCooldownsLeft = new float[3];

    public Action<int, float> AbilityTimerUpdate;
    private void Update()
    {
        for (int i = 0; i < abilityCooldownsLeft.Length; i++)
        {
            if (abilityCooldownsLeft[i] > 0)
            {
                abilityCooldownsLeft[i] -= Time.deltaTime;

                Debug.Log($"Ability {i} : {abilityCooldownsLeft[i]}");
                if (abilityCooldownsLeft[i] < 0)
                    abilityCooldownsLeft[i] = 0;

                AbilityTimerUpdate?.Invoke(i, (abilityCooldownsLeft[i] / abilityCooldowns[i]) * 100);
            }
        }
    }

    public void ActivateAbility(int index)
    {
        if (abilityCooldownsLeft[index] > 0)
            return;

        abilityCooldownsLeft[index] = abilityCooldowns[index];

        Debug.Log($"Activated Ability {index + 1}");
    }

    public float GetCooldownLeftAsPercentage(int abilityIndex)
    {
        if (abilityIndex < 0 || abilityIndex >= abilityCooldowns.Length)
            return 0;

        return (abilityCooldownsLeft[abilityIndex] / abilityCooldowns[abilityIndex]);
    }
}
