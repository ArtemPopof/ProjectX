using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartButton : MonoBehaviour
{
    public void DisableHeartButton()
    {
        if (PlayerPrefs.GetInt("heart") == 0 || Heart.Instance.IsHeartUsedThreeTimes())
        {
            gameObject.SetActive(false);
        }
        else
        {
            Heart.Instance.UseHeart();
        }
    }
}
