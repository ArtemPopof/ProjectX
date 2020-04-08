﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public bool isReversed = false;

    public bool Tap { get { return tap;} }
    public Vector2 SwipeDelta { get { return swipeDelta; } }
    public bool SwipeLeft { get { return swipeLeft; } }
    public bool SwipeRight { get { return swipeRight; } }
    public bool SwipeDown { get { return swipeDown; } }
    public bool SwipeUp { get { return swipeUp; } }

    private void Awake() {
        Instance = this;
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
    }
}