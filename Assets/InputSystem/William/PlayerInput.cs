//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/InputSystem/William/PlayerInput.inputactions
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

namespace SpaceBaboon
{
    public partial class @PlayerInput: IInputActionCollection2, IDisposable
    {
        public InputActionAsset asset { get; }
        public @PlayerInput()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInput"",
    ""maps"": [
        {
            ""name"": ""PlayerMovement"",
            ""id"": ""f5cbb5d7-b71f-418c-ba94-f79b6d2dd2e0"",
            ""actions"": [
                {
                    ""name"": ""PlayerDirection"",
                    ""type"": ""Value"",
                    ""id"": ""1c76bf9b-2d4a-4563-8250-e12af8265582"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""PlayerDash"",
                    ""type"": ""Value"",
                    ""id"": ""b9c77b2a-7edb-46fb-a683-f58503159798"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""87346404-753a-4826-8435-111e9a9f53c8"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayerDirection"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""a62f0d28-49c2-4710-ab88-dc42f235b221"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayerDirection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""9955dcef-e864-4871-b802-735b17fbc08b"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayerDirection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""a9ad558d-bc9c-40d9-a945-67b804bd0af8"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayerDirection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""4cb3d2fa-1c9b-48d2-a423-3bb47826b6b5"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayerDirection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""fce1e842-c3bf-46e1-947f-47d3e68dc1bd"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayerDash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""PlayerInteraction"",
            ""id"": ""5bdefbba-13bf-4e41-aaf6-fae2776bf019"",
            ""actions"": [
                {
                    ""name"": ""CollectResource"",
                    ""type"": ""Button"",
                    ""id"": ""710a26fd-ba19-47af-8a47-caa162017612"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""0e998e7e-e2b3-479f-9014-06187291d305"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CollectResource"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // PlayerMovement
            m_PlayerMovement = asset.FindActionMap("PlayerMovement", throwIfNotFound: true);
            m_PlayerMovement_PlayerDirection = m_PlayerMovement.FindAction("PlayerDirection", throwIfNotFound: true);
            m_PlayerMovement_PlayerDash = m_PlayerMovement.FindAction("PlayerDash", throwIfNotFound: true);
            // PlayerInteraction
            m_PlayerInteraction = asset.FindActionMap("PlayerInteraction", throwIfNotFound: true);
            m_PlayerInteraction_CollectResource = m_PlayerInteraction.FindAction("CollectResource", throwIfNotFound: true);
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

        // PlayerMovement
        private readonly InputActionMap m_PlayerMovement;
        private List<IPlayerMovementActions> m_PlayerMovementActionsCallbackInterfaces = new List<IPlayerMovementActions>();
        private readonly InputAction m_PlayerMovement_PlayerDirection;
        private readonly InputAction m_PlayerMovement_PlayerDash;
        public struct PlayerMovementActions
        {
            private @PlayerInput m_Wrapper;
            public PlayerMovementActions(@PlayerInput wrapper) { m_Wrapper = wrapper; }
            public InputAction @PlayerDirection => m_Wrapper.m_PlayerMovement_PlayerDirection;
            public InputAction @PlayerDash => m_Wrapper.m_PlayerMovement_PlayerDash;
            public InputActionMap Get() { return m_Wrapper.m_PlayerMovement; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(PlayerMovementActions set) { return set.Get(); }
            public void AddCallbacks(IPlayerMovementActions instance)
            {
                if (instance == null || m_Wrapper.m_PlayerMovementActionsCallbackInterfaces.Contains(instance)) return;
                m_Wrapper.m_PlayerMovementActionsCallbackInterfaces.Add(instance);
                @PlayerDirection.started += instance.OnPlayerDirection;
                @PlayerDirection.performed += instance.OnPlayerDirection;
                @PlayerDirection.canceled += instance.OnPlayerDirection;
                @PlayerDash.started += instance.OnPlayerDash;
                @PlayerDash.performed += instance.OnPlayerDash;
                @PlayerDash.canceled += instance.OnPlayerDash;
            }

            private void UnregisterCallbacks(IPlayerMovementActions instance)
            {
                @PlayerDirection.started -= instance.OnPlayerDirection;
                @PlayerDirection.performed -= instance.OnPlayerDirection;
                @PlayerDirection.canceled -= instance.OnPlayerDirection;
                @PlayerDash.started -= instance.OnPlayerDash;
                @PlayerDash.performed -= instance.OnPlayerDash;
                @PlayerDash.canceled -= instance.OnPlayerDash;
            }

            public void RemoveCallbacks(IPlayerMovementActions instance)
            {
                if (m_Wrapper.m_PlayerMovementActionsCallbackInterfaces.Remove(instance))
                    UnregisterCallbacks(instance);
            }

            public void SetCallbacks(IPlayerMovementActions instance)
            {
                foreach (var item in m_Wrapper.m_PlayerMovementActionsCallbackInterfaces)
                    UnregisterCallbacks(item);
                m_Wrapper.m_PlayerMovementActionsCallbackInterfaces.Clear();
                AddCallbacks(instance);
            }
        }
        public PlayerMovementActions @PlayerMovement => new PlayerMovementActions(this);

        // PlayerInteraction
        private readonly InputActionMap m_PlayerInteraction;
        private List<IPlayerInteractionActions> m_PlayerInteractionActionsCallbackInterfaces = new List<IPlayerInteractionActions>();
        private readonly InputAction m_PlayerInteraction_CollectResource;
        public struct PlayerInteractionActions
        {
            private @PlayerInput m_Wrapper;
            public PlayerInteractionActions(@PlayerInput wrapper) { m_Wrapper = wrapper; }
            public InputAction @CollectResource => m_Wrapper.m_PlayerInteraction_CollectResource;
            public InputActionMap Get() { return m_Wrapper.m_PlayerInteraction; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(PlayerInteractionActions set) { return set.Get(); }
            public void AddCallbacks(IPlayerInteractionActions instance)
            {
                if (instance == null || m_Wrapper.m_PlayerInteractionActionsCallbackInterfaces.Contains(instance)) return;
                m_Wrapper.m_PlayerInteractionActionsCallbackInterfaces.Add(instance);
                @CollectResource.started += instance.OnCollectResource;
                //@CollectResource.performed += instance.OnCollectResource;
                //@CollectResource.canceled += instance.OnCollectResource;
            }

            private void UnregisterCallbacks(IPlayerInteractionActions instance)
            {
                @CollectResource.started -= instance.OnCollectResource;
                //@CollectResource.performed -= instance.OnCollectResource;
                //@CollectResource.canceled -= instance.OnCollectResource;
            }

            public void RemoveCallbacks(IPlayerInteractionActions instance)
            {
                if (m_Wrapper.m_PlayerInteractionActionsCallbackInterfaces.Remove(instance))
                    UnregisterCallbacks(instance);
            }

            public void SetCallbacks(IPlayerInteractionActions instance)
            {
                foreach (var item in m_Wrapper.m_PlayerInteractionActionsCallbackInterfaces)
                    UnregisterCallbacks(item);
                m_Wrapper.m_PlayerInteractionActionsCallbackInterfaces.Clear();
                AddCallbacks(instance);
            }
        }
        public PlayerInteractionActions @PlayerInteraction => new PlayerInteractionActions(this);
        public interface IPlayerMovementActions
        {
            void OnPlayerDirection(InputAction.CallbackContext context);
            void OnPlayerDash(InputAction.CallbackContext context);
        }
        public interface IPlayerInteractionActions
        {
            void OnCollectResource(InputAction.CallbackContext context);
        }
    }
}
