using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomImage : MonoBehaviour
{
    public List<Sprite> sprites;
    public Image image;

    void Start()
    {
        var randomIndex = Random.Range(0, sprites.Count);
        image.sprite = sprites[randomIndex];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
