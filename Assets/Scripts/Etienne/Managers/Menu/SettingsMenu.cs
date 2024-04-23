using UnityEngine;
using UnityEngine.UIElements;

namespace SpaceBaboon.MenuSystem
{
    public class SettingsMenu
    {
        private MenuManager m_manager;

        private VisualElement m_root;

        private Toggle m_fullscreenToggle;
        private Slider m_masterVolumeSlider;
        private Button m_backButton;


        public void Create(VisualElement visualElement, MenuManager manager)
        {
            m_root = visualElement;
            m_manager = manager;

            m_fullscreenToggle = m_root.Q<Toggle>("FullscreenToggle");
            m_fullscreenToggle.value = Screen.fullScreen;

            m_masterVolumeSlider = m_root.Q<Slider>("VolumeSlider");
            m_masterVolumeSlider.value = AudioListener.volume;

            m_backButton = m_root.Q<Button>("BackButton");


            Enable();
        }

        private void Enable()
        {
            m_fullscreenToggle.RegisterValueChangedCallback(OnFullscreenToggled);
            m_masterVolumeSlider.RegisterValueChangedCallback(OnVolumeChanged);
            m_backButton.clicked += BackToMainMenu;
        }

        public void Disable()
        {
            m_fullscreenToggle.UnregisterValueChangedCallback(OnFullscreenToggled);
            m_masterVolumeSlider.UnregisterValueChangedCallback(OnVolumeChanged);
            m_backButton.clicked -= BackToMainMenu;
        }

        private void OnFullscreenToggled(ChangeEvent<bool> evt)
        {
            Screen.fullScreen = evt.newValue;
        }

        private void OnVolumeChanged(ChangeEvent<float> evt)
        {
            AudioListener.volume = evt.newValue;
        }

        private void BackToMainMenu()
        {
            m_manager.OpenMainMenu(m_root);
        }
    }
}
