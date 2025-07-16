using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = 1f;
    }
    public void Play()
    {
        SceneManager.LoadScene(sceneName: "Level1");
    }
    public void quit()
    {
        Application.Quit();
    }
}
