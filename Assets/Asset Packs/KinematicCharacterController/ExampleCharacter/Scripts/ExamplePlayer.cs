using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;
using KinematicCharacterController.Examples;
using UnityEngine.InputSystem;
using GameSettings;

namespace KinematicCharacterController.Examples
{
    public class ExamplePlayer : MonoBehaviour
    {
        [Header("Scriptable Objects")] 
        [SerializeField] private SO_Settings _soSettings;

        [Header("Menus")]
        [SerializeField] private PauseMenuManager _pauseMenuManager;

        public ExampleCharacterController Character;
        public ExampleCharacterCamera CharacterCamera;
        public CableScripts CableScript;
        //public float MouseSensibilityX = 0.2f;
        //public float MouseSensibilityY = 0.2f;

        private bool _jumpBool;
        private bool _crouchBoolDown;
        private bool _crouchBoolUp;
        private bool _runBoolDown;
        private bool _runBoolUp;
        private bool _geigerStatus;
        private bool _lampStatus;
        private bool _chargerStatus;
        private bool _isWalking;
        private float _scrollInput;
        private float _mouseLookAxisUp;
        private float _mouseLookAxisRight;
        private Vector2 _moveVector;
        private bool _paused;
        [SerializeField] private Animator _playerSteps;

        public bool ReadyToCharge = false;

        [SerializeField] private Animator _anim;
        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;

            // Tell camera to follow transform
            CharacterCamera.SetFollowTransform(Character.CameraFollowPoint);
            CharacterCamera.TargetDistance = 0f;

            // Ignore the character's collider(s) for camera obstruction checks
            CharacterCamera.IgnoredColliders.Clear();
            CharacterCamera.IgnoredColliders.AddRange(Character.GetComponentsInChildren<Collider>());
        }

        private void Update()
        {
            HandleCharacterInput();
            if(_moveVector.y > 0 || _moveVector.x > 0)
                _playerSteps.SetBool("IsWalking", true);
            else
                _playerSteps.SetBool("IsWalking", false);
        }

        private void LateUpdate()
        {
            // Handle rotating the camera along with physics movers
            if (CharacterCamera.RotateWithPhysicsMover && Character.Motor.AttachedRigidbody != null)
            {
                CharacterCamera.PlanarDirection = Character.Motor.AttachedRigidbody.GetComponent<PhysicsMover>().RotationDeltaFromInterpolation * CharacterCamera.PlanarDirection;
                CharacterCamera.PlanarDirection = Vector3.ProjectOnPlane(CharacterCamera.PlanarDirection, Character.Motor.CharacterUp).normalized;
            }

            HandleCameraInput();
        }

        public void InputMove(InputAction.CallbackContext context)
        {
            _moveVector = context.ReadValue<Vector2>();
           /*
            if(context.phase == InputActionPhase.Performed)
            {
                _playerSteps.SetBool("IsWalking", true);
            }
            if(context.phase == InputActionPhase.Canceled)
            {
                _playerSteps.SetBool("IsWalking", false);
            }
           */
        }
        public void InputLook(InputAction.CallbackContext context)
        {
            Vector2 MouseDelta = context.ReadValue<Vector2>();
            _mouseLookAxisUp = MouseDelta.y * _soSettings.mouseSensivityValue;
            _mouseLookAxisRight = MouseDelta.x * _soSettings.mouseSensivityValue;

            if (_soSettings.mouseXInvertValue)
                _mouseLookAxisRight *= -1f;

            if (_soSettings.mouseYInvertValue)
                _mouseLookAxisUp *= -1f;
        }
        public void InputScroll(InputAction.CallbackContext context)
        {
        }
        public void InputLeftClick(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                if (ReadyToCharge)
                {
                    CableScript.Charge();
                    _anim.SetBool("Charging", true);
                }
                else
                {
                    Debug.Log("Click");
                    CableScript.Interact();                    
                }
            }
            else
            {
                _anim.SetBool("Charging", false);
            }
        }
        public void InputRightClick(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                Debug.Log("Click Right");
                CableScript.EquipCharger();
                ReadyToCharge = true;
            }
            if(context.phase == InputActionPhase.Canceled)
            {
                CableScript.EquipLastItem();
                ReadyToCharge = false;
            }
        }
        public void InputHold(InputAction.CallbackContext context)
        {

            if (context.phase == InputActionPhase.Performed)
            {
                Debug.Log("Hold Click");
            }
            else
            {
            }
        }
        public void InputCrouch(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                _crouchBoolDown = true;
                _crouchBoolUp = false;
            }
            else
            {
                _crouchBoolDown = !true;
                _crouchBoolUp = !false;
            }
        }
        public void InputRun(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                _runBoolDown = true;
                _runBoolUp = false;
            }
            else
            {
                _runBoolDown = !true;
                _runBoolUp = !false;
            }
        }
        public void InputGeiger(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                CableScript.EquipGeiger();
            }

        }
        public void InputFlashLight(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                CableScript.EquipFlashLight();
            }
        }
        public void InputCharger(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                CableScript.EquipCharger();
            }
            else
            {
                CableScript.EquipCharger();
            }
        }
        public void InputSwitchMaskFilters(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                CableScript.ChangeFilter();
                CableScript.DropKey();
                
            }
            else
            {
                _anim.SetBool("Reload", false);
            }
            
        }
        public void InputTurnOnOff(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                CableScript.TurnOnOFF();
            }
        }
        public void Menu(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                _pauseMenuManager.OpenMenu();
            }
        }

        private void HandleCameraInput()
        {
            // Create the look input vector for the camera
            Vector3 lookInputVector = new Vector3(_mouseLookAxisRight, _mouseLookAxisUp, 0f);

            // Prevent moving the camera while the cursor isn't locked
            if (Cursor.lockState != CursorLockMode.Locked)
            {
                lookInputVector = Vector3.zero;
            }
            

            // Apply inputs to the camera
            CharacterCamera.UpdateWithInput(Time.deltaTime, _scrollInput, lookInputVector);
        }

        private void HandleCharacterInput()
        {
            PlayerCharacterInputs characterInputs = new PlayerCharacterInputs();

            // Build the CharacterInputs struct
            characterInputs.MoveAxisForward = _moveVector.y;
            characterInputs.MoveAxisRight = _moveVector.x;
            characterInputs.CameraRotation = CharacterCamera.Transform.rotation;
            characterInputs.JumpDown = _jumpBool;
            characterInputs.CrouchDown = _crouchBoolDown;
            characterInputs.CrouchUp = _crouchBoolUp;
            characterInputs.RunDown = _runBoolDown;
            characterInputs.RunUp = _runBoolUp;

            // Apply inputs to character
            Character.SetInputs(ref characterInputs);
        }
    }
}