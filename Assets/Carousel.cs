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
    public Vector3 desiredLocation;
    private int currentIndex = 0;
    private int maxIndex;
    private List<Action<int>> listeners = new List<Action<int>>();

    // Start is called before the first frame update
    void Start()
    {
        maxIndex = transform.childCount - 1;
        leftSwipeArrow.interactable = false;
        desiredLocation = transform.position;
        leftSwipeArrow.onClick.AddListener(() => { SwipeLeft(); });
        rightSwipeArrow.onClick.AddListener(() => { SwipeRight(); });
    }

    public void SwipeLeft()
    {
        if (currentIndex == 0)
        {
            return;
        }
        desiredLocation = transform.position + swipeVector;
        currentIndex--;
        rightSwipeArrow.interactable = true;
        if (currentIndex == 0)
        {
            leftSwipeArrow.interactable = false;
        }

        notifyListeners();
    }

    public void SwipeRight()
    {
        if (currentIndex == maxIndex)
        {
            return;
        }
        desiredLocation = transform.position - swipeVector;
        leftSwipeArrow.interactable = true;
        currentIndex++;
        if (currentIndex == maxIndex)
        {
            rightSwipeArrow.interactable = false;
        }

        notifyListeners();
    }

    private void notifyListeners()
    {
        foreach (var listener in listeners)
        {
            listener.Invoke(currentIndex);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, desiredLocation, Time.deltaTime * 2);
    }

    public void AddOnSwipeListener(Action<int> onSwipe)
    {
        listeners.Add(onSwipe);
    }
}
