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
        private Label m_scoreAmount;
        private Button m_playAgainButton;

        private void Awake()
        {
            GameManager.Instance.SetEndGameScreenScript(this);
            
            m_uiDoc = GetComponent<UIDocument>();
            VisualElement visualElement = m_uiDoc.rootVisualElement;

            m_root = visualElement.Q<VisualElement>("EndGameScreen");
            m_scoreAmount = visualElement.Q<Label>("ScoreAmount");
            m_playAgainButton = visualElement.Q<Button>("BackButton");

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
            m_root.style.display = DisplayStyle.Flex;
            int score = (int)GameManager.Instance.GameTimer;
            m_scoreAmount.text = score.ToString();
        }
    }
}
