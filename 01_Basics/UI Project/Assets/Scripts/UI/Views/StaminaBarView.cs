using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class StaminaBarView : MonoBehaviour
{
    [SerializeField]
    HUD controller;

    VisualElement m_StaminaBar;

    private const string StaminaBarName = "StaminaBarFill";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        VisualElement m_root = controller.GetUIDocument().rootVisualElement;
        m_StaminaBar = m_root.Q<VisualElement>(StaminaBarName);
    }

    public void UpdateStaminaBar(float percentage)
    {
        if (m_StaminaBar == null) return;

        m_StaminaBar.style.width = Length.Percent(percentage * 100);
    }
}
