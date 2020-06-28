using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewRecordScene : MonoBehaviour
{
    public VariableLook player;
    public Text scoreText;
    private Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        SoundManager.PlaySound("Highscore");

        var score = PlayerPrefs.GetInt("highscore");
        scoreText.text = score.ToString();

        var animator = player.CurrentModel.GetComponentInChildren<Animator>();
        // dragon animation makes dragon run away from camera. 
        // should fix animation before uncommenting this line
        //animator.SetTrigger("StartRunning");
    }

    public void OnClick()
    {
        SceneManager.Instance.GoToNextScene();
    }

}
