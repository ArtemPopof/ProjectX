using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartButton : MonoBehaviour
{
    public static HeartButton Instance { private set; get; }
    public void Start()
    {
        if (PlayerPrefs.GetInt("heart") == 0)
        {
            gameObject.SetActive(false);
        }
    }

    public void UserHeart()
    {
      int heartsCount = PlayerPrefs.GetInt("heart");
      PlayerPrefs.SetInt("heart", heartsCount - 1);
      GameManager.Instance.Resurrect();
      gameObject.SetActive(false);
    }
}
