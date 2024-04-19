using MEC;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoSingleton<GameController>
{
    // Dichiarazione degli eventi per la vita, il punteggio e le munizioni
    public event Action<int> LifeUpdated;
    public event Action<int> ScoreUpdated;
    public event Action<int> AmmoUpdated;

    // Riferimenti agli oggetti UI
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text scoreOver;
    [SerializeField] private Image[] lifeImages;
    [SerializeField] private Image[] ammoImages;
    [SerializeField] private GameObject GameOverPanel;
    [SerializeField] private GameObject PausePanel;
    [SerializeField] private GameObject pauseButton;


    private int currentScore = 0;
    private int lifeCount = 3;
    private bool isPaused = false;

    private void Start()
    {
        // Inizializza UI
        UpdateScoreUI();
        UpdateLifeUI();
       
    }

    public void UpdateScore()
    {
        currentScore++;
        ScoreUpdated?.Invoke(currentScore);
        UpdateScoreUI();
    }

    public void UpdateLife()
    {
        lifeCount--;
        LifeUpdated?.Invoke(lifeCount);
        UpdateLifeUI();

        if (lifeCount <= 0)
            GameOver();
    }

    public void UpdateAmmo(int ammoCount)
    {
        AmmoUpdated?.Invoke(ammoCount);
        UpdateAmmoUI(ammoCount);
    }

    public void GameOver()
    {
        GameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }

    private void UpdateScoreUI()
    {
        scoreText.text = currentScore.ToString();
        scoreOver.text = currentScore.ToString();
    }

    private void UpdateLifeUI()
    {
        
        for (int i = 0; i < lifeImages.Length; i++)
        {
            lifeImages[i].gameObject.SetActive(i < lifeCount);
            if (lifeCount == 0)
            {
                GameOver();
            }
        }
    }

    private void UpdateAmmoUI(int ammoCount)
    {
        for (int i = 0; i < ammoImages.Length; i++)
        {
            SetImageTransparency(ammoImages[i], i >= ammoCount ? 0.5f : 1f);
        }
    }

    public void ImgAmmoDeactivated()
    {
        for (int i = 0; i < ammoImages.Length; i++)
        {
            SetImageTransparency(ammoImages[i],0f);
        }
    }

    public void ImgAmmoActivated()
    {
        Timing.RunCoroutine(ammoActive());
    }

    protected IEnumerator<float> ammoActive()
    {
        yield return Timing.WaitForSeconds(4f);
        for (int i = 0; i < ammoImages.Length; i++)
        {
            yield return Timing.WaitForSeconds(0.5f);
            SetImageTransparency(ammoImages[i], 100f);
            
        }
        Timing.KillCoroutines("ammoActive");
    }

    private void SetImageTransparency(Image image, float alpha)
    {
        if (image != null)
        {
            // Ottieni il colore corrente dell'immagine
            Color currentColor = image.color;

            // Imposta il valore alpha del colore
            currentColor.a = alpha;

            // Applica il nuovo colore all'immagine
            image.color = currentColor;
        }
    }



    public void Pause()
    {
        if (isPaused == true)
        {
            PausePanel.SetActive(false);
            pauseButton.SetActive(true);
            Time.timeScale = 1; // Riprendi il gioco
            isPaused =false;
        }
        else
        {
            isPaused = true;
            pauseButton.SetActive(false);
            Time.timeScale = 0;
            PausePanel.SetActive(true); 
        }
       
    }

    public void Restart()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        Time.timeScale = 1;
        SceneManager.LoadScene(currentSceneIndex);
    }


    public void MainMenu()
    {
        Time.timeScale = 1;
        PoolController.Instance.DeactivateAll();
        SceneManager.LoadScene(0);
    }


    public void ExitGame()
    {
        Application.Quit();
    }
}