using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUi : MonoBehaviour
{
    public Text score;
    public Text distance;
    public Text multiplier;
    void Start()
    {
        GameManager.Instance.Properties.bind(score, "score");
        GameManager.Instance.Properties.bind(distance, "distance");
        GameManager.Instance.Properties.bind(multiplier, "multiplier");
    }

    void Update()
    {
    }
}
