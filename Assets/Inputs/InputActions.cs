// GENERATED AUTOMATICALLY FROM 'Assets/Inputs/InputActions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InputActions : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputActions"",
    ""maps"": [
        {
            ""name"": ""PlayerControls"",
            ""id"": ""1345719c-1d05-4ed3-be7e-cb6d5f8ea4e4"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""PassThrough"",
                    ""id"": ""cca1ee53-59c8-4d72-a8e4-7d7242037cf9"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""b1832fc1-c136-4875-9185-df959fb268e4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""49d21554-79fd-47f8-812e-f991901e71d5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MoveObject"",
                    ""type"": ""Button"",
                    ""id"": ""9ed6e779-21d5-4f1e-adc4-4d3f7183a8e8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Sword"",
                    ""type"": ""Button"",
                    ""id"": ""3b9a7a68-6fc4-4324-981b-6644980b2837"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Scanner"",
                    ""type"": ""Button"",
                    ""id"": ""17cb860e-5249-4812-b38c-9493d1c2e72f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""de81fb8b-f082-4991-a67f-6a9e47d977be"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4bce0d88-fd81-45bc-9626-f0c5903e3d3f"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""633a8f47-9366-4054-8923-a346e1442fef"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4589cd02-57d8-4475-8dd0-798e3d63f1b3"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveObject"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d0636181-0a1f-4bb8-9315-07e27536ab22"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Sword"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b916a3be-9e00-41a7-8fe0-ce49eec8a499"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Scanner"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // PlayerControls
        m_PlayerControls = asset.FindActionMap("PlayerControls", throwIfNotFound: true);
        m_PlayerControls_Move = m_PlayerControls.FindAction("Move", throwIfNotFound: true);
        m_PlayerControls_Jump = m_PlayerControls.FindAction("Jump", throwIfNotFound: true);
        m_PlayerControls_Attack = m_PlayerControls.FindAction("Attack", throwIfNotFound: true);
        m_PlayerControls_MoveObject = m_PlayerControls.FindAction("MoveObject", throwIfNotFound: true);
        m_PlayerControls_Sword = m_PlayerControls.FindAction("Sword", throwIfNotFound: true);
        m_PlayerControls_Scanner = m_PlayerControls.FindAction("Scanner", throwIfNotFound: true);
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

    // PlayerControls
    private readonly InputActionMap m_PlayerControls;
    private IPlayerControlsActions m_PlayerControlsActionsCallbackInterface;
    private readonly InputAction m_PlayerControls_Move;
    private readonly InputAction m_PlayerControls_Jump;
    private readonly InputAction m_PlayerControls_Attack;
    private readonly InputAction m_PlayerControls_MoveObject;
    private readonly InputAction m_PlayerControls_Sword;
    private readonly InputAction m_PlayerControls_Scanner;
    public struct PlayerControlsActions
    {
        private @InputActions m_Wrapper;
        public PlayerControlsActions(@InputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_PlayerControls_Move;
        public InputAction @Jump => m_Wrapper.m_PlayerControls_Jump;
        public InputAction @Attack => m_Wrapper.m_PlayerControls_Attack;
        public InputAction @MoveObject => m_Wrapper.m_PlayerControls_MoveObject;
        public InputAction @Sword => m_Wrapper.m_PlayerControls_Sword;
        public InputAction @Scanner => m_Wrapper.m_PlayerControls_Scanner;
        public InputActionMap Get() { return m_Wrapper.m_PlayerControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerControlsActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerControlsActions instance)
        {
            if (m_Wrapper.m_PlayerControlsActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMove;
                @Jump.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnJump;
                @Attack.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnAttack;
                @Attack.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnAttack;
                @Attack.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnAttack;
                @MoveObject.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMoveObject;
                @MoveObject.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMoveObject;
                @MoveObject.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMoveObject;
                @Sword.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnSword;
                @Sword.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnSword;
                @Sword.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnSword;
                @Scanner.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnScanner;
                @Scanner.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnScanner;
                @Scanner.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnScanner;
            }
            m_Wrapper.m_PlayerControlsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Attack.started += instance.OnAttack;
                @Attack.performed += instance.OnAttack;
                @Attack.canceled += instance.OnAttack;
                @MoveObject.started += instance.OnMoveObject;
                @MoveObject.performed += instance.OnMoveObject;
                @MoveObject.canceled += instance.OnMoveObject;
                @Sword.started += instance.OnSword;
                @Sword.performed += instance.OnSword;
                @Sword.canceled += instance.OnSword;
                @Scanner.started += instance.OnScanner;
                @Scanner.performed += instance.OnScanner;
                @Scanner.canceled += instance.OnScanner;
            }
        }
    }
    public PlayerControlsActions @PlayerControls => new PlayerControlsActions(this);
    public interface IPlayerControlsActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnAttack(InputAction.CallbackContext context);
        void OnMoveObject(InputAction.CallbackContext context);
        void OnSword(InputAction.CallbackContext context);
        void OnScanner(InputAction.CallbackContext context);
    }
}
