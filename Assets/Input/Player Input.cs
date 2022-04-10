//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.3.0
//     from Assets/Input/Player Input.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerInput : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Player Input"",
    ""maps"": [
        {
            ""name"": ""PlayerActionMap"",
            ""id"": ""0dc5fb39-4f47-4d5f-a856-0f1fe0f3d676"",
            ""actions"": [
                {
                    ""name"": ""Mouse Input"",
                    ""type"": ""PassThrough"",
                    ""id"": ""920fe338-949c-4ba2-9327-692777ed463e"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Mouse ScrollWheel"",
                    ""type"": ""Value"",
                    ""id"": ""5d05c155-b48f-49e0-8344-2e123af6d954"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Move"",
                    ""type"": ""PassThrough"",
                    ""id"": ""08d2471d-d194-4103-8a15-1a44905675d9"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Mouse Klick Right"",
                    ""type"": ""Button"",
                    ""id"": ""7c2c93c3-080f-413b-a352-15270c9e853f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Interact Hold"",
                    ""type"": ""Button"",
                    ""id"": ""9f15b743-4d66-486c-9a0d-b4f06672040b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Hold(duration=0.6,pressPoint=0.5)"",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Mouse Klick Left"",
                    ""type"": ""Button"",
                    ""id"": ""fe74e323-fa4a-4b42-b697-045d163c6785"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Tap(duration=0.5)"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Value"",
                    ""id"": ""88797fbf-d8d4-4bcb-9bec-bb635f1d9ae9"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Crouch"",
                    ""type"": ""Value"",
                    ""id"": ""53d07469-fc86-41b5-af3d-9ba0d2ddaf35"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Run"",
                    ""type"": ""Button"",
                    ""id"": ""eb487365-ee63-42a6-a49a-d3d826e3b6c4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SwitchFilter"",
                    ""type"": ""Button"",
                    ""id"": ""09881fa7-9beb-4ec9-a345-67188a881c9c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Geiger"",
                    ""type"": ""Button"",
                    ""id"": ""488c1478-cae5-484c-bacf-e3b538b3d22d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""FlashLight"",
                    ""type"": ""Button"",
                    ""id"": ""caa86691-443b-443b-9904-d84f919ade16"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""On / Off"",
                    ""type"": ""Button"",
                    ""id"": ""4d93e7a4-df78-475a-842c-253db43446fa"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""555b77ad-b89d-4ac4-9d3f-e348cee5a933"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PlayerControlScheme"",
                    ""action"": ""Mouse Input"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3a6850e6-52fc-49db-91ab-9373b712eb9d"",
                    ""path"": ""<Mouse>/scroll"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PlayerControlScheme"",
                    ""action"": ""Mouse ScrollWheel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Move"",
                    ""id"": ""f36c5bf7-3ba1-4788-b814-e6b86589d935"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""622e5cb0-a012-4c2a-a1b1-97a843bb2653"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PlayerControlScheme"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""18732067-a625-4bd7-b31d-daf4df6729e9"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PlayerControlScheme"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""614f675f-81b8-4163-8e0a-f88c707c266b"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PlayerControlScheme"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""1be308d4-9d1a-4377-a79f-17337f607b70"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PlayerControlScheme"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""41a7aec0-bb04-4cb0-a0a0-6edc2ff41afb"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PlayerControlScheme"",
                    ""action"": ""Mouse Klick Right"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""56afb1df-140b-4afe-bb7c-58066ac311c5"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PlayerControlScheme"",
                    ""action"": ""Mouse Klick Left"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6a2367a8-15a7-467c-add3-b802b7c72d8f"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PlayerControlScheme"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2a9ed897-349b-47b0-af1d-67db555b03d6"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PlayerControlScheme"",
                    ""action"": ""Crouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f7ed9eb3-417e-4bcc-b3c4-2f0c9f5ab44d"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PlayerControlScheme"",
                    ""action"": ""Run"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c02ba159-b910-4225-81e9-c913ec52a5a3"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PlayerControlScheme"",
                    ""action"": ""SwitchFilter"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c74f4d67-54f3-49c6-aa0e-c3df72b11f90"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PlayerControlScheme"",
                    ""action"": ""Geiger"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f52419c7-3304-47cd-b460-016131680554"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PlayerControlScheme"",
                    ""action"": ""FlashLight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""03ec0a6c-09b2-46f0-a1c7-23ece7ee7a9d"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PlayerControlScheme"",
                    ""action"": ""Interact Hold"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""baa8f8f5-d12d-4e12-9418-36ad97b3adcf"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PlayerControlScheme"",
                    ""action"": ""On / Off"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""PlayerControlScheme"",
            ""bindingGroup"": ""PlayerControlScheme"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": true,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": true,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": true,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<VirtualMouse>"",
                    ""isOptional"": true,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // PlayerActionMap
        m_PlayerActionMap = asset.FindActionMap("PlayerActionMap", throwIfNotFound: true);
        m_PlayerActionMap_MouseInput = m_PlayerActionMap.FindAction("Mouse Input", throwIfNotFound: true);
        m_PlayerActionMap_MouseScrollWheel = m_PlayerActionMap.FindAction("Mouse ScrollWheel", throwIfNotFound: true);
        m_PlayerActionMap_Move = m_PlayerActionMap.FindAction("Move", throwIfNotFound: true);
        m_PlayerActionMap_MouseKlickRight = m_PlayerActionMap.FindAction("Mouse Klick Right", throwIfNotFound: true);
        m_PlayerActionMap_InteractHold = m_PlayerActionMap.FindAction("Interact Hold", throwIfNotFound: true);
        m_PlayerActionMap_MouseKlickLeft = m_PlayerActionMap.FindAction("Mouse Klick Left", throwIfNotFound: true);
        m_PlayerActionMap_Jump = m_PlayerActionMap.FindAction("Jump", throwIfNotFound: true);
        m_PlayerActionMap_Crouch = m_PlayerActionMap.FindAction("Crouch", throwIfNotFound: true);
        m_PlayerActionMap_Run = m_PlayerActionMap.FindAction("Run", throwIfNotFound: true);
        m_PlayerActionMap_SwitchFilter = m_PlayerActionMap.FindAction("SwitchFilter", throwIfNotFound: true);
        m_PlayerActionMap_Geiger = m_PlayerActionMap.FindAction("Geiger", throwIfNotFound: true);
        m_PlayerActionMap_FlashLight = m_PlayerActionMap.FindAction("FlashLight", throwIfNotFound: true);
        m_PlayerActionMap_OnOff = m_PlayerActionMap.FindAction("On / Off", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // PlayerActionMap
    private readonly InputActionMap m_PlayerActionMap;
    private IPlayerActionMapActions m_PlayerActionMapActionsCallbackInterface;
    private readonly InputAction m_PlayerActionMap_MouseInput;
    private readonly InputAction m_PlayerActionMap_MouseScrollWheel;
    private readonly InputAction m_PlayerActionMap_Move;
    private readonly InputAction m_PlayerActionMap_MouseKlickRight;
    private readonly InputAction m_PlayerActionMap_InteractHold;
    private readonly InputAction m_PlayerActionMap_MouseKlickLeft;
    private readonly InputAction m_PlayerActionMap_Jump;
    private readonly InputAction m_PlayerActionMap_Crouch;
    private readonly InputAction m_PlayerActionMap_Run;
    private readonly InputAction m_PlayerActionMap_SwitchFilter;
    private readonly InputAction m_PlayerActionMap_Geiger;
    private readonly InputAction m_PlayerActionMap_FlashLight;
    private readonly InputAction m_PlayerActionMap_OnOff;
    public struct PlayerActionMapActions
    {
        private @PlayerInput m_Wrapper;
        public PlayerActionMapActions(@PlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @MouseInput => m_Wrapper.m_PlayerActionMap_MouseInput;
        public InputAction @MouseScrollWheel => m_Wrapper.m_PlayerActionMap_MouseScrollWheel;
        public InputAction @Move => m_Wrapper.m_PlayerActionMap_Move;
        public InputAction @MouseKlickRight => m_Wrapper.m_PlayerActionMap_MouseKlickRight;
        public InputAction @InteractHold => m_Wrapper.m_PlayerActionMap_InteractHold;
        public InputAction @MouseKlickLeft => m_Wrapper.m_PlayerActionMap_MouseKlickLeft;
        public InputAction @Jump => m_Wrapper.m_PlayerActionMap_Jump;
        public InputAction @Crouch => m_Wrapper.m_PlayerActionMap_Crouch;
        public InputAction @Run => m_Wrapper.m_PlayerActionMap_Run;
        public InputAction @SwitchFilter => m_Wrapper.m_PlayerActionMap_SwitchFilter;
        public InputAction @Geiger => m_Wrapper.m_PlayerActionMap_Geiger;
        public InputAction @FlashLight => m_Wrapper.m_PlayerActionMap_FlashLight;
        public InputAction @OnOff => m_Wrapper.m_PlayerActionMap_OnOff;
        public InputActionMap Get() { return m_Wrapper.m_PlayerActionMap; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActionMapActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActionMapActions instance)
        {
            if (m_Wrapper.m_PlayerActionMapActionsCallbackInterface != null)
            {
                @MouseInput.started -= m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnMouseInput;
                @MouseInput.performed -= m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnMouseInput;
                @MouseInput.canceled -= m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnMouseInput;
                @MouseScrollWheel.started -= m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnMouseScrollWheel;
                @MouseScrollWheel.performed -= m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnMouseScrollWheel;
                @MouseScrollWheel.canceled -= m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnMouseScrollWheel;
                @Move.started -= m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnMove;
                @MouseKlickRight.started -= m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnMouseKlickRight;
                @MouseKlickRight.performed -= m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnMouseKlickRight;
                @MouseKlickRight.canceled -= m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnMouseKlickRight;
                @InteractHold.started -= m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnInteractHold;
                @InteractHold.performed -= m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnInteractHold;
                @InteractHold.canceled -= m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnInteractHold;
                @MouseKlickLeft.started -= m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnMouseKlickLeft;
                @MouseKlickLeft.performed -= m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnMouseKlickLeft;
                @MouseKlickLeft.canceled -= m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnMouseKlickLeft;
                @Jump.started -= m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnJump;
                @Crouch.started -= m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnCrouch;
                @Crouch.performed -= m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnCrouch;
                @Crouch.canceled -= m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnCrouch;
                @Run.started -= m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnRun;
                @Run.performed -= m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnRun;
                @Run.canceled -= m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnRun;
                @SwitchFilter.started -= m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnSwitchFilter;
                @SwitchFilter.performed -= m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnSwitchFilter;
                @SwitchFilter.canceled -= m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnSwitchFilter;
                @Geiger.started -= m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnGeiger;
                @Geiger.performed -= m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnGeiger;
                @Geiger.canceled -= m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnGeiger;
                @FlashLight.started -= m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnFlashLight;
                @FlashLight.performed -= m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnFlashLight;
                @FlashLight.canceled -= m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnFlashLight;
                @OnOff.started -= m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnOnOff;
                @OnOff.performed -= m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnOnOff;
                @OnOff.canceled -= m_Wrapper.m_PlayerActionMapActionsCallbackInterface.OnOnOff;
            }
            m_Wrapper.m_PlayerActionMapActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MouseInput.started += instance.OnMouseInput;
                @MouseInput.performed += instance.OnMouseInput;
                @MouseInput.canceled += instance.OnMouseInput;
                @MouseScrollWheel.started += instance.OnMouseScrollWheel;
                @MouseScrollWheel.performed += instance.OnMouseScrollWheel;
                @MouseScrollWheel.canceled += instance.OnMouseScrollWheel;
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @MouseKlickRight.started += instance.OnMouseKlickRight;
                @MouseKlickRight.performed += instance.OnMouseKlickRight;
                @MouseKlickRight.canceled += instance.OnMouseKlickRight;
                @InteractHold.started += instance.OnInteractHold;
                @InteractHold.performed += instance.OnInteractHold;
                @InteractHold.canceled += instance.OnInteractHold;
                @MouseKlickLeft.started += instance.OnMouseKlickLeft;
                @MouseKlickLeft.performed += instance.OnMouseKlickLeft;
                @MouseKlickLeft.canceled += instance.OnMouseKlickLeft;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Crouch.started += instance.OnCrouch;
                @Crouch.performed += instance.OnCrouch;
                @Crouch.canceled += instance.OnCrouch;
                @Run.started += instance.OnRun;
                @Run.performed += instance.OnRun;
                @Run.canceled += instance.OnRun;
                @SwitchFilter.started += instance.OnSwitchFilter;
                @SwitchFilter.performed += instance.OnSwitchFilter;
                @SwitchFilter.canceled += instance.OnSwitchFilter;
                @Geiger.started += instance.OnGeiger;
                @Geiger.performed += instance.OnGeiger;
                @Geiger.canceled += instance.OnGeiger;
                @FlashLight.started += instance.OnFlashLight;
                @FlashLight.performed += instance.OnFlashLight;
                @FlashLight.canceled += instance.OnFlashLight;
                @OnOff.started += instance.OnOnOff;
                @OnOff.performed += instance.OnOnOff;
                @OnOff.canceled += instance.OnOnOff;
            }
        }
    }
    public PlayerActionMapActions @PlayerActionMap => new PlayerActionMapActions(this);
    private int m_PlayerControlSchemeSchemeIndex = -1;
    public InputControlScheme PlayerControlSchemeScheme
    {
        get
        {
            if (m_PlayerControlSchemeSchemeIndex == -1) m_PlayerControlSchemeSchemeIndex = asset.FindControlSchemeIndex("PlayerControlScheme");
            return asset.controlSchemes[m_PlayerControlSchemeSchemeIndex];
        }
    }
    public interface IPlayerActionMapActions
    {
        void OnMouseInput(InputAction.CallbackContext context);
        void OnMouseScrollWheel(InputAction.CallbackContext context);
        void OnMove(InputAction.CallbackContext context);
        void OnMouseKlickRight(InputAction.CallbackContext context);
        void OnInteractHold(InputAction.CallbackContext context);
        void OnMouseKlickLeft(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnCrouch(InputAction.CallbackContext context);
        void OnRun(InputAction.CallbackContext context);
        void OnSwitchFilter(InputAction.CallbackContext context);
        void OnGeiger(InputAction.CallbackContext context);
        void OnFlashLight(InputAction.CallbackContext context);
        void OnOnOff(InputAction.CallbackContext context);
    }
}
