using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SceneTransitionController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private UIDocument uiDocument;

    [Header("Settings")]
    [SerializeField] private float fadeDuration = 0.5f;

    private VisualElement m_fadeOverlay;

    private static SceneTransitionController instance;

    public static SceneTransitionController Instance => instance;

    private void Awake()
    {
        // Singleton
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;

        InitialiseUI();
    }

    private IEnumerator Start()
    {
        // Start black
        m_fadeOverlay.style.opacity = 1f;
        m_fadeOverlay.pickingMode = PickingMode.Position;

        // Wait one frame so the UI is drawn
        yield return null;

        // Fade into the scene
        yield return FadeOut();
    }

    private void OnDestroy()
    {
        if (instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void InitialiseUI()
    {
        if (uiDocument == null)
        {
            Debug.LogError("SceneTransitionController: UIDocument is not assigned.");
            return;
        }

        m_fadeOverlay = uiDocument.rootVisualElement.Q<VisualElement>("FadeOverlay");

        if (m_fadeOverlay == null)
        {
            Debug.LogError("SceneTransitionController: Could not find VisualElement named 'FadeOverlay'.");
            return;
        }

        m_fadeOverlay.style.opacity = 0f;
        m_fadeOverlay.pickingMode = PickingMode.Ignore;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(FadeInAfterSceneLoad());
    }

    private IEnumerator FadeInAfterSceneLoad()
    {
        yield return null;

        InitialiseUI();

        yield return FadeOut();
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        yield return FadeIn();

        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator FadeIn()
    {
        m_fadeOverlay.pickingMode = PickingMode.Position;
        m_fadeOverlay.style.opacity = 1f;

        yield return new WaitForSeconds(fadeDuration);
    }

    private IEnumerator FadeOut()
    {
        m_fadeOverlay.style.opacity = 0f;

        yield return new WaitForSeconds(fadeDuration);

        m_fadeOverlay.pickingMode = PickingMode.Ignore;
    }
}