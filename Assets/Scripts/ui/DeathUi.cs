using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathUi : MonoBehaviour
{
    public Text score;
    public Text coins;

    public Text hearts;

    public GameObject heart;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("heart") == 0 || Heart.Instance.IsHeartUsedThreeTimes())
        {
            heart.SetActive(false);
        }
        GameManager.Instance.Properties.bind(score, "score");
        GameManager.Instance.Properties.bind(coins, "coins");
        hearts.text = PlayerPrefs.GetInt("heart").ToString();
    }

    private void Awake()
    {
        animator.SetTrigger("Appear");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
