using UnityEngine;
using UnityEngine.UIElements;

public class AbilityBarView : MonoBehaviour
{
    [SerializeField]
    HUD controller;

    private VisualElement[] m_AbilityFills;

    private const string Ability1FillName = "Ability1Fill";
    private const string Ability2FillName = "Ability2Fill";
    private const string Ability3FillName = "Ability3Fill";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        VisualElement m_root = controller.GetUIDocument().rootVisualElement;

        m_AbilityFills = new VisualElement[]
            {
                m_root.Q<VisualElement>(Ability1FillName),
                m_root.Q<VisualElement>(Ability2FillName),
                m_root.Q<VisualElement>(Ability3FillName)
            };
    }

    public void UpdateAbilitycooldowns(int index, float percentage)
    {

        m_AbilityFills[index].style.height = Length.Percent(percentage);

    }
}
