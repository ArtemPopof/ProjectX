using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hard : MonoBehaviour
{
    private int hardCount {get;}

    public static Hard Instance { private set; get; }
    // Start is called before the first frame update
 
    public void UseHard() 
    {
        int hardCount = PlayerPrefs.GetInt("hard");
        PlayerPrefs.SetInt("hard", hardCount - 1);
        GameManager.Instance.Properties.AddToIntProperty("restartCount", 1);
    }

    public bool IsHardUsedThreeTimes()
    {
        bool flag = false;
        if (GameManager.Instance.Properties.GetInt("restartCount") == 3)
        {
            flag = true;
        }
        return flag;
    }
}
