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
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""fac264e1-e0ab-49c7-8e9f-f3b55699eeec"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MenuBack"",
                    ""type"": ""Button"",
                    ""id"": ""39ea6646-5b04-489b-8e09-e3ade1f81603"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MenuAccept"",
                    ""type"": ""Button"",
                    ""id"": ""d9cf74a5-595b-491e-8f18-88fc71c5651a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MenuNavigationUp"",
                    ""type"": ""Button"",
                    ""id"": ""69cdf099-047c-4a3c-b457-cad3e95db4a5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MenuNavigationDown"",
                    ""type"": ""Button"",
                    ""id"": ""03936ebb-91ab-4368-94cf-e33cf97e5b29"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SkipDialogue"",
                    ""type"": ""Button"",
                    ""id"": ""7e2daec0-b2a2-4b7e-b0f4-24c82a964e68"",
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
                    ""name"": ""2D Vector"",
                    ""id"": ""0c073cfd-0e6e-409d-b75e-baea9fa8d1de"",
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
                    ""id"": ""e3932140-f43c-4fe7-bbb7-da0fa48ab6f9"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""37f10b54-e60b-459a-92dc-453952987cb4"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""f6b52765-ef0a-48ff-9472-6cb979102020"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""8acbbad1-e91f-42bd-a2a8-dc68b54e0250"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
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
                    ""id"": ""c5b0e04b-299e-40ff-9399-b1916e87c3a1"",
                    ""path"": ""<Keyboard>/space"",
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
                    ""id"": ""4b52bdd4-1f0a-4a71-86bb-b81115d947a6"",
                    ""path"": ""<Mouse>/leftButton"",
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
                    ""id"": ""44d883a6-7556-4e93-925e-54114dee3d87"",
                    ""path"": ""<Keyboard>/shift"",
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
                    ""id"": ""55d5af6d-4e35-47ab-9b89-27ffe924d30c"",
                    ""path"": ""<Keyboard>/f"",
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
                },
                {
                    ""name"": """",
                    ""id"": ""56bc88de-7658-459a-8051-a1d237d6e75e"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Scanner"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""92ccfcf0-89ac-4355-a05d-58b1c5cd3d74"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""acc463a1-1d5b-4152-af9b-226322874ec9"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0c40da79-4f47-4c76-b3a2-e7b09bb6081d"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MenuBack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""939c7df3-7af2-4b70-a27f-2e7c41158ea5"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MenuAccept"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b074655a-ab43-41d0-b043-893a81ba1810"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MenuNavigationUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""825b3e4f-41bc-4217-a783-f20798a7673f"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MenuNavigationUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6572df07-5b57-4257-948f-805f3dd0dc75"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MenuNavigationDown"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e2834326-975e-4f8b-a6f4-218bb7d8f2d8"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MenuNavigationDown"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9aed35bf-367f-4c1d-8b46-92f57da62da7"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SkipDialogue"",
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
        m_PlayerControls_Pause = m_PlayerControls.FindAction("Pause", throwIfNotFound: true);
        m_PlayerControls_MenuBack = m_PlayerControls.FindAction("MenuBack", throwIfNotFound: true);
        m_PlayerControls_MenuAccept = m_PlayerControls.FindAction("MenuAccept", throwIfNotFound: true);
        m_PlayerControls_MenuNavigationUp = m_PlayerControls.FindAction("MenuNavigationUp", throwIfNotFound: true);
        m_PlayerControls_MenuNavigationDown = m_PlayerControls.FindAction("MenuNavigationDown", throwIfNotFound: true);
        m_PlayerControls_SkipDialogue = m_PlayerControls.FindAction("SkipDialogue", throwIfNotFound: true);
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
    private readonly InputAction m_PlayerControls_Pause;
    private readonly InputAction m_PlayerControls_MenuBack;
    private readonly InputAction m_PlayerControls_MenuAccept;
    private readonly InputAction m_PlayerControls_MenuNavigationUp;
    private readonly InputAction m_PlayerControls_MenuNavigationDown;
    private readonly InputAction m_PlayerControls_SkipDialogue;
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
        public InputAction @Pause => m_Wrapper.m_PlayerControls_Pause;
        public InputAction @MenuBack => m_Wrapper.m_PlayerControls_MenuBack;
        public InputAction @MenuAccept => m_Wrapper.m_PlayerControls_MenuAccept;
        public InputAction @MenuNavigationUp => m_Wrapper.m_PlayerControls_MenuNavigationUp;
        public InputAction @MenuNavigationDown => m_Wrapper.m_PlayerControls_MenuNavigationDown;
        public InputAction @SkipDialogue => m_Wrapper.m_PlayerControls_SkipDialogue;
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
                @Pause.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnPause;
                @MenuBack.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMenuBack;
                @MenuBack.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMenuBack;
                @MenuBack.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMenuBack;
                @MenuAccept.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMenuAccept;
                @MenuAccept.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMenuAccept;
                @MenuAccept.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMenuAccept;
                @MenuNavigationUp.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMenuNavigationUp;
                @MenuNavigationUp.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMenuNavigationUp;
                @MenuNavigationUp.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMenuNavigationUp;
                @MenuNavigationDown.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMenuNavigationDown;
                @MenuNavigationDown.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMenuNavigationDown;
                @MenuNavigationDown.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMenuNavigationDown;
                @SkipDialogue.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnSkipDialogue;
                @SkipDialogue.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnSkipDialogue;
                @SkipDialogue.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnSkipDialogue;
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
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
                @MenuBack.started += instance.OnMenuBack;
                @MenuBack.performed += instance.OnMenuBack;
                @MenuBack.canceled += instance.OnMenuBack;
                @MenuAccept.started += instance.OnMenuAccept;
                @MenuAccept.performed += instance.OnMenuAccept;
                @MenuAccept.canceled += instance.OnMenuAccept;
                @MenuNavigationUp.started += instance.OnMenuNavigationUp;
                @MenuNavigationUp.performed += instance.OnMenuNavigationUp;
                @MenuNavigationUp.canceled += instance.OnMenuNavigationUp;
                @MenuNavigationDown.started += instance.OnMenuNavigationDown;
                @MenuNavigationDown.performed += instance.OnMenuNavigationDown;
                @MenuNavigationDown.canceled += instance.OnMenuNavigationDown;
                @SkipDialogue.started += instance.OnSkipDialogue;
                @SkipDialogue.performed += instance.OnSkipDialogue;
                @SkipDialogue.canceled += instance.OnSkipDialogue;
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
        void OnPause(InputAction.CallbackContext context);
        void OnMenuBack(InputAction.CallbackContext context);
        void OnMenuAccept(InputAction.CallbackContext context);
        void OnMenuNavigationUp(InputAction.CallbackContext context);
        void OnMenuNavigationDown(InputAction.CallbackContext context);
        void OnSkipDialogue(InputAction.CallbackContext context);
    }
}
