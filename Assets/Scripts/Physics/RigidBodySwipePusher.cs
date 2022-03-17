using PlayerInput;
using UnityEngine;

namespace Physics
{
    public class RigidBodySwipePusher : MonoBehaviour
    {
        [SerializeField] private Transform mainCameraTransform;
        [SerializeField] private SwipeDetector swipeDetector;
        [SerializeField] private Rigidbody myRigidbody;
        [SerializeField] private float forceFactor = 100f;

        private void OnEnable()
        {
            //Subscribe to events
            swipeDetector.OnSwipeDown += ApplyForceFromSwipe;
            swipeDetector.OnSwipeUp += ApplyForceFromSwipe;
            swipeDetector.OnSwipeLeft += ApplyForceFromSwipe;
            swipeDetector.OnSwipeRight += ApplyForceFromSwipe;
        }

        private void OnDisable()
        {
            //Unsubscribe from events
            swipeDetector.OnSwipeDown -= ApplyForceFromSwipe;
            swipeDetector.OnSwipeUp -= ApplyForceFromSwipe;
            swipeDetector.OnSwipeLeft -= ApplyForceFromSwipe;
            swipeDetector.OnSwipeRight -= ApplyForceFromSwipe;
        }

        private void ApplyForceFromSwipe(Vector2 swipeVector)
        {
            var forceVector = new Vector3(swipeVector.x, 0, swipeVector.y);
            //This rotates the vector to be relative to the camera transform's rotation. The quaternion MUST be before the vector.
            // forceVector = mainCameraTransform.rotation * forceVector;

            //Here we also remove the horizontal part of the camera's rotation.
            var cameraForward = mainCameraTransform.forward;
            cameraForward.y = 0; //Removes the vertical part of the camera's forward direction.
            cameraForward.Normalize(); //Make sure to normalize before creating your new rotation.
            //Create a new rotation based on the camera's forward direction without the vertical part.
            var cameraRotationForcedToHorizontalPlane = Quaternion.LookRotation(cameraForward, Vector3.up);
            forceVector = cameraRotationForcedToHorizontalPlane * forceVector;

            myRigidbody.AddForce(forceVector * forceFactor);
        }
    }
}