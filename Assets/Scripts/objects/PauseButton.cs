using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour
{

    public static bool GameIsPaused = false;

    // Update is called once per frame
    public void ButtonBehavior()
    {
        if (GameIsPaused)
        {
            Resume();
        }
        else 
        {
            Pause();
        }
        
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
        GameManager.Instance.SetUIPanelActive("PauseUi", false);
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        GameIsPaused = true;
        GameManager.Instance.SetUIPanelActive("PauseUi", true);
    }
}
