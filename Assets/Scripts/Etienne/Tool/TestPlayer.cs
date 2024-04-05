using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceBaboon
{
    public class TestPlayer : MonoBehaviour
    {
        [SerializeField] private bool m_isInvincible = false;


        // Update is called once per frame
        void Update()
        {
            Debug.Log("invincible bool = " + m_isInvincible);
        
        }

        public void ToggleInvincibility()
        {
            m_isInvincible = !m_isInvincible;
            Debug.Log("bool was changed");
        }
    }
}
