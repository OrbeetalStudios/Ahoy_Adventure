using MEC;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoSingleton<GameController>, IPowerUpEvent, IPlayerEvent
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
    [SerializeField] private Image[] treasureImages;
    [SerializeField] private GameObject GameOverPanel;
    [SerializeField] private GameObject PausePanel;
    [SerializeField] private GameObject TreasurePanel;
    [SerializeField] private GameObject pauseButton;

    private int currentScore = 0;
    [SerializeField] private int defaultScoreIncrement = 1;
    private int scoreIncrement;
    [SerializeField] private int defaultStartLives = 3;
    private int currentLives;
    private bool isPaused = false;
    [SerializeField] private WavesController waves;
    private int currentDoubloonAmount;

    private void Start()
    {
        currentLives = defaultStartLives;
        scoreIncrement = defaultScoreIncrement;

        // Read saved amount of doubloons collected by user up to now
        currentDoubloonAmount = SavedDataManager.ReadInt(SavedDataManager.ESavedDataType.HighScore);

        // iscriviti a eventlistener per ricevere gli eventi
        EventListener.Instance.AddListener(this.gameObject);

        // Start checking game over conditions
        Timing.RunCoroutine(CheckGamOverCondition().CancelWith(gameObject));

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

    public void UpdateAmmo(int ammoCount)
    {
        AmmoUpdated?.Invoke(ammoCount);
        UpdateAmmoUI(ammoCount);
    }

    private void GameOver()
    {
        AudioManager.Instance.StopSpecificMusic(2);
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
            lifeImages[i].gameObject.SetActive(i < currentLives);
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
    private IEnumerator<float> CheckGamOverCondition()
    {
        while (currentLives > 0)// && Island.Instance.CurrentTreasure > 0)
        {
            yield return Timing.WaitForOneFrame;
        }

        GameOver();
    }
    public void OnPowerUpCollected(PowerUpData data)
    {
        switch (data.Type)
        {
            case EPowerUpType.KillGold:
                scoreIncrement = (int)data.Value;
                break;
            case EPowerUpType.LifeUp:
                if (currentLives < defaultStartLives)
                {
                    currentLives++;
                    UpdateLifeUI();
                }
                break;
            case EPowerUpType.DoubloonUp:
                // save the doubloons
                SavedDataManager.WriteInt(SavedDataManager.ESavedDataType.HighScore, ++currentDoubloonAmount);
                break;
            default:
                break;
        }
    }
    public void OnPowerUpExpired(PowerUpData data)
    {
        switch (data.Type)
        {
            case EPowerUpType.KillGold:
                scoreIncrement = defaultScoreIncrement;
                break;
            default:
                break;
        }
    }
    public void OnPlayerHit()
    {
        currentLives--;
        UpdateLifeUI();
    }

    public void UpdateTreasureUI(int currentTreasure){
        for (int i = 0; i < treasureImages.Length; i++)
        {
            treasureImages[i].gameObject.SetActive(i < currentTreasure);
        }
    }
}