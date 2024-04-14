using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace SpaceBaboon.MenuSystem
{
    public class MainMenuEvents : MonoBehaviour
    {
        private UIDocument m_uiDoc;

        private Button m_startButton;
        private Button m_settingsButton;
        private Button m_quitButton;

        private void Awake()
        {
            m_uiDoc = GetComponent<UIDocument>();
            VisualElement visualElement = m_uiDoc.rootVisualElement;

            m_startButton = visualElement.Q<Button>("StartButton");
            m_settingsButton = visualElement.Q<Button>("SettingsButton");
            m_quitButton = visualElement.Q<Button>("QuitButton");

        }

        private void OnEnable()
        {
            m_startButton.clicked += StartGame;
            m_settingsButton.clicked += OpenSettings;
            m_quitButton.clicked += QuitGame;
        }

        private void OnDisable()
        {
            m_startButton.clicked -= StartGame;
            m_settingsButton.clicked -= OpenSettings;
            m_quitButton.clicked -= QuitGame;
        }

        private void StartGame()
        {
            //Debug.Log("Start Game");

            SceneManager.LoadScene("SB_04-11_Sprint2");
        }

        private void OpenSettings()
        {
            Debug.Log("Open Settings");
        }

        private void QuitGame()
        {
            Application.Quit();
            //Debug.Log("Quit Game");
        }
    }
}
