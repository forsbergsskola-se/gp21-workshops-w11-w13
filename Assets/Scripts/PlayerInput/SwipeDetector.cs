using System;
using UnityEngine;

namespace PlayerInput
{
    public class SwipeDetector : MonoBehaviour
    {
        [SerializeField, Header("Swipe settings")]
        private float swipeDetectionTimeFrame = 0.5f;

        [SerializeField] private float swipeMinimumRequiredScreenFraction = 0.2f;

        //TODO Talk briefly about assemblies and assembly definition files.

        public Action<Vector2> OnSwipeUp, OnSwipeDown, OnSwipeLeft, OnSwipeRight; //Does the same as the 4 commented lines below.
        // public Action<float> OnSwipeUp;
        // public Action<float> OnSwipeDown;
        // public Action<float> OnSwipeLeft;
        // public Action<float> OnSwipeRight;

        private Vector2 pointerStartPosition;
        private Vector2 pointerEndPosition;

        private float pointerStartTime = 1f;

        private void Update()
        {
            GetTouchInput();
            GetMouseInput();
        }

        private void GetTouchInput()
        {
            if (Input.touchCount <= 0)
                return;

            if (Input.GetTouch(0).phase == TouchPhase.Began)
                OnTouchStart(Input.GetTouch(0).position);
            if (Input.GetTouch(0).phase == TouchPhase.Ended)
                OnTouchEnd(Input.GetTouch(0).position);
        }

        private void GetMouseInput()
        {
            if (Input.GetMouseButtonDown(0))
                OnTouchStart(Input.mousePosition);
            if (Input.GetMouseButtonUp(0))
                OnTouchEnd(Input.mousePosition);
        }

        private void OnTouchStart(Vector3 pointerPosition)
        {
            pointerStartPosition = pointerPosition;
            pointerStartTime = Time.time;
        }

        private void OnTouchEnd(Vector3 pointerPosition)
        {
            pointerEndPosition = pointerPosition;

            //Make sure that too much time hasn't passed.
            if (Time.time < pointerStartTime + swipeDetectionTimeFrame)
                CheckForSwipe(pointerStartPosition, pointerEndPosition, swipeMinimumRequiredScreenFraction);
        }

        //Made method static to minimize state in method and class.
        private void CheckForSwipe(Vector2 start, Vector2 end, float minimumDistance)
        {
            var positionDelta = end - start;

            //Divide positionDelta by screen width and height to remap value range from pixels to screen fraction. This makes it resolution independent.
            positionDelta /= new Vector2(Screen.width, Screen.height);

            //Make sure the user moved their finger far enough.
            if (positionDelta.magnitude < minimumDistance)
                return;

            // Debug.Log($"PositionDelta: {positionDelta}");
            // Debug.Log($"Swipe magnitude: {positionDelta.magnitude}");

            //Check for horizontal swipe
            if (Mathf.Abs(positionDelta.x) > Mathf.Abs(positionDelta.y))
            {
                if (positionDelta.x > 0)
                {
                    Debug.Log("Swipe right");
                    OnSwipeRight?.Invoke(new Vector2(positionDelta.x, 0));
                }

                if (positionDelta.x < 0)
                {
                    Debug.Log("Swipe left");
                    OnSwipeLeft?.Invoke(new Vector2(positionDelta.x, 0));
                }
            }
            //Check for vertical swipe
            else
            {
                if (positionDelta.y > 0)
                {
                    Debug.Log("Swipe up");
                    OnSwipeUp?.Invoke(new Vector2(0, positionDelta.y));
                }

                if (positionDelta.y < 0)
                {
                    Debug.Log("Swipe down");
                    OnSwipeDown?.Invoke(new Vector2(0, positionDelta.y));
                }
            }
        }
    }
}