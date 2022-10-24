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
            return PlayerPrefs.GetString("D").Equals("D") &&
                   PlayerPrefs.GetString("R").Equals("R") &&
                   PlayerPrefs.GetString("A").Equals("A") &&
                   PlayerPrefs.GetString("G").Equals("G") &&
                   PlayerPrefs.GetString("O").Equals("O") &&
                   PlayerPrefs.GetString("N").Equals("N");
    }

    public void DeleteLettersInPlayerPrefs()  
    {
        PlayerPrefs.SetString("D", null);
        PlayerPrefs.SetString("R", null);
        PlayerPrefs.SetString("A", null);
        PlayerPrefs.SetString("G", null);
        PlayerPrefs.SetString("O", null);
        PlayerPrefs.SetString("N", null);
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

