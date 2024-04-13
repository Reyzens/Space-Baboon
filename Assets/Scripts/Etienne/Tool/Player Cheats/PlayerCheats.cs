using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace SpaceBaboon
{
    public class PlayerCheats : MonoBehaviour
    {
        [SerializeField] private Player m_player;

        private UIDocument m_uiDoc;

        private bool m_displayBool;
        private Button m_displayButton;
        private VisualElement m_elementsToHide;

        private Toggle m_invincibilityToggle;
        private Button m_maxHealthButton;
        private Slider m_speedSlider;
        private Slider m_dashSpeedSlider;
        private Toggle m_meleeToggle;
        private Toggle m_gunToggle;
        private Toggle m_grenadeLauncherToggle;


        private void Awake()
        {
            m_uiDoc = GetComponent<UIDocument>();
            VisualElement visualElement = m_uiDoc.rootVisualElement;

            m_displayButton = visualElement.Q<Button>("DisplayButton");
            m_elementsToHide = visualElement.Q<VisualElement>("ElementsToHide");
            m_elementsToHide.style.visibility = Visibility.Hidden;
            //m_elementsToHide.style.display = DisplayStyle.None;



            m_invincibilityToggle = visualElement.Q<Toggle>("InvincibilityToggle");
            m_maxHealthButton = visualElement.Q<Button>("MaxHealthButton");
            m_speedSlider = visualElement.Q<Slider>("SpeedSlider");
            m_dashSpeedSlider = visualElement.Q<Slider>("DashSpeedSlider");
            m_meleeToggle = visualElement.Q<Toggle>("MeleeToggle");
            m_gunToggle = visualElement.Q<Toggle>("GunToggle");
            m_grenadeLauncherToggle = visualElement.Q<Toggle>("GrenadeLauncherToggle");

        }

        private void OnEnable()
        {
            m_displayButton.clicked += OnDisplayButtonClicked;


            m_invincibilityToggle.RegisterValueChangedCallback(OnInvincibilityToggled);
            m_maxHealthButton.clicked += OnMaxHealthButtonClicked;
            m_speedSlider.RegisterValueChangedCallback(OnSpeedChanged);
            m_dashSpeedSlider.RegisterValueChangedCallback(OnDashSpeedChanged);
            m_meleeToggle.RegisterValueChangedCallback(OnMeleeToggled);
            m_gunToggle.RegisterValueChangedCallback(OnGunToggled);
            m_grenadeLauncherToggle.RegisterValueChangedCallback(OnGrenadeLauncherToggled);
        }

        private void OnDisplayButtonClicked()
        {
            m_displayBool = !m_displayBool;

            if (m_displayBool)
            {
                m_elementsToHide.style.visibility = Visibility.Visible;
            }
            else
            {
                m_elementsToHide.style.visibility = Visibility.Hidden;
            }
        }

        private void OnGrenadeLauncherToggled(ChangeEvent<bool> evt)
        {
            throw new NotImplementedException();
        }

        private void OnGunToggled(ChangeEvent<bool> evt)
        {
            throw new NotImplementedException();
        }

        private void OnMeleeToggled(ChangeEvent<bool> evt)
        {
            throw new NotImplementedException();
        }

        private void OnDashSpeedChanged(ChangeEvent<float> evt)
        {
            throw new NotImplementedException();
        }

        private void OnSpeedChanged(ChangeEvent<float> evt)
        {
            m_player.SetSpeedWithMultiplier(evt.newValue);
        }

        private void OnMaxHealthButtonClicked()
        {
            m_player.SetCurrentHealthToMax();
        }

        private void OnInvincibilityToggled(ChangeEvent<bool> evt)
        {
            m_player.SetIsInvincible(evt.newValue);
        }
    }
}
