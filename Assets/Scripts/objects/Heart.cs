using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    private int heartsCount {get;}

    public static Heart Instance { private set; get; }
    // Start is called before the first frame update
 
    public void UseHeart() 
    {
        int heartsCount = PlayerPrefs.GetInt("heart");
        PlayerPrefs.SetInt("heart", heartsCount - 1);
        GameManager.Instance.Properties.AddToIntProperty("restartCount", 1);
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
