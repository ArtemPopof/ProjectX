using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathUi : MonoBehaviour
{
    public Text score;
    public Text coins;

    public Text hearts;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        //GameManager.Instance.Properties.bind(score, "score");
        //GameManager.Instance.Properties.bind(coins, "coins");
        hearts.text = PlayerPrefs.GetInt("heart").ToString();
    }

    private void Awake()
    {
        animator.SetTrigger("Appear");
    }

    // Update is called once per frame
    void Update()
    {
        score.text = GameManager.Instance.Properties.GetInt("score").ToString();
        coins.text = GameManager.Instance.Properties.GetInt("coins").ToString();
    }
}
