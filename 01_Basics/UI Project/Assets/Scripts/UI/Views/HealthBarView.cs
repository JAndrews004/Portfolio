using UnityEngine;
using UnityEngine.UIElements;

public class HealthBarView : MonoBehaviour
{
    [SerializeField]
    HUD controller;

    VisualElement m_HPBar;
    VisualElement m_DamageBar;

    private const string HPBarName = "HealthBarFill";
    private const string HPBarDamageName = "DamageBar";
    private const string HPBarBackgroundName = "HealthBarBackground";


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        VisualElement m_root = controller.GetUIDocument().rootVisualElement;
        m_HPBar = m_root.Q<VisualElement>(HPBarName);
        //m_Background = m_root.Q<VisualElement>(HPBarBackgroundName);
        m_DamageBar = m_root.Q<VisualElement>(HPBarDamageName);
    }

    public void UpdateHealthBar(float percentage)
    {
        if (m_HPBar == null) return;

        m_HPBar.style.width = Length.Percent(percentage * 100);
        m_DamageBar.style.width = Length.Percent(percentage * 100);
    }
}
