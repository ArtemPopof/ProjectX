using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static AudioClip playerDeath, playerJump, coin, swipe1, swipe2, chest, highscore, special;
    static AudioSource audioSource;
    private static int coinsCountInARow;
    private static float lastCoinTime;
    private static float maxCoinStreakDelay = 0.8f;
    private const float HALF_STEP_PITCH_INCREASE = 1.0f / 12.0f;
    private static int[] HALF_STEP_PATTERN = { 3, 2, 2 };

    void Start()
    {
        playerDeath = Resources.Load<AudioClip>("broken");
        playerJump = Resources.Load<AudioClip>("jump");
        coin = Resources.Load<AudioClip>("coin");
        swipe1 = Resources.Load<AudioClip>("swipe1");
        swipe2 = Resources.Load<AudioClip>("swipe2");
        chest = Resources.Load<AudioClip>("chest");
        highscore = Resources.Load<AudioClip>("highscore");
        special = Resources.Load<AudioClip>("special");

        audioSource = GetComponent<AudioSource>();
        lastCoinTime = 0;
        coinsCountInARow = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void PlaySound(string clip)
    {
        // resetPitch();

        switch (clip) {
            case "Death":
                if (GameManager.Instance.IsDead) return;
                audioSource.PlayOneShot(playerDeath);
                break;
            case "Jump":
                audioSource.PlayOneShot(swipe1);
                break;
            case "Coin":
                increaseAudioPitch();
                audioSource.PlayOneShot(coin);
            break;
                case "Swipe1":
                audioSource.PlayOneShot(swipe1);
                break;
            case "Swipe2":
                audioSource.PlayOneShot(swipe2);
                break;
            // case "Chest":
            //     audioSource.PlayOneShot(chest);
            //     break;
            case "Highscore":
                audioSource.PlayOneShot(highscore);
                break;
            case "Special":
                audioSource.PlayOneShot(special);
                break;
            default:
                break;
        }
    }

    private static void resetPitch()
    {
        audioSource.pitch = 1;
    }

    private static void increaseAudioPitch()
    {
        if (coinStreakIsOver())
        {
            coinsCountInARow = 0;
        }

        lastCoinTime = Time.time;

        var pitch = calculateCurrentPitch();
        audioSource.pitch = pitch;
    }

    private static float calculateCurrentPitch()
    {
        int currentPatternIndex = coinsCountInARow++ / 4 % HALF_STEP_PATTERN.Length;
        int totalHalfstepsFromRoot = 0;
        for (int i = 0; i < currentPatternIndex; i++)
        {
            totalHalfstepsFromRoot += HALF_STEP_PATTERN[i];
        }
        return 1.0f + totalHalfstepsFromRoot * HALF_STEP_PITCH_INCREASE;
    }

    private static bool coinStreakIsOver()
    {
        return Time.time - lastCoinTime > maxCoinStreakDelay;
    }
}
