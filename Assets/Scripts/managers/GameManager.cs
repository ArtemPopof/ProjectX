using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.Advertisements;

public class GameManager : MonoBehaviour
{
    private const bool DEBUG_MODE = true;
    private const string GAME_ID = "3565048";
    private const int SCORE_INCREMENT = 10;
    // this constant restrict loading, if scene was recently loaded
    // after 2 secs we need to show loading screen again
    // this will guarantee loading screen is active after 
    // game application is restarted
    private const float GAME_RESTART_DELAY_BEFORE_RELOADING = 2;

    public static GameManager Instance { private set; get; }

    public bool IsDead {set; get;}
    public bool IsRunning { set; get; }

    public bool IsLoading { set; get; }

    public PropertyList Properties {get; private set;}

    public VariableLook player;
    public CameraMotor cameraMotor;
    public LevelManager levelManager;
    private AdManager adManager;

    public Animator menu;

    private Timer secTimer;

    private int scoreIncrease = 1;
    private bool scoreIncreaseTick = false;

    private GameObject uiPanels;

    private GameObject deathCauser;

    void Awake()
    {
        Instance = this;
        IsRunning = false;
        IsLoading = true;
        IsDead = false; 
        Properties = new PropertyList();
        secTimer = InitTimer();
        adManager = new AdManager();

        Properties.Add("distance", 0.0f);
        Properties.Add("multiplier", 0.0f).WithCustomFormater(new MultiplierFormater());
        Properties.Add("score", 0);
        Properties.Add("coins", 0);
        Properties.Add("chests", 0);
        Properties.Add("eggs", 0);
        Properties.Add("debug", 0);
        Properties.Add("hard", 0);
        Properties.Add("restartCount", 0);
        PlayerPrefs.SetInt("currentScene", 0);
        PlayerPrefs.SetInt("chests", 0);
        

        var highscore = 0;
        if (PlayerPrefs.HasKey("highscore"))
        {
            highscore = PlayerPrefs.GetInt("highscore");
        }
        Properties.Add("highscore", highscore);

        uiPanels = GameObject.FindWithTag("UI");

        if (GameRestarted())
        {
            OnLoadingEnd();
        } else
        {
            initAdsEngine();
        }

        // Add default look into collection
        if (PlayerPrefs.GetString("availableLooks") == "")
        {
            PlayerPrefs.SetString("availableLooks", "0");
        }
    }

    private void Start()
    {
        // init player
        cameraMotor.lookAt = player.CurrentModel.transform;
    }

    private void initAdsEngine()
    {
        Advertisement.Initialize(GAME_ID, DEBUG_MODE);
    }

    private bool GameRestarted()
    {
        if (!PlayerPrefs.HasKey("LastRestart"))
        {
            return false;
        }

        float delta = Time.time - PlayerPrefs.GetFloat("LastRestart");
        if (delta < 0) return false;

        return delta < GAME_RESTART_DELAY_BEFORE_RELOADING;
    }

    void Update()
    {
        if (IsDead)
        {
            return;
        }

        //TODO extract all string constants
        //TODO maybe make some statemachine for states
        if (!IsRunning && !IsLoading && MobileInput.Instance.Tap) {
            if (MobileInput.Instance.TapShop)
            {
                OpenShop();
            } else
            {
                SetUIPanelActive("InGameUi", true);
                IsRunning = true;
                player.CurrentModel.StartRunning();
                secTimer.Start();
                cameraMotor.ZoomPlayer();
                cameraMotor.IsMoving = true;
                menu.SetTrigger("Hide");
            }
        }
        

        if (scoreIncreaseTick && !PauseButton.GameIsPaused)
        {

            var score = Properties.GetInt("score") + scoreIncrease * Properties.GetFloat("multiplier");
            int roundedScore = (int)Math.Floor(score + 1);
            Properties.setProperty("score", roundedScore);
            Properties.setProperty("highscore", Mathf.Max(roundedScore, Properties.GetInt("highscore")));
            scoreIncreaseTick = false;
        }
    }

    public void OnLoadingEnd()
    {
        SetUIPanelActive("LoadingUi", false);
        SetUIPanelActive("MainMenuUi", true);
        IsLoading = false;
    }

    public void SetUIPanelActive(string panelTag, bool isActive)
    {
        foreach(Transform child in uiPanels.transform)
        {
            if (child.CompareTag(panelTag))
            {
                child.gameObject.SetActive(isActive);
                return;
            }
        }
    }

    private Timer InitTimer()
    {
        var timer = new Timer();
        timer.Elapsed += new ElapsedEventHandler(OnTimer);
        timer.Interval = 200;

        return timer;
    }

    private void OnTimer(object source, ElapsedEventArgs e) {
        scoreIncreaseTick = true;
    }

    public void AddCoin()
    {
        AddCoins(1);
    }

    public void AddCoins(int count)
    {
        //var score = Properties.GetInt("score");
        //var multiplier = Properties.GetFloat("multiplier");
        //var newScore = Mathf.RoundToInt(score + multiplier * SCORE_INCREMENT);
        Properties.AddToIntProperty("coins", count);
    }

    public void AddChest()
    {
        Properties.AddToIntProperty("chests", 1);
        SoundManager.PlaySound("Special");
    }

    public void AddEgg()
    {
        Properties.AddToIntProperty("eggs", 1);
    }

    public void AddHard()
    {
        if (Properties.GetInt("eggs") == 10)
        {
            int hardCount = PlayerPrefs.GetInt("hard") + 1;
            PlayerPrefs.SetInt("hard", hardCount);
        }
    }

    public void OnPlayerDeath(GameObject collider)
    {
        IsRunning = false;
        IsDead = true;
        deathCauser = collider;
        SetUIPanelActive("InGameUi", false);
        SetUIPanelActive("GameOverUi", true);
    }

    public void Resurrect()
    {
        SetUIPanelActive("GameOverUi", false);
        SetUIPanelActive("InGameUi", true);
        EvaporateGameObjectsOfCurrentAndNextSegment();
        IsRunning = true;
        IsDead = false;
        player.CurrentModel.ResurrectPlayer();
    }

    public void RestartGame()
    {
        var coins = PlayerPrefs.GetInt("coins") + Properties.GetInt("coins");
        PlayerPrefs.SetInt("coins", coins);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Sprint3");
        SetUIPanelActive("GameOverUi", false);
        menu.SetTrigger("Show");
        PlayerPrefs.SetFloat("LastRestart", Time.time);
        Letter.Instance.DeleteLettersInPlayerPrefs();
    }

    private void EvaporateGameObjectsOfCurrentAndNextSegment()
    {
        var currentSegment = levelManager.GetSegementByGameObject(deathCauser);
        var nextSegment = levelManager.GetNextSegment(currentSegment);

        // need animation here
        currentSegment.SegmentObjects.SetActive(false);
        nextSegment.SegmentObjects.SetActive(false);
    }

    public void OpenShop()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Shop");
    }
}