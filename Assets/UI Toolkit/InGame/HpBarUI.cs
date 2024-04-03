
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace SpaceBaboon.UI_Toolkit
{
    public class HpBarUI : MonoBehaviour
    {
        private GameObject playerGameObject;
        private Player playerRef;
        private ProgressBar hpBar;
        private VisualElement root;
        private void OnEnable()
        {
            playerGameObject = GameObject.FindGameObjectWithTag("Player");
            playerRef = playerGameObject.GetComponent<Player>();
            root = GetComponent<UIDocument>().rootVisualElement;
            hpBar = root.Q<ProgressBar>("HpBar");
        }

        private void Update()
        {
            //hpBar.value = playerRef.m_currentHealth;
            hpBar.value = playerRef.GetCurrentHealth();
        }
    }
}
