﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { private set; get; }
    public bool IsRunning { set; get; }
    public PropertyList Properties {get; private set;}
    public PlayerMotor playerMotor;

    private Timer secTimer;

    private int scoreIncrease = 1;

    void Awake()
    {
        Instance = this;
        IsRunning = false;
        Properties = new PropertyList();
        secTimer = InitTimer();

        Properties.Add("distance", 0.0f);
        Properties.Add("multiplier", 0.0f).WithCustomFormater(new MultiplierFormater());
        Properties.Add("score", 0);
        
    }

    void Update()
    {
        if (!IsRunning && MobileInput.Instance.Tap) {
            IsRunning = true;
            playerMotor.StartRunning();
            secTimer.Start();
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
        var score = Properties.GetInt("score") + scoreIncrease * Properties.GetFloat("multiplier");
        score = (int) Math.Round(score);
        Properties.setProperty("score", score);
    }
}
