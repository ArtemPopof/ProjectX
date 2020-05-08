using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Letter : MonoBehaviour
{
    public Animator animator;

    public static Letter Instance { private set; get; }

    void Awake()
    {
        Instance = this;
    }
    private void Start()
    {

        animator = GetComponent<Animator>();
    }

    public bool IsCollectedAllLetters()
    {
            return PlayerPrefs.GetString("D").Length >= 1 &&
                   PlayerPrefs.GetString("R").Length >= 1 &&
                   PlayerPrefs.GetString("A").Length >= 1 &&
                   PlayerPrefs.GetString("G").Length >= 1 &&
                   PlayerPrefs.GetString("O").Length >= 1 &&
                   PlayerPrefs.GetString("N").Length >= 1;
    }

    public void OnTriggerEnter(Collider other)
    {
        SoundManager.PlaySound("Coin");
        animator.SetTrigger("Collision");
        string letter = gameObject.name;
        PlayerPrefs.SetString(letter, letter);
        WordUi.Instance.TriggerWordUi(letter);
        Destroy(this.gameObject, 1.5f);
    } 
}

