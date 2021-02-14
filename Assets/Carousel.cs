using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Carousel : MonoBehaviour
{
    public Vector3 swipeVector = new Vector3(1000, 0, 1);
    public Button leftSwipeArrow;
    public Button rightSwipeArrow;
    public Vector3 startPosition;
    public Vector3 desiredLocation;
    private int currentIndex = 0;
    private int maxIndex;
    private List<Action<ShopItem>> listeners = new List<Action<ShopItem>>();
    // actually move carousel (prefabs in carousel) when swiping
    public bool enableCarouselMotion = false;
    public List<ShopItem> items;

    // Start is called before the first frame update
    void Awake()
    {
        startPosition = transform.position;
        maxIndex = items.Count -1;
        desiredLocation = transform.position;
    }

    void Start()
    {
        leftSwipeArrow.onClick.AddListener(() => { SwipeLeft(); });
        rightSwipeArrow.onClick.AddListener(() => { SwipeRight(); });
    }

    public void SetIndex(int index)
    {
        currentIndex = index;
        desiredLocation = startPosition - currentIndex * swipeVector;
        leftSwipeArrow.interactable = true;
        rightSwipeArrow.interactable = true;

        if (currentIndex == maxIndex)
        {
            rightSwipeArrow.interactable = false;
        }
        if (currentIndex == 0)
        {
            leftSwipeArrow.interactable = false;
        }

        SoundManager.PlaySound("swipe1");
        notifyListeners();
    }

    public void SwipeLeft()
    {
        currentIndex--;
        SetIndex(currentIndex);
    }

    public void SwipeRight()
    {
        currentIndex++;
        SetIndex(currentIndex);
    }

    private void notifyListeners()
    {
        foreach (var listener in listeners)
        {
            listener.Invoke(items[currentIndex]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (enableCarouselMotion)
        {
            transform.position = Vector3.Lerp(transform.position, desiredLocation, Time.deltaTime * 2);
        }
    }

    public void AddOnSwipeListener(Action<ShopItem> onSwipe)
    {
        listeners.Add(onSwipe);
    }
}
