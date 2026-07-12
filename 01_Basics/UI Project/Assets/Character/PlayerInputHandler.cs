using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{

    public CharacterData data;

    public InputAction AddHealthAction;
    public InputAction TakeHealthAction;
    public InputAction AddStaminaAction;
    public InputAction TakeStaminaAction;
    public InputAction ToggleTooltipAction;

    public InputAction Ability1Action;
    public InputAction Ability2Action;
    public InputAction Ability3Action;

    private void OnEnable()
    {
        AddHealthAction.Enable();
        AddHealthAction.performed += data.AddHealth;

        TakeHealthAction.Enable();
        TakeHealthAction.performed += data.TakeDamage;

        AddStaminaAction.Enable();
        AddStaminaAction.performed += data.AddStamina;

        TakeStaminaAction.Enable();
        TakeStaminaAction.performed += data.TakeStamina;

        ToggleTooltipAction.Enable();
        ToggleTooltipAction.performed += data.ToggleTooltip;

        Ability1Action.Enable();
        Ability1Action.performed += data.ActivateAbility1;

        Ability2Action.Enable();
        Ability2Action.performed += data.ActivateAbility2;

        Ability3Action.Enable();
        Ability3Action.performed += data.ActivateAbility3;

    }

    public void OnDisable()
    {

        AddHealthAction.performed -= data.AddHealth;
        AddHealthAction.Disable();


        TakeHealthAction.performed -= data.TakeDamage;
        TakeHealthAction.Disable();

        AddStaminaAction.performed -= data.AddStamina;
        AddStaminaAction.Disable();

        TakeStaminaAction.performed -= data.TakeStamina;
        TakeStaminaAction.Disable();


        ToggleTooltipAction.performed -= data.ToggleTooltip;
        ToggleTooltipAction.Disable();

        Ability1Action.performed -= data.ActivateAbility1;
        Ability1Action.Disable();

        Ability2Action.performed -= data.ActivateAbility2;
        Ability2Action.Disable();

        Ability3Action.performed -= data.ActivateAbility3;
        Ability3Action.Disable();
    }
}
