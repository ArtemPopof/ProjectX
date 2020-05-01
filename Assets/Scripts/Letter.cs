using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Letter : MonoBehaviour
{
    public Animator animator;

    public GameObject d, r, a, g, o, n;

    public HashSet<string> dragonLetters = new HashSet<string>();

    public static Letter Instance { private set; get; }

    private void Start()
    {
    
        animator = GetComponent<Animator>();
    }

    public void addLetter(string letter) 
    {
        dragonLetters.Add(letter);
    }

    public bool isCollectedWordComplited() 
    {
        byte countOfDragonLetters = 6;
        if (dragonLetters.Count == countOfDragonLetters) {
            PlayerPrefs.SetInt("isWordCollected", 1);
            return true;
        } 
        PlayerPrefs.SetInt("isWordCollected", 0);
        return false;
    }

    public void OnTriggerEnter(Collider other)
    {

        SoundManager.PlaySound("Coin");
        animator.SetTrigger("Collision");
        string letter = gameObject.name;
        PlayerPrefs.SetString(letter, letter);
        // if(isActiveLetter(gameObject))
        WordUi.Instance.TriggerWordUi(letter);
        addLetter(letter);
        Destroy(this.gameObject, 1.5f);
    } 

//     private bool isActiveLetter(GameObject gameObject)
//     {
//        int i = 0;
//        foreach(string letter in dragonLetters) {
//            if(gameObject.name.Equals(letter.ToUpper()))
//             i++;
//         }
//     if(i == 1) 
//     {
//     return false; 
//     }
//     return true;
// }
}

