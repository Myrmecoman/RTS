// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/Inputs/Controls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Controls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Controls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controls"",
    ""maps"": [
        {
            ""name"": ""TopDownControls"",
            ""id"": ""8ee366fe-8ed7-42ce-af2f-51a0f9fbb646"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""a1f851b5-73b6-47fb-bbd6-313fd2aa9e98"",
                    ""expectedControlType"": ""Dpad"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""HoldPosition"",
                    ""type"": ""Button"",
                    ""id"": ""af890c8d-f0d8-449a-8bcd-136f43b57422"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MoveCommand"",
                    ""type"": ""Button"",
                    ""id"": ""5e715d4e-f8a1-46e1-952c-0e73c5238ef2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Zoom"",
                    ""type"": ""Value"",
                    ""id"": ""8f2c1f22-07d7-4d64-8bd0-29abb28370bd"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Select"",
                    ""type"": ""Button"",
                    ""id"": ""5f35377c-72c5-4f66-8af1-2bbe85b5426d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""BoxSelect"",
                    ""type"": ""Button"",
                    ""id"": ""7d8508e1-0007-477c-8645-fccf4bde466d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""StackAction"",
                    ""type"": ""Button"",
                    ""id"": ""7e07e3b1-0a8a-41ed-a67b-d5df9af3f0f6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Arrows"",
                    ""id"": ""4b1242f3-09a6-49f1-8b16-e88ef5c8b09e"",
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
                    ""id"": ""4f1b1163-d5f9-48ba-a555-e680b6e8af85"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""e6e5830a-bca6-48ca-ab2b-389c3ab713d0"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""42ae12ec-9e0a-4810-8259-876ee1a63e74"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""6d628367-2d0b-4216-be30-27a8b4d82aaa"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""b51fd4ac-70cf-4547-9ae0-5adb9c695630"",
                    ""path"": ""<Mouse>/scroll"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Zoom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a8d7d8c6-9696-4679-8dde-85d310b63461"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4928bfb2-f54f-450d-9651-6afef81b59ae"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Hold"",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""BoxSelect"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4d608903-53db-4a10-9c6b-78092028fe0d"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""StackAction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1c6bffd9-3ed9-4157-b5db-512ab26cf599"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""MoveCommand"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3ff7ac75-8b40-4653-a47e-209b2e55040f"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""HoldPosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""PC"",
            ""bindingGroup"": ""PC"",
            ""devices"": [
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // TopDownControls
        m_TopDownControls = asset.FindActionMap("TopDownControls", throwIfNotFound: true);
        m_TopDownControls_Move = m_TopDownControls.FindAction("Move", throwIfNotFound: true);
        m_TopDownControls_HoldPosition = m_TopDownControls.FindAction("HoldPosition", throwIfNotFound: true);
        m_TopDownControls_MoveCommand = m_TopDownControls.FindAction("MoveCommand", throwIfNotFound: true);
        m_TopDownControls_Zoom = m_TopDownControls.FindAction("Zoom", throwIfNotFound: true);
        m_TopDownControls_Select = m_TopDownControls.FindAction("Select", throwIfNotFound: true);
        m_TopDownControls_BoxSelect = m_TopDownControls.FindAction("BoxSelect", throwIfNotFound: true);
        m_TopDownControls_StackAction = m_TopDownControls.FindAction("StackAction", throwIfNotFound: true);
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

    // TopDownControls
    private readonly InputActionMap m_TopDownControls;
    private ITopDownControlsActions m_TopDownControlsActionsCallbackInterface;
    private readonly InputAction m_TopDownControls_Move;
    private readonly InputAction m_TopDownControls_HoldPosition;
    private readonly InputAction m_TopDownControls_MoveCommand;
    private readonly InputAction m_TopDownControls_Zoom;
    private readonly InputAction m_TopDownControls_Select;
    private readonly InputAction m_TopDownControls_BoxSelect;
    private readonly InputAction m_TopDownControls_StackAction;
    public struct TopDownControlsActions
    {
        private @Controls m_Wrapper;
        public TopDownControlsActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_TopDownControls_Move;
        public InputAction @HoldPosition => m_Wrapper.m_TopDownControls_HoldPosition;
        public InputAction @MoveCommand => m_Wrapper.m_TopDownControls_MoveCommand;
        public InputAction @Zoom => m_Wrapper.m_TopDownControls_Zoom;
        public InputAction @Select => m_Wrapper.m_TopDownControls_Select;
        public InputAction @BoxSelect => m_Wrapper.m_TopDownControls_BoxSelect;
        public InputAction @StackAction => m_Wrapper.m_TopDownControls_StackAction;
        public InputActionMap Get() { return m_Wrapper.m_TopDownControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(TopDownControlsActions set) { return set.Get(); }
        public void SetCallbacks(ITopDownControlsActions instance)
        {
            if (m_Wrapper.m_TopDownControlsActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnMove;
                @HoldPosition.started -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnHoldPosition;
                @HoldPosition.performed -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnHoldPosition;
                @HoldPosition.canceled -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnHoldPosition;
                @MoveCommand.started -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnMoveCommand;
                @MoveCommand.performed -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnMoveCommand;
                @MoveCommand.canceled -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnMoveCommand;
                @Zoom.started -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnZoom;
                @Zoom.performed -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnZoom;
                @Zoom.canceled -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnZoom;
                @Select.started -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnSelect;
                @Select.performed -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnSelect;
                @Select.canceled -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnSelect;
                @BoxSelect.started -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnBoxSelect;
                @BoxSelect.performed -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnBoxSelect;
                @BoxSelect.canceled -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnBoxSelect;
                @StackAction.started -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnStackAction;
                @StackAction.performed -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnStackAction;
                @StackAction.canceled -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnStackAction;
            }
            m_Wrapper.m_TopDownControlsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @HoldPosition.started += instance.OnHoldPosition;
                @HoldPosition.performed += instance.OnHoldPosition;
                @HoldPosition.canceled += instance.OnHoldPosition;
                @MoveCommand.started += instance.OnMoveCommand;
                @MoveCommand.performed += instance.OnMoveCommand;
                @MoveCommand.canceled += instance.OnMoveCommand;
                @Zoom.started += instance.OnZoom;
                @Zoom.performed += instance.OnZoom;
                @Zoom.canceled += instance.OnZoom;
                @Select.started += instance.OnSelect;
                @Select.performed += instance.OnSelect;
                @Select.canceled += instance.OnSelect;
                @BoxSelect.started += instance.OnBoxSelect;
                @BoxSelect.performed += instance.OnBoxSelect;
                @BoxSelect.canceled += instance.OnBoxSelect;
                @StackAction.started += instance.OnStackAction;
                @StackAction.performed += instance.OnStackAction;
                @StackAction.canceled += instance.OnStackAction;
            }
        }
    }
    public TopDownControlsActions @TopDownControls => new TopDownControlsActions(this);
    private int m_PCSchemeIndex = -1;
    public InputControlScheme PCScheme
    {
        get
        {
            if (m_PCSchemeIndex == -1) m_PCSchemeIndex = asset.FindControlSchemeIndex("PC");
            return asset.controlSchemes[m_PCSchemeIndex];
        }
    }
    public interface ITopDownControlsActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnHoldPosition(InputAction.CallbackContext context);
        void OnMoveCommand(InputAction.CallbackContext context);
        void OnZoom(InputAction.CallbackContext context);
        void OnSelect(InputAction.CallbackContext context);
        void OnBoxSelect(InputAction.CallbackContext context);
        void OnStackAction(InputAction.CallbackContext context);
    }
}
