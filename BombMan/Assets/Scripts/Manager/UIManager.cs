using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject Healthbar;

    [Header("UI Elements")]
    public GameObject pauseMenu;
    public Slider bossHealthBar;
    public GameObject gameOver;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);


    }

    public void UpdateHealth(float currentHealth)
    {
        switch (currentHealth)
        {
            case 3:
                Healthbar.transform.GetChild(0).gameObject.SetActive(true);
                Healthbar.transform.GetChild(1).gameObject.SetActive(true);
                Healthbar.transform.GetChild(2).gameObject.SetActive(true);
                break;
            case 2:
                Healthbar.transform.GetChild(0).gameObject.SetActive(true);
                Healthbar.transform.GetChild(1).gameObject.SetActive(true);
                Healthbar.transform.GetChild(2).gameObject.SetActive(false);
                break;
            case 1:
                Healthbar.transform.GetChild(0).gameObject.SetActive(true);
                Healthbar.transform.GetChild(1).gameObject.SetActive(false);
                Healthbar.transform.GetChild(2).gameObject.SetActive(false);
                break;
            case 0:
                Healthbar.transform.GetChild(0).gameObject.SetActive(false);
                Healthbar.transform.GetChild(1).gameObject.SetActive(false);
                Healthbar.transform.GetChild(2).gameObject.SetActive(false);
                break;
        }
    }

    public void PauseGame()
    {
        if (pauseMenu.active)
            ResumeGame();
        else
        {

        pauseMenu.SetActive(true);

        Time.timeScale = 0;
        }
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);

        Time.timeScale = 1;
    }

    public void SetBossHealth(float Health)
    {
        bossHealthBar.maxValue = Health;
    }

    public void UpdateBossHealth(float Health)
    {
        bossHealthBar.value = Health;
    }

    public void GameOverUI(bool playerDead)
    {
        gameOver.SetActive(playerDead);
    }
}
