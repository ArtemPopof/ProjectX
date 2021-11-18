﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.Advertisements;
using GoogleMobileAds.Api;

public class GameManager : MonoBehaviour
{
    private const bool DEBUG_MODE = false;
    private const string GAME_ID = "ca-app-pub-3940256099942544/5224354917";
    public const string ADMOB_GAME_ID = "ca-app-pub-4835916624633322~1666485099";
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

    private GameObject[] convases;

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
        Properties.Add("heart", 0);
        PlayerPrefs.SetInt("currentScene", 0); 
        PlayerPrefs.SetInt("chests", 0);
        
        if (PlayerPrefs.GetInt("heart") <= 0) 
        { 
            PlayerPrefs.SetInt("heart", 0);
        }

        var highscore = 0;
        if (PlayerPrefs.HasKey("highscore"))
        {
            highscore = PlayerPrefs.GetInt("highscore");
        }
        Properties.Add("highscore", highscore);

        convases = GameObject.FindGameObjectsWithTag("UI");

        if (GameRestarted())
        {
            OnLoadingEnd();
        } else
        {
            InitAdsEngine();
        }


        // Add default look and level into collection
        // 0 - default look, 100 - default lvl
        if (PlayerPrefs.GetString("availableItems") == "")
        {
            PlayerPrefs.SetString("availableItems", "0;100");
        }
        // set default level
        if (!PlayerPrefs.HasKey("currentLevel"))
        {
            PlayerPrefs.SetInt("currentLevel", 100);
        }

        // uncomment to reset all props
        //PlayerPrefs.DeleteAll();
        //PlayerPrefs.SetInt("heart", 550);
        //PlayerPrefs.SetInt("coins", 55550);
    }

    private void Start()
    {
        AppSpyClient.JornalAction("Start", "");
        // init player
        cameraMotor.lookAt = player.CurrentModel.transform;
    }

    public static void InitAdsEngine()
    {
        MobileAds.Initialize(initStatus => { Debug.Log("Init ad engine: " + initStatus); });
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
            if (MobileInput.Instance.TapButton)
            {
                //OpenShop();
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

        ShowFeedbackWindowIfWasnt();
    }

    private void ShowFeedbackWindowIfWasnt()
    {
        AppSpyClient.JornalAction("FeedbackShown", "");

        if (PlayerPrefs.GetInt("highscore") == 0)
        {
            return;
        }
        if (PlayerPrefs.GetInt("feedbackPerformed") != 0)
        {
            return;
        }
        var currentHours = (int)(DateTime.Now.Ticks / TimeSpan.TicksPerHour);
        if (currentHours - PlayerPrefs.GetInt("feedbackRequestLastTime") < 10)
        {
            return;
        }

        // need to show feedback window
        IsRunning = false;
        SetUIPanelActive("FeedbackUI", true);
        PlayerPrefs.SetInt("feedbackRequestLastTime", currentHours);
    }

    public void GoToPlayStore()
    {
        AppSpyClient.JornalAction("GoToStore", "");

        Application.OpenURL("market://details?id=com.AbbySoft.DragonRun");
        PlayerPrefs.SetInt("feedbackPerformed", 1);
        SetUIPanelActive("FeedbackUI", false);
        AddCoins(500);
        IsRunning = true;
    }

    public void SetUIPanelActive(string panelTag, bool isActive)
    {
        GameObject panel = null;
        for (int i = 0; i < this.convases.Length; i++)
        {
            panel = FindComponentByTag(convases[i].transform, panelTag);
            if (panel != null) break;
        }

        if (panel == null)
        {
            Debug.LogError("ERROR, NO PANEL FOUND: " + panelTag);
        }

        panel.SetActive(isActive);
    }

    public static GameObject FindComponentByTag(Transform parent, string tag)
    {
        foreach (Transform child in parent)
        {
            if (child.CompareTag(tag))
            {
                return child.gameObject;
            }
        }

        return null;
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

    public void AddHeart(int count)
    {
        if (Properties.GetInt("eggs") == 10)
        {
            int heartsCount = PlayerPrefs.GetInt("heart") + count;
            PlayerPrefs.SetInt("heart", heartsCount);
        }
    }

    public void OnPlayerDeath(GameObject collider)
    {
        AppSpyClient.JornalAction("Dead", "");

        IsRunning = false;
        IsDead = true;
        deathCauser = collider;
        SetUIPanelActive("InGameUi", false);
        SetUIPanelActive("DialogsUI", true);
    }

    public void Resurrect()
    {
        AppSpyClient.JornalAction("Ressurected", "");

        SetUIPanelActive("GameOverUi", false);
        SetUIPanelActive("InGameUi", true);
        EvaporateGameObjectsOfCurrentAndNextSegment();
        IsRunning = true;
        IsDead = false;
        player.CurrentModel.ResurrectPlayer();
    }

    public void RestartGame()
    {
        AppSpyClient.JornalAction("Restart", "");

        var time = Time.time;
        var coins = PlayerPrefs.GetInt("coins") + Properties.GetInt("coins");
        PlayerPrefs.SetInt("coins", coins);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Sprint3");
        SetUIPanelActive("GameOverUi", false);
        menu.SetTrigger("Show");
        PlayerPrefs.SetFloat("LastRestart", Time.time);
        Letter.Instance.DeleteLettersInPlayerPrefs();

        Debug.Log("Restarted in " + (Time.time - time) + " seconds");
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
        AppSpyClient.JornalAction("OpenShop", "");

        UnityEngine.SceneManagement.SceneManager.LoadScene("Shop");
    }
}