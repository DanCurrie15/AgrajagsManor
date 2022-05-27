using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public bool GameOn { private set; get; }
    public GameObject playerChar;

    // Start is called before the first frame update
    void Start()
    {
        GameOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameOver()
    {
        UIManager.Instance.ShowGameOverPanel();
        GameOn = false;
    }
}
