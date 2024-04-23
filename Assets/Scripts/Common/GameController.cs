using MEC;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoSingleton<GameController>, IPowerUpEvent
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
    [SerializeField] private int defaultScoreIncrement = 1;
    private int scoreIncrement;
    private int lifeCount = 3;
    private bool isPaused = false;
    private bool invulnerabilityOn = false;
    [SerializeField] private WavesController waves;

    private void Start()
    {
        scoreIncrement = defaultScoreIncrement;

        // iscriviti a eventlistener per ricevere gli eventi
        EventListener.Instance.AddListener(this.gameObject);

        // Inizializza UI
        UpdateScoreUI();
        UpdateLifeUI();
        AudioManager.Instance.StopSpecificMusic(0);
        AudioManager.Instance.PlaySpecificMusic(2);     
    }

    public void UpdateScore()
    {
        currentScore += scoreIncrement;
        ScoreUpdated?.Invoke(currentScore);
        UpdateScoreUI();
    }

    public void UpdateLife()
    {
        if (invulnerabilityOn) return;

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
        AudioManager.Instance.PlaySpecificOneShot(9);
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

        waves.StartGame();
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
        AudioManager.Instance.StopSpecificMusic(2);
        SceneManager.LoadScene(1);
    }
    public void ExitGame()
    {
        Application.Quit();
    }

    public void OnClickSound()
    {
        AudioManager.Instance.PlaySpecificOneShot(4);
    }

    public void OnOffSound()
    {
        AudioManager.Instance.PlaySpecificOneShot(14);   
    }
   
    public void OnPowerUpCollected(PowerUpData data)
    {
        switch (data.Type)
        {
            case EPowerUpType.Invulnerability:
                invulnerabilityOn = true;
                break;
            case EPowerUpType.KillGold:
                scoreIncrement = (int)data.Value;
                break;
            case EPowerUpType.LifeUp:
                // TODO
                break;
            case EPowerUpType.CrewGold:
                // TODO
                break;
            default:
                break;
        }
    }
    public void OnPowerUpExpired(PowerUpData data)
    {
        switch (data.Type)
        {
            case EPowerUpType.Invulnerability:
                invulnerabilityOn = false;
                break;
            case EPowerUpType.KillGold:
                scoreIncrement = defaultScoreIncrement;
                break;
            case EPowerUpType.LifeUp:
                // TODO
                break;
            case EPowerUpType.CrewGold:
                // TODO
                break;
            default:
                break;
        }
    }
}