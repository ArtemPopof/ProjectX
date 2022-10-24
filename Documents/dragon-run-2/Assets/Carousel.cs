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
    public bool enableCarouselAspectOffset = false;
    public float debugAspect = -1f;
    public List<ShopItem> items;

    private const float targetAspectRatio = 2.0555f;

    // Start is called before the first frame update
    void Awake()
    {
        startPosition = transform.position;
        maxIndex = items.Count -1;
        desiredLocation = transform.position;

        if (!enableCarouselAspectOffset) return;

        var heightIsBigger = Screen.currentResolution.height > Screen.currentResolution.width;
        var realScreenAspectRatio = heightIsBigger ?
            Screen.currentResolution.height * 1f / Screen.currentResolution.width :
            Screen.currentResolution.width * 1f / Screen.currentResolution.height;

        if (debugAspect != -1f) realScreenAspectRatio = debugAspect;

        var offsetX = swipeVector.x += (targetAspectRatio - realScreenAspectRatio) * 359;
        swipeVector.x = offsetX;

        var scale = realScreenAspectRatio / targetAspectRatio;
        items.ForEach((item) =>
        {
            item.gameObject.transform.localScale = new Vector3(scale, scale, scale);
        });

        Debug.Log("New offset: " + offsetX);
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
