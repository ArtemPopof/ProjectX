using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundAudio : MonoBehaviour
{
    public AudioSource firstLevel;
    public AudioSource seceondLevel;
    private int currentLevel;

    void Start()
    {
        currentLevel = PlayerPrefs.GetInt("currentLevel") - 100;
        if (currentLevel == 0)
        {
            firstLevel.Play();
        }
        else 
        {
            seceondLevel.Play();
        }
    }
}
