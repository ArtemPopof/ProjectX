using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartButton : MonoBehaviour
{
    public GameObject heartButton;

        public void DisableHeartButton()
    {
        if (PlayerPrefs.GetInt("heart") == 0 || Heart.Instance.IsHeartUsedThreeTimes())
        {
            heartButton.SetActive(false);
        }
        else
        {
            Heart.Instance.UseHeart();
        }
    }
}
