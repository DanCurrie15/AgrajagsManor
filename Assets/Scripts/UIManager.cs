using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
    public TextMeshProUGUI health;
    public TextMeshProUGUI killsToNextPossession;
    public GameObject gameOverPanel;
    public GameObject startGamePanel;

    private int maxHealth;
    private int currentHealth;

    public void SetPlayerMaxHealth(int setMax)
    {
        currentHealth = maxHealth = setMax;
        RedrawHealth();
    }

    public void SetPlayerCurrentHealth(int setHealth)
    {
        currentHealth = setHealth;
        RedrawHealth();
    }

    private void RedrawHealth()
    {
        health.text = currentHealth.ToString() + " / " + maxHealth.ToString();
    }

    public void ShowGameOverPanel()
    {
        gameOverPanel.SetActive(true);
    }

    public void HideStartGamePanel()
    {
        startGamePanel.SetActive(false);
    }

    public void StartGame()
    {
        GameManager.Instance.StartGame();
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void UpdateKills() {
        killsToNextPossession.text = GameManager.Instance.KillsToNextPossession().ToString();
    }
}
