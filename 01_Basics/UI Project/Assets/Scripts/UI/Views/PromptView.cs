using UnityEngine;
using UnityEngine.UIElements;

public class PromptView : MonoBehaviour
{
    [SerializeField]
    HUD controller;

    Label m_PromptLabel;

    private const string PromptLabelName = "PromptLabel";

    private const string HiddenClassName = "hidden";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        VisualElement m_root = controller.GetUIDocument().rootVisualElement;
        m_PromptLabel = m_root.Q<Label>(PromptLabelName);
    }

    public void ChangePromptLabel(string label, bool hide)
    {
        m_PromptLabel.text = label;

        if (!hide)
        {
            m_PromptLabel.RemoveFromClassList(HiddenClassName);
            return;
        }

        m_PromptLabel.AddToClassList(HiddenClassName);
    }
}
