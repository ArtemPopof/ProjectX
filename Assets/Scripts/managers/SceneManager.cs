using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance { get; set; }
    private static List<Scene> scenes = new List<Scene>(5);
    private int currentScene = 0;

    static SceneManager()
    {
        Instance = new SceneManager();
        scenes.Add(Scene.GAMEPLAY);
        scenes.Add(Scene.HIGHSCORE);
        scenes.Add(Scene.WORD_QUEST_PRIZE);
        scenes.Add(Scene.EGG_QUEST_PRIZE);
        scenes.Add(Scene.PRIZE_GIVAWAY);
    }

    void Awake()
    {
        currentScene = PlayerPrefs.GetInt("currentScene");
    }

    public void GoToNextScene()
    {
        currentScene = PlayerPrefs.GetInt("currentScene");

        currentScene++;
        if (currentScene >= scenes.Count)
        {
            currentScene = 0;
        }

        PlayerPrefs.SetInt("currentScene", currentScene);

        switch (scenes[currentScene])
        {
            case Scene.GAMEPLAY:
                GameManager.Instance.RestartGame();
                break;
            case Scene.HIGHSCORE:
                CheckHighscore();
                break;
            case Scene.PRIZE_GIVAWAY:
                CheckForChests();
                break;
            case Scene.EGG_QUEST_PRIZE:
                CheckForEggQuest();
                break;
            case Scene.WORD_QUEST_PRIZE:
                CheckForWordQuest();
                break;
        }
    }


    public void CheckHighscore()
    {
         var Properties = GameManager.Instance.Properties;

        PlayerPrefs.SetInt("chests", Properties.GetInt("chests"));
        PlayerPrefs.SetInt("eggs", Properties.GetInt("eggs"));

        var lastHighscore = PlayerPrefs.GetInt("highscore");
        if (Properties.GetInt("score") > lastHighscore)
        {
            PlayerPrefs.SetInt("highscore", Properties.GetInt("score"));
            UnityEngine.SceneManagement.SceneManager.LoadScene("NewHighscore");
        } else
        {
            GoToNextScene();
        }
    }

    public void CheckForChests()
    {
        if (GameManager.Instance.Properties.GetInt("chests") > 0)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("PrizeGivaway");
        } else
        {
            GoToNextScene();
        }
    }

    public void CheckForWordQuest()
    {
        if (Letter.Instance.IsCollectedAllLetters() && PlayerPrefs.GetInt("CollectedLettersQuestPrize") != 1)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("WordPrize");
            PlayerPrefs.SetInt("CollectedLettersQuestPrize", 1);
        }
        else
        {
            GoToNextScene();
        }
    }

    public void CheckForEggQuest()
    {
        if (GameManager.Instance.Properties.GetInt("eggs") >= 10)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("EggsPrize");
        } else
        {
            GoToNextScene();
        }
    }

    public enum Scene
    {
        PRIZE_GIVAWAY,
        HIGHSCORE,
        EGG_QUEST_PRIZE,
        WORD_QUEST_PRIZE,
        GAMEPLAY
    }
}
