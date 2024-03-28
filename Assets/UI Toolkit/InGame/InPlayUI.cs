using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace SpaceBaboon.UI_Toolkit
{
    public class InPlayUI : MonoBehaviour
    {
        [SerializeField] private Player playerRef;
        private Label ressource1;
        private VisualElement root;

        private void OnEnable()
        {
            root = GetComponent<UIDocument>().rootVisualElement;

            ressource1 = root.Q<Label>("ressourcevalue");


        }

        private void Update()
        {
            //hpBar.value = playerRef.m_currentHealth;
            ressource1.text = playerRef.GetRessourceOne().ToString();
        }
    }
}