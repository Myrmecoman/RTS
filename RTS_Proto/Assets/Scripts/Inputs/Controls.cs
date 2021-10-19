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
                    ""name"": ""HoldPosition"",
                    ""type"": ""Button"",
                    ""id"": ""af890c8d-f0d8-449a-8bcd-136f43b57422"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""f4587ae0-39d1-4771-af25-68b1f7bcc516"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Patrol"",
                    ""type"": ""Button"",
                    ""id"": ""818c8cc5-970f-481d-8790-ef394517edf4"",
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
                },
                {
                    ""name"": ""AddToGroup"",
                    ""type"": ""Button"",
                    ""id"": ""7338b123-9376-41e4-8309-74bbe2784e37"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RemoveFromGroup"",
                    ""type"": ""Button"",
                    ""id"": ""6bfa3bdf-0f32-4e4d-b5be-d39db1d28a0e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""AddAndRemoveFromOtherGroups"",
                    ""type"": ""Button"",
                    ""id"": ""03bd2721-80f5-4eb9-868f-933097a4b3f1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Group1"",
                    ""type"": ""Button"",
                    ""id"": ""5ba0610e-bacb-49c1-a819-10962916f0fe"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Group2"",
                    ""type"": ""Button"",
                    ""id"": ""db6c1f88-ce37-44cc-89a4-db3c1bdc4f93"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Group3"",
                    ""type"": ""Button"",
                    ""id"": ""eb66788e-6fda-4f98-957b-8124e404e471"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Group4"",
                    ""type"": ""Button"",
                    ""id"": ""3517e6e9-5f8c-4ac0-960f-8c4041d58472"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Group5"",
                    ""type"": ""Button"",
                    ""id"": ""14d8bf97-7673-49e7-af10-1acd5db515e2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Group6"",
                    ""type"": ""Button"",
                    ""id"": ""80305b3c-cd48-45c9-a753-c85d74b174bd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Group7"",
                    ""type"": ""Button"",
                    ""id"": ""0846d5b1-2bed-49d6-a0eb-cd49ba187431"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Group8"",
                    ""type"": ""Button"",
                    ""id"": ""27aac874-1243-47dc-9822-3c71bc7e74b6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Group9"",
                    ""type"": ""Button"",
                    ""id"": ""fcf96da0-4547-4041-80fd-3e900d3fac97"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Group0"",
                    ""type"": ""Button"",
                    ""id"": ""89b171ec-0d7e-401a-b68b-180c96049c66"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""AddOrReplaceCamera"",
                    ""type"": ""Button"",
                    ""id"": ""8ac01832-3d45-488b-80b8-57548400cd3f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CamBase1"",
                    ""type"": ""Button"",
                    ""id"": ""251ec99c-d057-4b92-b795-d243591b3e07"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CamBase2"",
                    ""type"": ""Button"",
                    ""id"": ""32ca2f82-eeeb-4261-b347-90cc558d5c67"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CamBase3"",
                    ""type"": ""Button"",
                    ""id"": ""92f3552f-4a7d-40fb-b56d-27e2e04d016f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CamBase4"",
                    ""type"": ""Button"",
                    ""id"": ""126f86c9-9100-4614-994b-b2fed8d3312f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CamBase5"",
                    ""type"": ""Button"",
                    ""id"": ""22c1be50-5562-4836-9d8f-d78c772d4232"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CamBase6"",
                    ""type"": ""Button"",
                    ""id"": ""547ca704-7a8a-47c3-9d76-e18ce7146ef8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CamBase7"",
                    ""type"": ""Button"",
                    ""id"": ""33cc00bb-76a9-4b79-b428-111ba3bdf9a0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CamBase8"",
                    ""type"": ""Button"",
                    ""id"": ""aca25dd7-8a23-430c-88a2-c69e86532bd3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CamBase9"",
                    ""type"": ""Button"",
                    ""id"": ""0b1b7ebb-e58a-46a1-9b25-5de982663c51"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CamBase10"",
                    ""type"": ""Button"",
                    ""id"": ""badd191e-ecc9-4d5b-8845-60c0fe347295"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
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
                    ""path"": ""<Keyboard>/leftShift"",
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
                },
                {
                    ""name"": """",
                    ""id"": ""62879f76-4dee-4992-8f32-1c9ac748698c"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""69ce3bc4-2495-4683-b772-bc1ee77c6f41"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Patrol"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""25fff336-d460-47ce-9dbd-a6d412e391b9"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""AddToGroup"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""66b9d496-d832-4062-b043-c429f8c2c9bc"",
                    ""path"": ""<Keyboard>/leftCtrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""RemoveFromGroup"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2abe8233-05c3-43d6-aa02-eb696ae696be"",
                    ""path"": ""<Keyboard>/leftAlt"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""AddAndRemoveFromOtherGroups"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ee12ebf5-74a5-49d2-8b99-ff9f1c76f4ca"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Group1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0ea585d5-82eb-48e8-8c49-93efee43fd50"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Group2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bd8adde1-3b82-4654-8d6d-d75459c1fa78"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Group3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0cc755ee-9e7a-4e6d-88fe-32341f660f5a"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Group4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""53538392-585b-4f5e-a08d-6f44cc44c247"",
                    ""path"": ""<Keyboard>/5"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Group5"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""58c4fba8-f8f7-457b-82d0-b3dc18f37ba3"",
                    ""path"": ""<Keyboard>/6"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Group6"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a49adf9b-cb45-40e5-92ad-68b7c0cc3e07"",
                    ""path"": ""<Keyboard>/7"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Group7"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7f91e0de-8c6b-4371-9084-1202fbbe60d8"",
                    ""path"": ""<Keyboard>/8"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Group8"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9856dabb-0590-45be-8729-80da9d0bc832"",
                    ""path"": ""<Keyboard>/9"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Group9"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""af7d1275-235c-4e15-b763-89a1e9a7e32e"",
                    ""path"": ""<Keyboard>/0"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Group0"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""52e63b2a-c28e-4b04-b518-c8d796114b55"",
                    ""path"": ""<Keyboard>/f1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""CamBase1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""25f63af4-5380-4741-a36c-c5e204e119c9"",
                    ""path"": ""<Keyboard>/f2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""CamBase2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""99acd8eb-272f-445a-8c14-7019518a508c"",
                    ""path"": ""<Keyboard>/f3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""CamBase3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0fa7067a-7232-412b-81ff-277ae95a9d1a"",
                    ""path"": ""<Keyboard>/f4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""CamBase4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0ee8216d-ec7d-4a6f-82bd-591d66757451"",
                    ""path"": ""<Keyboard>/f5"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""CamBase5"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3892de59-e0bc-48cf-8988-6d38b9fad2ce"",
                    ""path"": ""<Keyboard>/f6"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""CamBase6"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f320b0d5-3f63-4f9a-aa71-eecafba071f3"",
                    ""path"": ""<Keyboard>/f7"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""CamBase7"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a0801e7e-3bc8-42a3-a344-1f15dafcd3fe"",
                    ""path"": ""<Keyboard>/f8"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""CamBase8"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ccde0a0f-0843-4b29-9d39-6bea3664fc9e"",
                    ""path"": ""<Keyboard>/f9"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""CamBase9"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4b415e5d-ba8b-4660-9654-9be29b02ae6d"",
                    ""path"": ""<Keyboard>/f10"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""CamBase10"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7e5fc132-f6c4-4824-892e-183c866ff366"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""AddOrReplaceCamera"",
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
        m_TopDownControls_HoldPosition = m_TopDownControls.FindAction("HoldPosition", throwIfNotFound: true);
        m_TopDownControls_Attack = m_TopDownControls.FindAction("Attack", throwIfNotFound: true);
        m_TopDownControls_Patrol = m_TopDownControls.FindAction("Patrol", throwIfNotFound: true);
        m_TopDownControls_MoveCommand = m_TopDownControls.FindAction("MoveCommand", throwIfNotFound: true);
        m_TopDownControls_Zoom = m_TopDownControls.FindAction("Zoom", throwIfNotFound: true);
        m_TopDownControls_BoxSelect = m_TopDownControls.FindAction("BoxSelect", throwIfNotFound: true);
        m_TopDownControls_StackAction = m_TopDownControls.FindAction("StackAction", throwIfNotFound: true);
        m_TopDownControls_AddToGroup = m_TopDownControls.FindAction("AddToGroup", throwIfNotFound: true);
        m_TopDownControls_RemoveFromGroup = m_TopDownControls.FindAction("RemoveFromGroup", throwIfNotFound: true);
        m_TopDownControls_AddAndRemoveFromOtherGroups = m_TopDownControls.FindAction("AddAndRemoveFromOtherGroups", throwIfNotFound: true);
        m_TopDownControls_Group1 = m_TopDownControls.FindAction("Group1", throwIfNotFound: true);
        m_TopDownControls_Group2 = m_TopDownControls.FindAction("Group2", throwIfNotFound: true);
        m_TopDownControls_Group3 = m_TopDownControls.FindAction("Group3", throwIfNotFound: true);
        m_TopDownControls_Group4 = m_TopDownControls.FindAction("Group4", throwIfNotFound: true);
        m_TopDownControls_Group5 = m_TopDownControls.FindAction("Group5", throwIfNotFound: true);
        m_TopDownControls_Group6 = m_TopDownControls.FindAction("Group6", throwIfNotFound: true);
        m_TopDownControls_Group7 = m_TopDownControls.FindAction("Group7", throwIfNotFound: true);
        m_TopDownControls_Group8 = m_TopDownControls.FindAction("Group8", throwIfNotFound: true);
        m_TopDownControls_Group9 = m_TopDownControls.FindAction("Group9", throwIfNotFound: true);
        m_TopDownControls_Group0 = m_TopDownControls.FindAction("Group0", throwIfNotFound: true);
        m_TopDownControls_AddOrReplaceCamera = m_TopDownControls.FindAction("AddOrReplaceCamera", throwIfNotFound: true);
        m_TopDownControls_CamBase1 = m_TopDownControls.FindAction("CamBase1", throwIfNotFound: true);
        m_TopDownControls_CamBase2 = m_TopDownControls.FindAction("CamBase2", throwIfNotFound: true);
        m_TopDownControls_CamBase3 = m_TopDownControls.FindAction("CamBase3", throwIfNotFound: true);
        m_TopDownControls_CamBase4 = m_TopDownControls.FindAction("CamBase4", throwIfNotFound: true);
        m_TopDownControls_CamBase5 = m_TopDownControls.FindAction("CamBase5", throwIfNotFound: true);
        m_TopDownControls_CamBase6 = m_TopDownControls.FindAction("CamBase6", throwIfNotFound: true);
        m_TopDownControls_CamBase7 = m_TopDownControls.FindAction("CamBase7", throwIfNotFound: true);
        m_TopDownControls_CamBase8 = m_TopDownControls.FindAction("CamBase8", throwIfNotFound: true);
        m_TopDownControls_CamBase9 = m_TopDownControls.FindAction("CamBase9", throwIfNotFound: true);
        m_TopDownControls_CamBase10 = m_TopDownControls.FindAction("CamBase10", throwIfNotFound: true);
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
    private readonly InputAction m_TopDownControls_HoldPosition;
    private readonly InputAction m_TopDownControls_Attack;
    private readonly InputAction m_TopDownControls_Patrol;
    private readonly InputAction m_TopDownControls_MoveCommand;
    private readonly InputAction m_TopDownControls_Zoom;
    private readonly InputAction m_TopDownControls_BoxSelect;
    private readonly InputAction m_TopDownControls_StackAction;
    private readonly InputAction m_TopDownControls_AddToGroup;
    private readonly InputAction m_TopDownControls_RemoveFromGroup;
    private readonly InputAction m_TopDownControls_AddAndRemoveFromOtherGroups;
    private readonly InputAction m_TopDownControls_Group1;
    private readonly InputAction m_TopDownControls_Group2;
    private readonly InputAction m_TopDownControls_Group3;
    private readonly InputAction m_TopDownControls_Group4;
    private readonly InputAction m_TopDownControls_Group5;
    private readonly InputAction m_TopDownControls_Group6;
    private readonly InputAction m_TopDownControls_Group7;
    private readonly InputAction m_TopDownControls_Group8;
    private readonly InputAction m_TopDownControls_Group9;
    private readonly InputAction m_TopDownControls_Group0;
    private readonly InputAction m_TopDownControls_AddOrReplaceCamera;
    private readonly InputAction m_TopDownControls_CamBase1;
    private readonly InputAction m_TopDownControls_CamBase2;
    private readonly InputAction m_TopDownControls_CamBase3;
    private readonly InputAction m_TopDownControls_CamBase4;
    private readonly InputAction m_TopDownControls_CamBase5;
    private readonly InputAction m_TopDownControls_CamBase6;
    private readonly InputAction m_TopDownControls_CamBase7;
    private readonly InputAction m_TopDownControls_CamBase8;
    private readonly InputAction m_TopDownControls_CamBase9;
    private readonly InputAction m_TopDownControls_CamBase10;
    public struct TopDownControlsActions
    {
        private @Controls m_Wrapper;
        public TopDownControlsActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @HoldPosition => m_Wrapper.m_TopDownControls_HoldPosition;
        public InputAction @Attack => m_Wrapper.m_TopDownControls_Attack;
        public InputAction @Patrol => m_Wrapper.m_TopDownControls_Patrol;
        public InputAction @MoveCommand => m_Wrapper.m_TopDownControls_MoveCommand;
        public InputAction @Zoom => m_Wrapper.m_TopDownControls_Zoom;
        public InputAction @BoxSelect => m_Wrapper.m_TopDownControls_BoxSelect;
        public InputAction @StackAction => m_Wrapper.m_TopDownControls_StackAction;
        public InputAction @AddToGroup => m_Wrapper.m_TopDownControls_AddToGroup;
        public InputAction @RemoveFromGroup => m_Wrapper.m_TopDownControls_RemoveFromGroup;
        public InputAction @AddAndRemoveFromOtherGroups => m_Wrapper.m_TopDownControls_AddAndRemoveFromOtherGroups;
        public InputAction @Group1 => m_Wrapper.m_TopDownControls_Group1;
        public InputAction @Group2 => m_Wrapper.m_TopDownControls_Group2;
        public InputAction @Group3 => m_Wrapper.m_TopDownControls_Group3;
        public InputAction @Group4 => m_Wrapper.m_TopDownControls_Group4;
        public InputAction @Group5 => m_Wrapper.m_TopDownControls_Group5;
        public InputAction @Group6 => m_Wrapper.m_TopDownControls_Group6;
        public InputAction @Group7 => m_Wrapper.m_TopDownControls_Group7;
        public InputAction @Group8 => m_Wrapper.m_TopDownControls_Group8;
        public InputAction @Group9 => m_Wrapper.m_TopDownControls_Group9;
        public InputAction @Group0 => m_Wrapper.m_TopDownControls_Group0;
        public InputAction @AddOrReplaceCamera => m_Wrapper.m_TopDownControls_AddOrReplaceCamera;
        public InputAction @CamBase1 => m_Wrapper.m_TopDownControls_CamBase1;
        public InputAction @CamBase2 => m_Wrapper.m_TopDownControls_CamBase2;
        public InputAction @CamBase3 => m_Wrapper.m_TopDownControls_CamBase3;
        public InputAction @CamBase4 => m_Wrapper.m_TopDownControls_CamBase4;
        public InputAction @CamBase5 => m_Wrapper.m_TopDownControls_CamBase5;
        public InputAction @CamBase6 => m_Wrapper.m_TopDownControls_CamBase6;
        public InputAction @CamBase7 => m_Wrapper.m_TopDownControls_CamBase7;
        public InputAction @CamBase8 => m_Wrapper.m_TopDownControls_CamBase8;
        public InputAction @CamBase9 => m_Wrapper.m_TopDownControls_CamBase9;
        public InputAction @CamBase10 => m_Wrapper.m_TopDownControls_CamBase10;
        public InputActionMap Get() { return m_Wrapper.m_TopDownControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(TopDownControlsActions set) { return set.Get(); }
        public void SetCallbacks(ITopDownControlsActions instance)
        {
            if (m_Wrapper.m_TopDownControlsActionsCallbackInterface != null)
            {
                @HoldPosition.started -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnHoldPosition;
                @HoldPosition.performed -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnHoldPosition;
                @HoldPosition.canceled -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnHoldPosition;
                @Attack.started -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnAttack;
                @Attack.performed -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnAttack;
                @Attack.canceled -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnAttack;
                @Patrol.started -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnPatrol;
                @Patrol.performed -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnPatrol;
                @Patrol.canceled -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnPatrol;
                @MoveCommand.started -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnMoveCommand;
                @MoveCommand.performed -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnMoveCommand;
                @MoveCommand.canceled -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnMoveCommand;
                @Zoom.started -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnZoom;
                @Zoom.performed -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnZoom;
                @Zoom.canceled -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnZoom;
                @BoxSelect.started -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnBoxSelect;
                @BoxSelect.performed -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnBoxSelect;
                @BoxSelect.canceled -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnBoxSelect;
                @StackAction.started -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnStackAction;
                @StackAction.performed -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnStackAction;
                @StackAction.canceled -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnStackAction;
                @AddToGroup.started -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnAddToGroup;
                @AddToGroup.performed -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnAddToGroup;
                @AddToGroup.canceled -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnAddToGroup;
                @RemoveFromGroup.started -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnRemoveFromGroup;
                @RemoveFromGroup.performed -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnRemoveFromGroup;
                @RemoveFromGroup.canceled -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnRemoveFromGroup;
                @AddAndRemoveFromOtherGroups.started -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnAddAndRemoveFromOtherGroups;
                @AddAndRemoveFromOtherGroups.performed -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnAddAndRemoveFromOtherGroups;
                @AddAndRemoveFromOtherGroups.canceled -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnAddAndRemoveFromOtherGroups;
                @Group1.started -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnGroup1;
                @Group1.performed -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnGroup1;
                @Group1.canceled -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnGroup1;
                @Group2.started -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnGroup2;
                @Group2.performed -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnGroup2;
                @Group2.canceled -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnGroup2;
                @Group3.started -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnGroup3;
                @Group3.performed -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnGroup3;
                @Group3.canceled -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnGroup3;
                @Group4.started -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnGroup4;
                @Group4.performed -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnGroup4;
                @Group4.canceled -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnGroup4;
                @Group5.started -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnGroup5;
                @Group5.performed -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnGroup5;
                @Group5.canceled -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnGroup5;
                @Group6.started -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnGroup6;
                @Group6.performed -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnGroup6;
                @Group6.canceled -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnGroup6;
                @Group7.started -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnGroup7;
                @Group7.performed -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnGroup7;
                @Group7.canceled -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnGroup7;
                @Group8.started -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnGroup8;
                @Group8.performed -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnGroup8;
                @Group8.canceled -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnGroup8;
                @Group9.started -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnGroup9;
                @Group9.performed -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnGroup9;
                @Group9.canceled -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnGroup9;
                @Group0.started -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnGroup0;
                @Group0.performed -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnGroup0;
                @Group0.canceled -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnGroup0;
                @AddOrReplaceCamera.started -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnAddOrReplaceCamera;
                @AddOrReplaceCamera.performed -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnAddOrReplaceCamera;
                @AddOrReplaceCamera.canceled -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnAddOrReplaceCamera;
                @CamBase1.started -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnCamBase1;
                @CamBase1.performed -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnCamBase1;
                @CamBase1.canceled -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnCamBase1;
                @CamBase2.started -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnCamBase2;
                @CamBase2.performed -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnCamBase2;
                @CamBase2.canceled -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnCamBase2;
                @CamBase3.started -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnCamBase3;
                @CamBase3.performed -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnCamBase3;
                @CamBase3.canceled -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnCamBase3;
                @CamBase4.started -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnCamBase4;
                @CamBase4.performed -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnCamBase4;
                @CamBase4.canceled -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnCamBase4;
                @CamBase5.started -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnCamBase5;
                @CamBase5.performed -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnCamBase5;
                @CamBase5.canceled -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnCamBase5;
                @CamBase6.started -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnCamBase6;
                @CamBase6.performed -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnCamBase6;
                @CamBase6.canceled -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnCamBase6;
                @CamBase7.started -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnCamBase7;
                @CamBase7.performed -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnCamBase7;
                @CamBase7.canceled -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnCamBase7;
                @CamBase8.started -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnCamBase8;
                @CamBase8.performed -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnCamBase8;
                @CamBase8.canceled -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnCamBase8;
                @CamBase9.started -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnCamBase9;
                @CamBase9.performed -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnCamBase9;
                @CamBase9.canceled -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnCamBase9;
                @CamBase10.started -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnCamBase10;
                @CamBase10.performed -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnCamBase10;
                @CamBase10.canceled -= m_Wrapper.m_TopDownControlsActionsCallbackInterface.OnCamBase10;
            }
            m_Wrapper.m_TopDownControlsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @HoldPosition.started += instance.OnHoldPosition;
                @HoldPosition.performed += instance.OnHoldPosition;
                @HoldPosition.canceled += instance.OnHoldPosition;
                @Attack.started += instance.OnAttack;
                @Attack.performed += instance.OnAttack;
                @Attack.canceled += instance.OnAttack;
                @Patrol.started += instance.OnPatrol;
                @Patrol.performed += instance.OnPatrol;
                @Patrol.canceled += instance.OnPatrol;
                @MoveCommand.started += instance.OnMoveCommand;
                @MoveCommand.performed += instance.OnMoveCommand;
                @MoveCommand.canceled += instance.OnMoveCommand;
                @Zoom.started += instance.OnZoom;
                @Zoom.performed += instance.OnZoom;
                @Zoom.canceled += instance.OnZoom;
                @BoxSelect.started += instance.OnBoxSelect;
                @BoxSelect.performed += instance.OnBoxSelect;
                @BoxSelect.canceled += instance.OnBoxSelect;
                @StackAction.started += instance.OnStackAction;
                @StackAction.performed += instance.OnStackAction;
                @StackAction.canceled += instance.OnStackAction;
                @AddToGroup.started += instance.OnAddToGroup;
                @AddToGroup.performed += instance.OnAddToGroup;
                @AddToGroup.canceled += instance.OnAddToGroup;
                @RemoveFromGroup.started += instance.OnRemoveFromGroup;
                @RemoveFromGroup.performed += instance.OnRemoveFromGroup;
                @RemoveFromGroup.canceled += instance.OnRemoveFromGroup;
                @AddAndRemoveFromOtherGroups.started += instance.OnAddAndRemoveFromOtherGroups;
                @AddAndRemoveFromOtherGroups.performed += instance.OnAddAndRemoveFromOtherGroups;
                @AddAndRemoveFromOtherGroups.canceled += instance.OnAddAndRemoveFromOtherGroups;
                @Group1.started += instance.OnGroup1;
                @Group1.performed += instance.OnGroup1;
                @Group1.canceled += instance.OnGroup1;
                @Group2.started += instance.OnGroup2;
                @Group2.performed += instance.OnGroup2;
                @Group2.canceled += instance.OnGroup2;
                @Group3.started += instance.OnGroup3;
                @Group3.performed += instance.OnGroup3;
                @Group3.canceled += instance.OnGroup3;
                @Group4.started += instance.OnGroup4;
                @Group4.performed += instance.OnGroup4;
                @Group4.canceled += instance.OnGroup4;
                @Group5.started += instance.OnGroup5;
                @Group5.performed += instance.OnGroup5;
                @Group5.canceled += instance.OnGroup5;
                @Group6.started += instance.OnGroup6;
                @Group6.performed += instance.OnGroup6;
                @Group6.canceled += instance.OnGroup6;
                @Group7.started += instance.OnGroup7;
                @Group7.performed += instance.OnGroup7;
                @Group7.canceled += instance.OnGroup7;
                @Group8.started += instance.OnGroup8;
                @Group8.performed += instance.OnGroup8;
                @Group8.canceled += instance.OnGroup8;
                @Group9.started += instance.OnGroup9;
                @Group9.performed += instance.OnGroup9;
                @Group9.canceled += instance.OnGroup9;
                @Group0.started += instance.OnGroup0;
                @Group0.performed += instance.OnGroup0;
                @Group0.canceled += instance.OnGroup0;
                @AddOrReplaceCamera.started += instance.OnAddOrReplaceCamera;
                @AddOrReplaceCamera.performed += instance.OnAddOrReplaceCamera;
                @AddOrReplaceCamera.canceled += instance.OnAddOrReplaceCamera;
                @CamBase1.started += instance.OnCamBase1;
                @CamBase1.performed += instance.OnCamBase1;
                @CamBase1.canceled += instance.OnCamBase1;
                @CamBase2.started += instance.OnCamBase2;
                @CamBase2.performed += instance.OnCamBase2;
                @CamBase2.canceled += instance.OnCamBase2;
                @CamBase3.started += instance.OnCamBase3;
                @CamBase3.performed += instance.OnCamBase3;
                @CamBase3.canceled += instance.OnCamBase3;
                @CamBase4.started += instance.OnCamBase4;
                @CamBase4.performed += instance.OnCamBase4;
                @CamBase4.canceled += instance.OnCamBase4;
                @CamBase5.started += instance.OnCamBase5;
                @CamBase5.performed += instance.OnCamBase5;
                @CamBase5.canceled += instance.OnCamBase5;
                @CamBase6.started += instance.OnCamBase6;
                @CamBase6.performed += instance.OnCamBase6;
                @CamBase6.canceled += instance.OnCamBase6;
                @CamBase7.started += instance.OnCamBase7;
                @CamBase7.performed += instance.OnCamBase7;
                @CamBase7.canceled += instance.OnCamBase7;
                @CamBase8.started += instance.OnCamBase8;
                @CamBase8.performed += instance.OnCamBase8;
                @CamBase8.canceled += instance.OnCamBase8;
                @CamBase9.started += instance.OnCamBase9;
                @CamBase9.performed += instance.OnCamBase9;
                @CamBase9.canceled += instance.OnCamBase9;
                @CamBase10.started += instance.OnCamBase10;
                @CamBase10.performed += instance.OnCamBase10;
                @CamBase10.canceled += instance.OnCamBase10;
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
        void OnHoldPosition(InputAction.CallbackContext context);
        void OnAttack(InputAction.CallbackContext context);
        void OnPatrol(InputAction.CallbackContext context);
        void OnMoveCommand(InputAction.CallbackContext context);
        void OnZoom(InputAction.CallbackContext context);
        void OnBoxSelect(InputAction.CallbackContext context);
        void OnStackAction(InputAction.CallbackContext context);
        void OnAddToGroup(InputAction.CallbackContext context);
        void OnRemoveFromGroup(InputAction.CallbackContext context);
        void OnAddAndRemoveFromOtherGroups(InputAction.CallbackContext context);
        void OnGroup1(InputAction.CallbackContext context);
        void OnGroup2(InputAction.CallbackContext context);
        void OnGroup3(InputAction.CallbackContext context);
        void OnGroup4(InputAction.CallbackContext context);
        void OnGroup5(InputAction.CallbackContext context);
        void OnGroup6(InputAction.CallbackContext context);
        void OnGroup7(InputAction.CallbackContext context);
        void OnGroup8(InputAction.CallbackContext context);
        void OnGroup9(InputAction.CallbackContext context);
        void OnGroup0(InputAction.CallbackContext context);
        void OnAddOrReplaceCamera(InputAction.CallbackContext context);
        void OnCamBase1(InputAction.CallbackContext context);
        void OnCamBase2(InputAction.CallbackContext context);
        void OnCamBase3(InputAction.CallbackContext context);
        void OnCamBase4(InputAction.CallbackContext context);
        void OnCamBase5(InputAction.CallbackContext context);
        void OnCamBase6(InputAction.CallbackContext context);
        void OnCamBase7(InputAction.CallbackContext context);
        void OnCamBase8(InputAction.CallbackContext context);
        void OnCamBase9(InputAction.CallbackContext context);
        void OnCamBase10(InputAction.CallbackContext context);
    }
}
