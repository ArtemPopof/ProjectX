using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewRecordScene : MonoBehaviour
{
    public Animator playerAnimator;
    public Text scoreText;

    // Start is called before the first frame update
    void Start()
    {
        SoundManager.PlaySound("Highscore");

        var score = PlayerPrefs.GetInt("highscore");
        scoreText.text = score.ToString();
        playerAnimator.SetTrigger("StartRunning");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        if (PlayerPrefs.GetInt("chests") <= 0)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Sprint0");
        } 
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("PrizeGivaway");
        }
    }

}
