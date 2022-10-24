using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartButton : MonoBehaviour
{
    public static HeartButton Instance { private set; get; }
    public void Start()
    {
        if (PlayerPrefs.GetInt("heart") == 0)
        {
            var button = gameObject.GetComponent<Button>();
            button.interactable = false;
        }
    }

    public void UserHeart()
    {
      int heartsCount = PlayerPrefs.GetInt("heart");
      PlayerPrefs.SetInt("heart", heartsCount - 1);
      GameManager.Instance.Resurrect();
      var button = gameObject.GetComponent<Button>();
      button.interactable = false;
    }
}
