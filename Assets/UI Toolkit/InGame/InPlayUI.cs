using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

namespace SpaceBaboon.UI_Toolkit
{
    public class InPlayUI : MonoBehaviour
    {
        [SerializeField] private Player playerRef;
        private Label ressourceone;
        private Label ressourcetwo;
        private Label ressourcetree;
        
        private VisualElement root;

        private void OnEnable()
        {
            root = GetComponent<UIDocument>().rootVisualElement;

            ressourceone = root.Q<Label>("ressourcevaluemetal");
            ressourcetwo = root.Q<Label>("ressourcevaluecrystal");
            ressourcetree = root.Q<Label>("ressourcevaluetechno");


        }

        private void Update()
        {
            //hpBar.value = playerRef.m_currentHealth;
            ressourceone.text = playerRef.GetRessourceOne().ToString();
            ressourcetwo.text = playerRef.GetRessourceTwo().ToString();
            ressourcetree.text = playerRef.GetRessourceTree().ToString();
            
            
        }
        
        
    }
}