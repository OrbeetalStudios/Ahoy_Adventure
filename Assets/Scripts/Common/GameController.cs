using MEC;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController :MonoBehaviour
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

   
    private static GameController _instance;

    // Proprietà pubblica per accedere all'istanza del singleton
    public static GameController Instance
    {
        get
        {
            // Se l'istanza non esiste, cerchiamo di trovarla nella scena
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameController>();

                // Se non è stata trovata, la creiamo
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("GameController");
                    _instance = singletonObject.AddComponent<GameController>();
                }
            }

            // Restituisci l'istanza trovata o creata
            return _instance;
        }
    }
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

    

    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }


    public void ExitGame()
    {
        Application.Quit();
    }
}