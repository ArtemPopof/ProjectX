using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private const int SCORE_INCREMENT = 10;

    public static GameManager Instance { private set; get; }

    public bool IsDead {set; get;}
    public bool IsRunning { set; get; }
    public PropertyList Properties {get; private set;}

    public PlayerMotor playerMotor;
    public CameraMotor cameraMotor;

    public Animator menu;

    private Timer secTimer;

    private int scoreIncrease = 1;
    private bool scoreIncreaseTick = false;

    private GameObject uiPanels;

    void Awake()
    {
        Instance = this;
        IsRunning = false;
        IsDead = false;
        Properties = new PropertyList();
        secTimer = InitTimer();

        Properties.Add("distance", 0.0f);
        Properties.Add("multiplier", 0.0f).WithCustomFormater(new MultiplierFormater());
        Properties.Add("score", 0);
        Properties.Add("coins", 0);
        Properties.Add("chests", 0);

        uiPanels = GameObject.FindWithTag("UI");
    }

    void Update()
    {
        if (IsDead)
        {
            return;
        }

        //TODO extract all string constants
        //TODO maybe make some statemachine for states
        if (!IsRunning && MobileInput.Instance.Tap) {
            SetUIPanelActive("InGameUi", true);
            IsRunning = true;
            playerMotor.StartRunning();
            secTimer.Start();
            cameraMotor.ZoomPlayer();
            cameraMotor.IsMoving = true;
            menu.SetTrigger("Hide");
        }
        

        if (scoreIncreaseTick)
        {
            var score = Properties.GetInt("score") + scoreIncrease * Properties.GetFloat("multiplier");
            int roundedScore = (int)Math.Floor(score + 1);
            Properties.setProperty("score", roundedScore);
            scoreIncreaseTick = false;
        }
    }

    private void SetUIPanelActive(string panelTag, bool isActive)
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
    }

    public void OnPlayerDeath()
    {
        IsRunning = false;
        IsDead = true;
        SetUIPanelActive("InGameUi", false);
        SetUIPanelActive("GameOverUi", true);
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        SetUIPanelActive("GameOverUi", false);
    }

    public void CheckForNewHighscore()
    {
        var lastHighscore = PlayerPrefs.GetInt("highscore");
        if (Properties.GetInt("score") > lastHighscore)
        {
            PlayerPrefs.SetInt("highscore", Properties.GetInt("score"));
            PlayerPrefs.SetInt("chests", Properties.GetInt("chests"));
            UnityEngine.SceneManagement.SceneManager.LoadScene("NewHighscore");
            return;
        }

        CheckForPrizesCollected();
    }

    public void CheckForPrizesCollected()
    {
        if (Properties.GetInt("chests") <= 0)
        {
            RestartGame();
            return;
        }

        // TODO extract constant
        UnityEngine.SceneManagement.SceneManager.LoadScene("PrizeGivaway");
    }
}