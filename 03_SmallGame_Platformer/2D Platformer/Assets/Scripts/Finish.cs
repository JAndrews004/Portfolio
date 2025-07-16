using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Finish : MonoBehaviour
{

    public GameObject FinishUI;
    public TextMeshProUGUI TimeMsg;
    public TextMeshProUGUI ScoreMsg;
    public GameObject ScoreUI;
    private float time;
    private float StartTime;
    private int score;

    // Start is called before the first frame update
    void Start()
    {
        FinishUI.SetActive(false);
        StartTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ShowUI()
    {
        time = Time.time - StartTime;
        ScoreUI.SetActive(false);
        FinishUI.SetActive(true);

        TimeMsg.text = "You completed the level in: " + time + "s";
        ScoreMsg.text = "With a score of: " + score;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
        

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            score = collision.GetComponent<CollectableManager>().Collected;
            ShowUI();

        }
    }
}
