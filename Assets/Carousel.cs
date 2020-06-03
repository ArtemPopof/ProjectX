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
    private List<Action<int>> listeners = new List<Action<int>>();

    // Start is called before the first frame update
    void Awake()
    {
        startPosition = transform.position;
        maxIndex = transform.childCount - 1;
        desiredLocation = transform.position;
    }

    void Start()
    {
        leftSwipeArrow.interactable = false;
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
    }

    public void SwipeLeft()
    {
        currentIndex--;
        SetIndex(currentIndex);

        notifyListeners();
    }

    public void SwipeRight()
    {
        currentIndex++;
        SetIndex(currentIndex);

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
