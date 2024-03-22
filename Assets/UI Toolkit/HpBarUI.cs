
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace SpaceBaboon.UI_Toolkit
{
    public class HpBarUI : MonoBehaviour
    {
        public Player playerRef;
        private void OnEnable()
        {
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;

            ProgressBar hpBar = root.Q<ProgressBar>("HpBar");
        }
    }
}
