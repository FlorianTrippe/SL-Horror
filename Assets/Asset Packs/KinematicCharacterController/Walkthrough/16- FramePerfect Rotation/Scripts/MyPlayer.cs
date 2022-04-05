using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;
using KinematicCharacterController.Examples;
using System.Linq;
using UnityEngine.InputSystem;

namespace KinematicCharacterController.Walkthrough.FramePerfectRotation
{
    public class MyPlayer : MonoBehaviour
    {
        public ExampleCharacterCamera OrbitCamera;
        public Transform CameraFollowPoint;
        public MyCharacterController Character;

        private const string MouseXInput = "Mouse X";
        private const string MouseYInput = "Mouse Y";
        private const string MouseScrollInput = "Mouse ScrollWheel";
        private const string HorizontalInput = "Horizontal";
        private const string VerticalInput = "Vertical";

        private float _mouseLookAxisUp;
        private float _mouseLookAxisRight;
        private float _scrollInput;
        private Vector2 _moveVector;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;

            // Tell camera to follow transform
            OrbitCamera.SetFollowTransform(CameraFollowPoint);

            // Ignore the character's collider(s) for camera obstruction checks
            OrbitCamera.IgnoredColliders.Clear();
            OrbitCamera.IgnoredColliders.AddRange(Character.GetComponentsInChildren<Collider>());
        }

        private void Update()
        {
            // if (Input.GetMouseButtonDown(0))
            // {
            //     Cursor.lockState = CursorLockMode.Locked;
            // }

            HandleCharacterInput();
        }

        private void LateUpdate()
        {
            HandleCameraInput();
            Character.PostInputUpdate(Time.deltaTime, OrbitCamera.transform.forward);
        }

        public void InputMouseDelta(InputAction.CallbackContext context)
        {
            Vector2 MouseDelta = context.ReadValue<Vector2>();
            _mouseLookAxisUp = MouseDelta.y;
            _mouseLookAxisRight = MouseDelta.x;
            if (context.performed)
            {
                
            }
            else if (context.canceled)
            {
                
            }
            
        }
        public void InputMouseScrollWheel(InputAction.CallbackContext context)
        {
            Vector2 ScrollVector = context.ReadValue<Vector2>();
            _scrollInput = -ScrollVector.y;
            if (context.performed)
            {
            }
            else if (context.canceled)
            {

            }
        }
        public void InputMove(InputAction.CallbackContext context)
        {
            _moveVector = context.ReadValue<Vector2>();
            if (context.performed)
            {
            }
            else if (context.canceled)
            {
            }
        }
        public void InputJump(InputAction.CallbackContext context)
        {
            if (context.performed)
            {

            }
            else if (context.canceled)
            {

            }
        }
        public void InputMouseClickRight(InputAction.CallbackContext context)
        {
            if (context.performed)
            {

            }
            else if (context.canceled)
            {

            }
        }
        public void InputMouseClickLeft(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OrbitCamera.TargetDistance = (OrbitCamera.TargetDistance == 0f) ? OrbitCamera.DefaultDistance : 0f;
            }
            else if (context.canceled)
            {

            }
        }

        private void HandleCameraInput()
        {
            // Create the look input vector for the camera
            // _mouseLookAxisUp = Input.GetAxisRaw(MouseYInput);
            // _mouseLookAxisRight = Input.GetAxisRaw(MouseXInput);
            Vector3 lookInputVector = new Vector3(_mouseLookAxisRight, _mouseLookAxisUp, 0f);

            // Prevent moving the camera while the cursor isn't locked
            if (Cursor.lockState != CursorLockMode.Locked)
            {
                lookInputVector = Vector3.zero;
            }

            // Input for zooming the camera (disabled in WebGL because it can cause problems)
            // _scrollInput = -Input.GetAxis(MouseScrollInput);
#if UNITY_WEBGL
        scrollInput = 0f;
#endif

            // Apply inputs to the camera
            OrbitCamera.UpdateWithInput(Time.deltaTime, _scrollInput, lookInputVector);

            // Handle toggling zoom level

            // if (Input.GetMouseButtonDown(1))
            // {
            // }
        }

        private void HandleCharacterInput()
        {
            PlayerCharacterInputs characterInputs = new PlayerCharacterInputs();

            // Build the CharacterInputs struct
            // characterInputs.MoveAxisForward = Input.GetAxisRaw(VerticalInput);
            // characterInputs.MoveAxisRight = Input.GetAxisRaw(HorizontalInput);
            characterInputs.MoveAxisForward = _moveVector.y;
            characterInputs.MoveAxisRight = _moveVector.x;
            characterInputs.CameraRotation = OrbitCamera.Transform.rotation;

            // Apply inputs to character
            Character.SetInputs(ref characterInputs);
        }
    }
}