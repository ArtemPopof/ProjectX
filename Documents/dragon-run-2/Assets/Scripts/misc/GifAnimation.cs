using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GifAnimation : MonoBehaviour
{
    public Sprite[] sprites;
    public int fps = 15;

    private Image imageWidget;

    void Start()
    {
        imageWidget = transform.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        var index = (int) (Time.time * fps % sprites.Length);
        imageWidget.sprite = sprites[index];
    }
}
