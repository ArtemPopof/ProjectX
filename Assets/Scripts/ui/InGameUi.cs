using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUi : MonoBehaviour
{
    public Text score;
    public Text coins;
    public Text multiplier;
    void Start()
    {
        GameManager.Instance.Properties.bind(score, "score");
        GameManager.Instance.Properties.bind(coins, "coins");
        GameManager.Instance.Properties.bind(multiplier, "multiplier");
    }

    void Update()
    {
    }
}
