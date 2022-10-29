﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUi : MonoBehaviour
{
    public Text score;
    public Text coins;
    public Text multiplier;
    public Text highscore;
    public Text debug;

    public Text eggs;

    void Start()
    {
        if (multiplier == null || multiplier.text == null) return;
        GameManager.Instance.Properties.bind(score, "score");
        GameManager.Instance.Properties.bind(coins, "coins");
        GameManager.Instance.Properties.bind("multiplier", (value) => {
            if (multiplier == null || multiplier.text == null) return;
            multiplier.text = "x" + value.ToString();
            });
        GameManager.Instance.Properties.bind(highscore, "highscore");
        GameManager.Instance.Properties.bind(eggs, "eggs");
        GameManager.Instance.Properties.bind(debug, "debug");
    }
    

    void Update()
    {
    }
}