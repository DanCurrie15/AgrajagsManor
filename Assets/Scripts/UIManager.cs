using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
    public TextMeshProUGUI health;
    public GameObject gameOverPanel;

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

    public void ReloadScene()
    {
        Debug.Log("Retry");
        SceneManager.LoadScene("Game");
    }
}
