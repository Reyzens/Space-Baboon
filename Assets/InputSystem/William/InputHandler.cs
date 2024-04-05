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
        public delegate void DashEvent();
        

        public PlayerInput m_Input;
        public MoveEvent m_MoveEvent;
        public DashEvent m_DashStartEvent;
        //public DashEvent m_DashEndEvent;
        
        

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

        public void OnPlayerDashStart(InputAction.CallbackContext context)
        {
            m_DashStartEvent?.Invoke();
        }

        //public void OnPlayerDashEnd(InputAction.CallbackContext context)
        //{
        //    m_DashEndEvent?.Invoke();
        //}
    }
}