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
        SceneManager.Instance.GoToNextScene();
    }

}
