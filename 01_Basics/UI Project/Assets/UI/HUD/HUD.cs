using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class HUD : MonoBehaviour
{
    [SerializeField] UIDocument UiDoc;
    [SerializeField] CharacterData data;
    [SerializeField] HealthBarView health;
    [SerializeField] StaminaBarView stamina;
    [SerializeField] ObjectiveView quest;
    [SerializeField] AbilityBarView abilities;
    [SerializeField] PromptView prompt;


    public VisualElement m_root;

    Label m_LevelLabel;
    private const string LevelLabelName = "LevelLabel";

    void Start()
    {
        m_root = UiDoc.rootVisualElement;
        
        m_LevelLabel = m_root.Q<Label>(LevelLabelName);
        
        health.UpdateHealthBar(data.GetHPPercentage());
        stamina.UpdateStaminaBar(data.GetStaminaPercentage());
        quest.UpdateQuest(data.currentQuest);
        UpdateLevel(data.GetLevel());
    }

    private void OnEnable()
    {
        data.OnHealthChange += health.UpdateHealthBar;
        data.OnStaminaChange += stamina.UpdateStaminaBar;
        data.OnLevelChange += UpdateLevel;
        data.OnQuestChange += quest.UpdateQuest;
        data.TriggerPromptText += prompt.ChangePromptLabel;
        data.abilitySystem.AbilityTimerUpdate += abilities.UpdateAbilitycooldowns;
}


    private void OnDisable()
    {
        data.OnHealthChange -= health.UpdateHealthBar;
        data.OnStaminaChange -= stamina.UpdateStaminaBar;
        data.OnLevelChange -= UpdateLevel;
        data.OnQuestChange -= quest.UpdateQuest;
        data.TriggerPromptText -= prompt.ChangePromptLabel;
        data.abilitySystem.AbilityTimerUpdate -= abilities.UpdateAbilitycooldowns;
    }
    

    private void UpdateLevel(int level)
    {
        if (m_LevelLabel == null) return;
        m_LevelLabel.text = level.ToString();
    }

    public UIDocument GetUIDocument()
    {
        return UiDoc;
    }



}
