﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
public class MobileInput : MonoBehaviour
{
    private const float DEADZONE = 100.0f;
    public static MobileInput Instance { set; get; }
    private bool tap;
    private bool swipeLeft;
    private bool swipeRight;
    private bool swipeUp;
    private bool swipeDown;
    private Vector2 swipeDelta;
    private Vector2 startTouch;

    private List<Action<Vector2>> onTapActions;

    public bool isReversed = false;

    private Vector3 previousTapPos = Vector2.zero;

    public Transform shopButton;

    public bool Tap { get { return tap;} }

    public bool TapButton { get; set; }
    public Vector2 SwipeDelta { get { return swipeDelta; } }
    public bool SwipeLeft { get { return swipeLeft; } }
    public bool SwipeRight { get { return swipeRight; } }
    public bool SwipeDown { get { return swipeDown; } }
    public bool SwipeUp { get { return swipeUp; } }

    private void Awake() {
        Instance = this;

        onTapActions = new List<Action<Vector2>>();
    }

    private void Update() {
        // Reseting all the booleans
        tap = false;
        swipeLeft = false;
        swipeRight = false;
        swipeDown = false;
        swipeUp = false;

        // Let's check for inputs 
        #region Standalone Inputs
        if (Input.GetMouseButtonDown(0)) {
            //TapButton = IsButtonTapped(Input.mousePosition);
            tap = true;
            startTouch = Input.mousePosition;
        } else if (Input.GetMouseButtonUp(0)) {
            startTouch = Vector2.zero;
            swipeDelta = Vector2.zero; 
        }
        #endregion

        #region Mobile Inputs
        if (Input.touches.Length != 0) {
            if (Input.touches[0].phase == TouchPhase.Began) {
                tap = true;
                //TapButton = IsButtonTapped(Input.touches);
                startTouch = Input.mousePosition;
            } else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled) {
                startTouch = Vector2.zero;
                swipeDelta = Vector2.zero; 
            }
        }
        #endregion
    
        // Caclulate distance
        swipeDelta = Vector2.zero;
        if (startTouch != Vector2.zero) {
            // Let's check with mobile
            if (Input.touches.Length != 0) {
                swipeDelta = Input.touches[0].position - startTouch;
            }
            // Let's check with standalone 
            else if (Input.GetMouseButton(0)) {
                swipeDelta = (Vector2) Input.mousePosition - startTouch;
            }
        }

        // Checck that we're beyond the deadzone
        if (swipeDelta.magnitude > DEADZONE) {
            // This is a confirmed swipe
            float x = swipeDelta.x;
            float y = swipeDelta.y;
            
            if (Mathf.Abs(x) > Mathf.Abs(y)) {
                // Left or Right
                if (x < 0) {
                    swipeLeft = true;
                } else {
                    swipeRight = true;
                }

                if (isReversed)
                {
                    swipeLeft = !swipeLeft;
                    swipeRight = !swipeRight;
                }
            } else {
                // Up or Down
                if (y < 0)  {
                    swipeDown = true;
                } else {
                    swipeUp = true;
                }
            }

            startTouch = Vector2.zero;
            swipeDelta = Vector2.zero;
        }

        if ((tap || TapButton) && !swipeDown && !swipeUp && !swipeLeft && !swipeRight && (GameManager.Instance == null || !GameManager.Instance.IsRunning))
        {
            onTapActions.ForEach((action) => action.Invoke(GetTapOrClickPosition()));
            SoundManager.PlaySound("Click");
        }
    }

    private Vector2 GetTapOrClickPosition()
    {
        if (Input.touches.Length != 0)
        {
            return Input.touches[0].position;
        }
        else
        {
            return Input.mousePosition;
        }
    }

    private bool IsButtonTapped(Touch[] touches)
    {
        if (shopButton == null) return false;

        var buttonsZone = Screen.height - shopButton.position.y + 100;

        foreach (Touch touch in touches)
        {
            var realPosition = new Vector2(touch.position.x, Screen.height - touch.position.y);

            //var factRect = shopButton.GetComponent<RectTransform>().rect;
            //factRect.position = new Vector2(factRect.position.x * -1, factRect.position.y * -1);
            //factRect.size = factRect.size * 1.5f;
            //var heightScale = Screen.height / shopButton.GetComponentInParent<CanvasScaler>().referenceResolution.y;
            //factRect.size *= heightScale;

            //if (factRect.Contains(realPosition))
            //{
            //    return true;
            //}

            if (realPosition.y < buttonsZone)
            {
                return true;
            }
        }

        return false;
    }

    private bool IsButtonTapped(Vector3 position)
    {
        var realPosition = new Vector3(position.x, Screen.height - position.y, position.z);

        //var factRect = shopButton.GetComponent<RectTransform>().rect;
        //factRect.position = new Vector2(factRect.position.x * -1, factRect.position.y * -1);
        //factRect.size = factRect.size * 1.5f; // le petit kostyl to make the shop button clickable
        //var heightScale = Screen.height / shopButton.GetComponentInParent<CanvasScaler>().referenceResolution.y;
        //factRect.size *= heightScale;
        //return factRect.Contains(realPosition);

        var buttonsZone = Screen.height - shopButton.position.y + 100;

        Debug.Log("POS: " + shopButton.position);

        Debug.Log("ButtonsZone: " + buttonsZone);
        Debug.Log("RealPosition: " + realPosition);

        return realPosition.y < buttonsZone;
    }

    public void OnTap(Action<Vector2> onTap)
    {
        onTapActions.Add(onTap);
    }
}
