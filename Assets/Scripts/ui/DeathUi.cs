using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathUi : MonoBehaviour
{
    public Text score;
    public Text coins;

    public Text hards;

    public GameObject hardButton;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.Properties.bind(score, "score");
        GameManager.Instance.Properties.bind(coins, "coins");
        hards = PlayerPrefs.GetInt("hard");
    }

    private void Awake()
    {
        animator.SetTrigger("Appear");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisableHardButton()
    {
        if (Hard.Instance.IsHardUsedThreeTimes())
        {
            hardButton.SetActive(false);
        }
    }
}
