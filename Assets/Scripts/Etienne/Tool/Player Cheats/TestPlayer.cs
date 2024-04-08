using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace SpaceBaboon
{
    public class TestPlayer : MonoBehaviour
    {
        [SerializeField] private bool m_isInvincible = false;
        [SerializeField] private float m_speed = 5.0f;

        [SerializeField] private PlayerData m_playerData;



        // Update is called once per frame
        void Update()
        {
            //Debug.Log("invincible bool = " + m_isInvincible);
            //Debug.Log("speed value = " + m_speed);
        
        }

        public void SetInvincibility(bool value)
        {
            m_isInvincible = value;
            //Debug.Log("bool was changed");
        }

        public void SetSpeed(float value)
        {
            m_speed = value;
            //Debug.Log("speed was changed");
        }

        public PlayerData GetData()
        {
            return m_playerData;
        }
    }
}
