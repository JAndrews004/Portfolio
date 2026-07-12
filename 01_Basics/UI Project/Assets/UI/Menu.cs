using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Menu : MonoBehaviour
{
    [SerializeField] UIDocument UiDoc;
    [SerializeField] Settings settings;

    VisualElement m_root;
    VisualElement m_SettingsContainer;
    VisualElement m_CreditsContainer;
    Label m_CreditsContents;

    private const string SettingsContainerName = "SettingsContainer";
    private const string CreditsContainerName = "CreditsContainer";
    private const string CreditsContentsContainerName = "CreditsContents";

    private const string PlayButtonName = "PlayButton";
    private const string SettingsButtonName = "SettingsButton";
    private const string CreditsButtonName = "CreditsButton";
    private const string QuitButtonName = "QuitButton";

    private const string SettingsCloseButtonName = "CloseSettingsButton";
    private const string CreditsCloseButtonName = "CloseCreditsButton";

    Button m_PlayButton;
    Button m_SettingsButton;
    Button m_CreditsButton;
    Button m_QuitButton;

    Button m_SettingsCloseButton;
    Button m_CreditsCloseButton;

    private const string HiddenClassName = "hidden";
    private const string SettingsHiddenClassName = "settings-hidden";
    private const string CreditContainerClassName = "credits-container";
    private const string CreditStartClassName = "credits-start";


    void OnEnable()
    {
        m_root = UiDoc.rootVisualElement;
        m_SettingsContainer = m_root.Q<VisualElement>(SettingsContainerName);
        m_CreditsContainer = m_root.Q<VisualElement>(CreditsContainerName);

        m_PlayButton = m_root.Q<Button>(PlayButtonName);
        m_SettingsButton = m_root.Q<Button>(SettingsButtonName);
        m_CreditsButton = m_root.Q<Button>(CreditsButtonName);
        m_QuitButton = m_root.Q<Button>(QuitButtonName);

        m_SettingsCloseButton = m_root.Q<Button>(SettingsCloseButtonName);
        m_CreditsCloseButton = m_root.Q<Button>(CreditsCloseButtonName);

        m_CreditsContents = m_root.Q<Label>(CreditsContentsContainerName);

        m_PlayButton.clicked += PlayGame;
        m_SettingsButton.clicked += OpenSettings;
        m_SettingsCloseButton.clicked += CloseSettings;
        m_CreditsButton.clicked += OpenCredits;
        m_CreditsCloseButton.clicked += CloseCredits;
        m_QuitButton.clicked += QuitGame;

    }
    private void OnDisable()
    {
        m_PlayButton.clicked -= PlayGame;
        m_SettingsButton.clicked -= OpenSettings;
        m_SettingsCloseButton.clicked -= CloseSettings;
        m_CreditsButton.clicked -= OpenCredits;
        m_CreditsCloseButton.clicked -= CloseCredits;
        m_QuitButton.clicked -= QuitGame;
    }
    private void PlayGame()
    {
        SceneTransitionController.Instance.LoadScene("MainGame");
    }
    
    private void OpenCredits()
    {
        m_CreditsContainer.RemoveFromClassList(HiddenClassName);
        m_CreditsContents.AddToClassList(CreditStartClassName);

    }
    private void CloseCredits()
    {
        m_CreditsContainer.AddToClassList(HiddenClassName);
        m_CreditsContents.RemoveFromClassList(CreditStartClassName);

    }

    private void OpenSettings()
    {
        settings.OpenSettings(m_SettingsContainer,SettingsHiddenClassName);
    }
    private void CloseSettings()
    {
        settings.CloseSettings(m_SettingsContainer, SettingsHiddenClassName);
    }

    private void QuitGame()
    {
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
