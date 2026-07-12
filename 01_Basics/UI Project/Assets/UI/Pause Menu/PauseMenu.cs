using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] UIDocument UiDoc;
    [SerializeField] Settings settings;

    VisualElement m_root;
    VisualElement m_SettingsContainer;
    VisualElement m_Background;

    private const string SettingsContainerName = "SettingsContainer";
    private const string BackgroundContainerName = "Background";

    private const string ResumeButtonName = "ResumeButton";
    private const string SettingsButtonName = "SettingsButton";
    private const string MainMenuButtonName = "MainMenuButton";

    private const string SettingsCloseButtonName = "CloseSettingsButton";

    private const string HiddenClassName = "hidden";
    private const string SettingsHiddenClassName = "settings-hidden";

    Button m_ResumeButton;
    Button m_SettingsButton;
    Button m_MainMenuButton;

    Button m_SettingsCloseButton;

    public InputAction pauseAction;

    void OnEnable()
    {
        m_root = UiDoc.rootVisualElement;
        m_SettingsContainer = m_root.Q<VisualElement>(SettingsContainerName);
        m_Background = m_root.Q<VisualElement>(BackgroundContainerName);

        m_ResumeButton = m_root.Q<Button>(ResumeButtonName);
        m_SettingsButton = m_root.Q<Button>(SettingsButtonName);
        m_MainMenuButton = m_root.Q<Button>(MainMenuButtonName);

        m_SettingsCloseButton = m_root.Q<Button>(SettingsCloseButtonName);


        m_ResumeButton.clicked += ResumeGame;
        m_SettingsButton.clicked += OpenSettings;
        m_SettingsCloseButton.clicked += CloseSettings;
        
        m_MainMenuButton.clicked += MainMenu;

        pauseAction.Enable();
        pauseAction.performed += OnPausePressed;
    }

    private void OnDisable()
    {
        m_ResumeButton.clicked -= ResumeGame;
        m_SettingsButton.clicked -= OpenSettings;
        m_SettingsCloseButton.clicked -= CloseSettings;
        m_MainMenuButton.clicked -= MainMenu;

        pauseAction.performed -= OnPausePressed;
        pauseAction.Disable();
    }

    private void MainMenu()
    {
        ResumeGame();
        SceneTransitionController.Instance.LoadScene("MainMenu");
    }

    private void OpenSettings()
    {
        settings.OpenSettings(m_SettingsContainer, SettingsHiddenClassName);
    }
    private void CloseSettings()
    {
        settings.CloseSettings(m_SettingsContainer, SettingsHiddenClassName);
    }

    private void ResumeGame()
    {
        m_Background.AddToClassList(HiddenClassName);
        Time.timeScale = 1;
    }

    private void OnPausePressed(InputAction.CallbackContext context)
    {
        PauseGame();
    }

    private void PauseGame()
    {
        m_Background.RemoveFromClassList(HiddenClassName);
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
