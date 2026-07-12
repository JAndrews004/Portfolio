using UnityEngine;
using UnityEngine.UIElements;

public class ObjectiveView : MonoBehaviour
{
    [SerializeField]
    HUD controller;

    Label m_ObjectiveLabel;

    private const string ObjectiveLabelName = "CurrentObjective";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        VisualElement m_root = controller.GetUIDocument().rootVisualElement;
        m_ObjectiveLabel = m_root.Q<Label>(ObjectiveLabelName);

    }

    public void UpdateQuest(string questText)
    {
        if (m_ObjectiveLabel == null) return;
        m_ObjectiveLabel.text = questText;
    }
}
