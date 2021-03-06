﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Dont forget to set transform to maxWidth
 * (maxWidth = init transform width)
 */
public class ProgressBar : MonoBehaviour
{
    private float maxWidth;
    private const float loadingTime = 2.0f;
    private float startTime;
    // Start is called before the first frame update
    void Start()
    {
        maxWidth = CalcMaxWidth();

        SetProgress(0);

        startTime = Time.time;
    }

    void Update()
    {
        var percents = (Time.time - startTime) / loadingTime;
        percents = Mathf.Clamp(percents, 0, 1);
        percents *= 100;

        SetProgress((int) percents);
    }

    private float CalcMaxWidth()
    {
        var parentRect = transform.GetComponent<RectTransform>();
        return parentRect.rect.width;
    }

    public void SetProgress(int percent)
    {
        var targetWidth = (int) (maxWidth / 100.0f * percent);
        transform.GetComponent<RectTransform>().sizeDelta = new Vector2(targetWidth, 0);

        if (percent != 100)
        {
            return;
        }

        if (GameManager.Instance != null)
        {
            // Extract to callback
            GameManager.Instance.OnLoadingEnd();
            startTime = Time.time;
        }
    }
}
