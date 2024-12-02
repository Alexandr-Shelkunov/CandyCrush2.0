using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alexender.Runer
{
    public class SwipeController : MonoBehaviour
    {
        public static bool tap, swipeLeft, swipeRight, swipeUp, swipeDown;
        private bool isDraging = false;
        private Vector2 startTouch, swipeDelta;

        private void Update()
        {
            tap = swipeDown = swipeUp = swipeLeft = swipeRight = false;

            if (Input.GetMouseButtonDown(0) || (Input.touches.Length > 0 && Input.touches[0].phase == TouchPhase.Began))
            {
                tap = true;
                isDraging = true;
                startTouch = Input.mousePosition;
                if (Input.touches.Length > 0)
                    startTouch = Input.touches[0].position;
            }

            if (Input.GetMouseButtonUp(0) || (Input.touches.Length > 0 && (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)))
            {
                isDraging = false;
                Reset();
            }

            swipeDelta = Vector2.zero;
            if (isDraging)
            {
                if (Input.touches.Length > 0)
                    swipeDelta = Input.touches[0].position - startTouch;
                else if (Input.GetMouseButton(0))
                    swipeDelta = (Vector2)Input.mousePosition - startTouch;
            }

            DetectSwipeDirection();
        }

        private void DetectSwipeDirection()
        {
            if (swipeDelta.magnitude > 100) 
            {
                float x = swipeDelta.x;
                float y = swipeDelta.y;

                if (Mathf.Abs(x) > Mathf.Abs(y))
                {
                    swipeLeft = x < 0;
                    swipeRight = x > 0;
                }
                else
                {
                    swipeDown = y < 0;
                    swipeUp = y > 0;
                }

                Reset();
            }
        }

        private void Reset()
        {
            startTouch = swipeDelta = Vector2.zero;
            isDraging = false;
        }
    }
}