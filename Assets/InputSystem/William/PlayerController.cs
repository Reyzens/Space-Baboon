using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

namespace SpaceBaboon
{
    public class PlayerMovement : MonoBehaviour
    {
        private Rigidbody2D m_playerRigidbody;
        private Transform m_playerTransform;

        [SerializeField]
        private int m_playerSpeed = 1;
        private float m_horizontal;
        private float m_vertical;
        private int m_rotationlock = 0;


        // Start is called before the first frame update
        void Start()
        {
            enabled = false;
            InputHandler.instance.m_MoveEvent += Move;
            m_playerRigidbody = GetComponent<Rigidbody2D>();
            m_playerTransform = GetComponent<Transform>();
        }



        private void Move(Vector2 values)
        {
            if (values == Vector2.zero)
            {
                enabled = false;
                m_playerRigidbody.velocity = new Vector2 (0, 0);
                m_playerRigidbody.rotation = m_rotationlock;
                m_playerRigidbody.angularVelocity = m_rotationlock;
                return;
            }
            

            m_horizontal = values.x;
            m_vertical = values.y;

            m_playerRigidbody.velocity = new Vector2(m_horizontal, m_vertical) * m_playerSpeed;
            m_playerRigidbody.freezeRotation = true;

            enabled = true;

        }
    }
}
