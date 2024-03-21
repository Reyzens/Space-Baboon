using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceBaboon
{
    public class Player : MonoBehaviour
    {
        [SerializeField]
        private PlayerData m_playerData;
        private Rigidbody2D m_playerRigidbody;
        private Transform m_playerTransform;
        private Dictionary<SpaceBaboon.InteractableResource.EResourceType,int> m_collectibleInventory;
        private List<Weapon> m_equipedWeapon;
        private List<Weapon> m_blockedWeapon;

      
        [SerializeField]
        private bool m_DebugMode;
        private float m_horizontal;
        private float m_vertical;
        private int m_rotationlock = 0;
        private float m_playerDashTimer;
        private float m_playerDashCDTimer;
        private float m_playerCurrentMovespeed;
       

        void Start()
        {
            enabled = false;
            InputHandler.instance.m_MoveEvent += Move;
            InputHandler.instance.m_DashEvent += Dash;
            m_playerRigidbody = GetComponent<Rigidbody2D>();
            m_playerTransform = GetComponent<Transform>();
        }

        // Update is called once per frame
        void Update()
        {
          
        }

        private void Move(Vector2 values)
        {
            if (values == Vector2.zero)
            {
                enabled = false;
                m_playerRigidbody.velocity = new Vector2(0, 0);
                m_playerRigidbody.rotation = m_rotationlock;
                m_playerRigidbody.angularVelocity = m_rotationlock;
                return;
            }


            m_horizontal = values.x;
            m_vertical = values.y;

            m_playerRigidbody.velocity = new Vector2(m_horizontal, m_vertical) * m_playerData.Movespeed;
            m_playerRigidbody.freezeRotation = true;

            enabled = true;

        }

        private void Dash()
        {
            if(m_DebugMode)
            {
                Debug.Log("Dash");
            }
            
        }
    }
}