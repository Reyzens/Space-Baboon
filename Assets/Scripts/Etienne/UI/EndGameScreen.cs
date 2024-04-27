using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace SpaceBaboon
{
    public class EndGameScreen : MonoBehaviour
    {
        private UIDocument m_uiDoc;

        private VisualElement m_root;
        private Button m_playAgainButton;

        private void Awake()
        {
            GameManager.Instance.SetEndGameScreenScript(this);
            
            m_uiDoc = GetComponent<UIDocument>();
            m_root = m_uiDoc.rootVisualElement;

            m_playAgainButton = m_root.Q<Button>("BackButton");

        }

        private void OnEnable()
        {
            m_playAgainButton.clicked += LaunchMainMenu;
        }

        private void OnDisable()
        {
            m_playAgainButton.clicked -= LaunchMainMenu;
        }

        private void LaunchMainMenu()
        {

            m_root.style.display = DisplayStyle.None;
            SceneManager.LoadScene("SB_MainMenu");
        }

        public void ActivateScreen()
        {
            Debug.Log("activatescreen");
            m_root.style.display = DisplayStyle.Flex;
        }
    }
}
