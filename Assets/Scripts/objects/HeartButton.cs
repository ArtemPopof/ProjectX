using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartButton : MonoBehaviour
{
    public void Start()
    {
      if (PlayerPrefs.GetInt("heart") <=0 || IsHeartUsedThreeTimes())
        {
            gameObject.SetActive(false);
        }
    }

    public void UserHeart()
    {
        int heartsCount = PlayerPrefs.GetInt("heart");
        PlayerPrefs.SetInt("heart", heartsCount - 1);
        GameManager.Instance.Properties.AddToIntProperty("restartCount", 1);
        GameManager.Instance.Resurrect();
    }

    public bool IsHeartUsedThreeTimes()
    {
        bool flag = false;
        if (GameManager.Instance.Properties.GetInt("restartCount") == 3)
        {
            flag = true;
        }
        return flag;
    }
}
