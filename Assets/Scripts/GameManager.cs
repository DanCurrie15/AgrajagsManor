using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public bool GameOn { private set; get; }
    public GameObject playerChar;
    public int KillCount { private set; get; }
    [SerializeField]
    private int newPossessionThreshold = 2;

    public void StartGame()
    {
        UIManager.Instance.HideStartGamePanel();
        GameOn = true;
    }

    public void GameOver()
    {
        UIManager.Instance.ShowGameOverPanel();
        GameOn = false;
    }

    public void AddToKillCount() {
        KillCount++;
        if (KillCount > newPossessionThreshold) {
            PlayerManager.Instance.GainNewPossession();
            newPossessionThreshold *= 2;
        }
        UIManager.Instance.UpdateKills();
    }

    public int KillsToNextPossession() {
        return newPossessionThreshold - KillCount;
    }
}
