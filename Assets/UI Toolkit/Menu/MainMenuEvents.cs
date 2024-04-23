using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace SpaceBaboon.MenuSystem
{
    public class MainMenuEvents : MonoBehaviour
    {
        private UIDocument m_uiDoc;

        //------- Main Menu --------------------------
        private VisualElement m_mainMenu;

        private Button m_startButton;
        private Button m_settingsButton;
        private Button m_quitButton;

        //------- Settings Menu --------------------------
        private VisualElement m_settingsMenu;

        private Toggle m_fullscreenToggle;
        
        private Button m_settingsBackButton;





        private void Awake()
        {
            m_uiDoc = GetComponent<UIDocument>();
            VisualElement visualElement = m_uiDoc.rootVisualElement;

            //------- Main Menu --------------------------
            m_mainMenu = visualElement.Q<VisualElement>("MainMenuContainer");

            m_startButton = visualElement.Q<Button>("StartButton");
            m_settingsButton = visualElement.Q<Button>("SettingsButton");
            m_quitButton = visualElement.Q<Button>("QuitButton");

            //------- Settings Menu --------------------------
            m_settingsMenu = visualElement.Q<VisualElement>("SettingsMenuContainer");

            m_fullscreenToggle = visualElement.Q<Toggle>("FullscreenToggle");
            m_fullscreenToggle.value = Screen.fullScreen;
            m_settingsBackButton = visualElement.Q<Button>("SettingsBackButton");




        }

        private void OnEnable()
        {
            m_startButton.clicked += StartGame;
            m_settingsButton.clicked += OpenSettings;
            m_quitButton.clicked += QuitGame;

            m_fullscreenToggle.RegisterValueChangedCallback(OnFullscreenToggled);

            m_settingsBackButton.clicked += GoToMainMenu;
        }


        private void OnDisable()
        {
            m_startButton.clicked -= StartGame;
            m_settingsButton.clicked -= OpenSettings;
            m_quitButton.clicked -= QuitGame;

            m_fullscreenToggle.UnregisterValueChangedCallback(OnFullscreenToggled);

            m_settingsBackButton.clicked -= GoToMainMenu;

        }



        private void OnFullscreenToggled(ChangeEvent<bool> evt)
        {
            Screen.fullScreen = evt.newValue;
        }

        private void StartGame()
        {
            //Debug.Log("Start Game");

            SceneManager.LoadScene("SB_04-11_Sprint2");
        }

        private void OpenSettings()
        {
            //Debug.Log("Open Settings");

            m_mainMenu.style.display = DisplayStyle.None;
            m_settingsMenu.style.display = DisplayStyle.Flex;
        }

        private void QuitGame()
        {
            Application.Quit();
            //Debug.Log("Quit Game");
        }

        private void GoToMainMenu()
        {
            m_settingsMenu.style.display = DisplayStyle.None;
            m_mainMenu.style.display = DisplayStyle.Flex;
        }



    }
}
