using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuUI : MonoBehaviour
{
    public Text highscore;
    public Text coins;

    public Text hearts;

    // Start is called before the first frame update
    void Start()
    {
        highscore.text = PlayerPrefs.GetInt("highscore").ToString();
        coins.text = PlayerPrefs.GetInt("coins").ToString();
        hearts.text = PlayerPrefs.GetInt("heart").ToString();
    }

}
