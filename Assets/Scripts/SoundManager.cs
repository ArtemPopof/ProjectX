using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static AudioClip playerDeath, playerJump, coin, swipe1, swipe2;
    static AudioSource audioSource;
    void Start()
    {
        playerDeath = Resources.Load<AudioClip>("broken");
        playerJump = Resources.Load<AudioClip>("jump");
        coin = Resources.Load<AudioClip>("coin");
        swipe1 = Resources.Load<AudioClip>("swipe1");
        swipe2 = Resources.Load<AudioClip>("swipe2");

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void PlaySound(string clip)
    {
        switch(clip) {
            case "Death":
            audioSource.PlayOneShot(playerDeath);
            break;
            case "Jump":
            audioSource.PlayOneShot(playerJump);
            break;
            case "Coin":
            audioSource.PlayOneShot(coin);
            break;
            case "Swipe1":
            audioSource.PlayOneShot(swipe1);
            break;
            case "Swipe2":
            audioSource.PlayOneShot(swipe2);
            break;
            
        }
    }
}
