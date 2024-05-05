using MEC;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoSingleton<GameController>, IPowerUpEvent, IPlayerEvent, IDataPeristence
{
    // Dichiarazione degli eventi per la vita, il punteggio e le munizioni
    public event Action<int> LifeUpdated;
    public event Action<int> ScoreUpdated;
    public event Action<int> AmmoUpdated;

    // Riferimenti agli oggetti UI
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text dooublonsText;
    [SerializeField] private TMP_Text scoreOver;
    [SerializeField] private Image[] lifeImages;
    [SerializeField] private Image[] ammoImages;
    [SerializeField] private Image[] greenTreasureImages;
    [SerializeField] private Image redTreasureImage;
    [SerializeField] private TMP_Text[] scoreTextGameOver;
    [SerializeField] private GameObject GameOverPanel;
    [SerializeField] private GameObject PausePanel;
    [SerializeField] private GameObject TreasurePanel;
    [SerializeField] private GameObject BonusPanel;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private GameObject keyText;
    [SerializeField] private GameObject pool;
    public GameObject defend;
    public GameObject WaveStart;
    private int currentScore = 0;
    [SerializeField] private int defaultScoreIncrement = 1;
    private int scoreIncrement;
    [SerializeField] private int defaultStartLives = 3;
    private int currentLives;
    private bool isPaused = false;
    [SerializeField] private WavesController waves;
    private int currentDoubloonAmount;
    private int[] score;

    private void Start()
    {
        DataPersistenceManager.instance.NowLoad();
        currentLives = defaultStartLives;
        scoreIncrement = defaultScoreIncrement;
       
        // iscriviti a eventlistener per ricevere gli eventi
        EventListener.Instance.AddListener(this.gameObject);

        // Start checking game over conditions
        Timing.RunCoroutine(CheckGameOverCondition().CancelWith(gameObject));

        Timing.RunCoroutine(StartGame().CancelWith(gameObject));

        // Inizializza UI
        UpdateScoreUI();
        UpdateLifeUI();
        AudioManager.Instance.StopSpecificMusic(0);    
    }

    protected IEnumerator<float> StartGame()
    {
        yield return Timing.WaitForSeconds(4f);
        keyText.SetActive(true);
        while (!(Input.anyKeyDown))
        {
            yield return Timing.WaitForOneFrame;
        }
        loadingPanel.SetActive(false);
        player.SetActive(true);
        pool.SetActive(true);
        defend.SetActive(true);
        AudioManager.Instance.PlaySpecificMusic(2);
        Timing.KillCoroutines("StartGame");
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

    public void GameOver()
    {
        UpdateDifferentScore();
        dooublonsText.text = currentDoubloonAmount.ToString();
        AudioManager.Instance.StopSpecificMusic(2);
        AudioManager.Instance.PlaySpecificOneShot(9);
        GameOverPanel.SetActive(true);
        PoolController.Instance.Clear();
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
        Timing.RunCoroutine(ammoActive().CancelWith(gameObject)); ;
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
        pauseButton.SetActive(true);    
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
    private IEnumerator<float> CheckGameOverCondition()
    {
        while (currentLives > 0 && Island.Instance.CurrentTreasure > 0)
        {
            yield return Timing.WaitForOneFrame;
        }

        GameOver();
        Timing.KillCoroutines("CheckGameOverCondition");
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
                BonusPanel.transform.Find("Life").gameObject.SetActive(true);
                break;
            case EPowerUpType.DoubloonUp:
                 currentDoubloonAmount+=(int)data.Value;
            
                BonusPanel.transform.Find("Doubloon").gameObject.SetActive(true);
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

    public void UpdateTreasureUI(float currentTreasure){
        if(currentTreasure%1==0){
            redTreasureImage.gameObject.SetActive(false);
            for (int i = 0; i < greenTreasureImages.Length; i++)
            {
                greenTreasureImages[i].gameObject.SetActive(i < currentTreasure);
            }
        } else {
            for (int i = 0; i < greenTreasureImages.Length; i++)
            {
                greenTreasureImages[i].gameObject.SetActive(i < (currentTreasure-1));
            }
            redTreasureImage.gameObject.SetActive(true);
        }
    }

    public void LoadData(GameData data)
    {
        currentDoubloonAmount=data.doubloons;
        score=data.score;
    }

    public void SaveData(ref GameData data)
    {
        data.doubloons = currentDoubloonAmount;
        data.score = score;
    }

    private void UpdateDifferentScore()
    {
        Array.Sort(score);//ordina l'array
        if (score[0] < currentScore)
        {
            score[0] = currentScore;
        }
        Array.Sort(score);//riordina l'array
        scoreTextGameOver[2].text = score[2].ToString();
        scoreTextGameOver[1].text = score[1].ToString();
        scoreTextGameOver[0].text = score[0].ToString();
        DataPersistenceManager.instance.NowSave();
    }

    
}