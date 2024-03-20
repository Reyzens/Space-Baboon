using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static SpaceBaboon.PlayerInput;

namespace SpaceBaboon
{
    public class InputHandler : MonoBehaviour, IPlayerMovementActions
    {
        public static InputHandler instance;

        public delegate void MoveEvent(Vector2 values);

        private PlayerInput m_Input;
        public MoveEvent m_MoveEvent;
        

        void Awake()
        {
            if (instance != null)
            {
                instance.KillInstance();
                return;
            };
            instance ??= this;

            m_Input = new PlayerInput();
          
        }

        private void KillInstance()
        {
            Destroy(this);
        }

        void Start()
        {
            m_Input.PlayerMovement.SetCallbacks(this);
        }

        private void OnEnable()
        {
            m_Input.Enable();
        }

        private void OnDisable()
        {
            m_Input.Disable();
        }

        public void OnPlayerDirection(InputAction.CallbackContext context)
        {
            m_MoveEvent?.Invoke(context.ReadValue<Vector2>());
        }
    }
}