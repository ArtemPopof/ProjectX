using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CircleProgressBar : MonoBehaviour
{
    public float time;
    private float timeLeft;
    public Text timeLabel;
    public Image progressImage;
    public EventTrigger.TriggerEvent whenTimeEnds;
    void Start()
    {
        //timeLeft = time;
    }

    private void Awake()
    {
        timeLeft = time;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeLeft <= 0)
        {
            timeLeft = time;
            whenTimeEnds.Invoke(null);
            return;
        }

        timeLabel.text = ((int)timeLeft).ToString();
        progressImage.fillAmount = timeLeft / time;

        timeLeft -= Time.deltaTime;
    }
}
