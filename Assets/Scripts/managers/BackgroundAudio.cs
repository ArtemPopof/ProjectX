using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundAudio : MonoBehaviour
{
    public AudioSource firstLevel;
    public AudioSource seceondLevel;

    [System.Obsolete]
    void Start()
    {
        var currentLevel = PlayerPrefs.GetInt("currentLevel") - 100;
        if (currentLevel == 0)
        {
            firstLevel.Play();
        }
        else 
        {
            seceondLevel.Play();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        bool f = firstLevel.isPlaying;
        bool s = seceondLevel.isPlaying;
        var currentLevel = PlayerPrefs.GetInt("currentLevel") - 100;
        if (currentLevel == 0)
        {
            seceondLevel.Stop();
            firstLevel.Play();
        } 
        else
        { 
            firstLevel.Stop();
            seceondLevel.Play();
        }
    }
}
