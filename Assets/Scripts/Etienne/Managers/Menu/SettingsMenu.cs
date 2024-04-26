using System;
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
        private Slider m_windowSizeSlider;
        private Button m_backButton;

        private const int DEFAULT_SIZE = 120;
        private const int WIDTH_RATIO = 16;
        private const int HEIGHT_RATIO = 9;
        


        public void Create(VisualElement visualElement, MenuManager manager)
        {
            m_root = visualElement;
            m_manager = manager;

            m_fullscreenToggle = m_root.Q<Toggle>("FullscreenToggle");
            m_fullscreenToggle.value = Screen.fullScreen;

            m_masterVolumeSlider = m_root.Q<Slider>("VolumeSlider");
            m_masterVolumeSlider.value = AudioListener.volume;

            m_windowSizeSlider = m_root.Q<Slider>("WindowSizeSlider");
            m_windowSizeSlider.value = DEFAULT_SIZE;

            m_backButton = m_root.Q<Button>("BackButton");


            Enable();
        }

        private void Enable()
        {
            m_fullscreenToggle.RegisterValueChangedCallback(OnFullscreenToggled);
            m_masterVolumeSlider.RegisterValueChangedCallback(OnVolumeChanged);
            m_windowSizeSlider.RegisterValueChangedCallback(OnWindowSizeChanged);
            //m_windowSizeSlider.RegisterCallback<PointerDownEvent>(Foo);
            m_backButton.clicked += BackToMainMenu;
        }

        private void Foo(PointerDownEvent evt)
        {
            Debug.Log("hello");
        }

        public void Disable()
        {
            m_fullscreenToggle.UnregisterValueChangedCallback(OnFullscreenToggled);
            m_masterVolumeSlider.UnregisterValueChangedCallback(OnVolumeChanged);
            m_windowSizeSlider.UnregisterValueChangedCallback(OnWindowSizeChanged);

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

        private void OnWindowSizeChanged(ChangeEvent<float> evt)
        {
            SetResolution((int)evt.newValue);
        }

        private void SetResolution(int size)
        {
            //Screen.SetResolution
            int width = size * WIDTH_RATIO;
            int height = size * HEIGHT_RATIO;

            Debug.Log(width + " x " + height);

            //Screen.SetResolution(width, height, FullScreenMode.Windowed);

            //TODO : SET FULLSCREEN TOGGLE TO FALSE
        }

        private void BackToMainMenu()
        {
            m_manager.OpenMainMenu(m_root);
        }
    }
}
