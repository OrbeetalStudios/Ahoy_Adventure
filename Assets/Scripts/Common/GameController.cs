using MEC;
using System;
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
    [SerializeField] private Image[] lifeImages;
    [SerializeField] private Image[] ammoImages;
    [SerializeField] private GameObject GameOverPanel;


    private int currentScore = 0;
    private int lifeCount = 3;

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
        Debug.Log("Ho tolto una vita");
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

    public void Restart()
    {
        GameOverPanel.SetActive(false);
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    public void ExitGame()
    {
        Application.Quit();
    }
}