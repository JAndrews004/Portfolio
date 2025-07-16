using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public List<GameObject> RSPonits;
    public GameObject Player;
    private GameObject CurrentPoint;
    private List<GameObject> VisitedPoints = new List<GameObject>();
    private bool IsDead;
    
    // Start is called before the first frame update
    void Start()
    {
        CurrentPoint = RSPonits[0];
        VisitedPoints.Add(RSPonits[0]);
        
    }

    // Update is called once per frame
    void Update()
    {
        
        CheckDeath();
    }

    public void NewRSP(GameObject newRSP)
    {
        for (int i = 0; i < VisitedPoints.Count; i++)
        {
            if (VisitedPoints[i] == newRSP || newRSP == null)
            {
                return;
            }
        }
        
        CurrentPoint = newRSP;
        VisitedPoints.Add(CurrentPoint);
    }

    public void OnDeath()
    { 
        Player.transform.position = new Vector2(CurrentPoint.transform.position.x, CurrentPoint.transform.position.y);
        Time.timeScale = 1f;
        Player.GetComponent<DeathHandler>().DeathUI.SetActive(false);
        Player.GetComponent<DeathHandler>().IsDead = false;
        Player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    void CheckDeath()
    {
        IsDead = Player.GetComponent<DeathHandler>().IsDead;
        if (IsDead && Input.GetKey(KeyCode.R))
        {
            OnDeath();
        }
    }
}
