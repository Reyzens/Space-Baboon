using SpaceBaboon.EnemySystem;
using SpaceBaboon.TutorialSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceBaboon
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance;

        public Transform m_dmgPopUpPrefab;
        public Player Player { get; set; }
        public EnemySpawner EnemySpawner { get; set; }

        private EndGameScreen m_endGameScreen;

        private TutorialPopUpWindow m_tutorialWindow;

        public float WindowSizeScale { get; set; } = 1.0f;

        public float GameTimer { get; private set; } = 0.0f;

        private bool m_isCountingTime = false;
        private bool m_isPaused = false;

        public static GameManager Instance
        {
            get
            {
                if (instance != null) { return instance; }

                Debug.LogError("GameManager instance is null");
                return null;
            }
        }

        #region Unity
        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this);
                return;
            }
            instance = this;
            DontDestroyOnLoad(this);
        }

        private void Update()
        {
            if (m_isPaused)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
            if (!m_isPaused && m_isCountingTime)
            {
                GameTimer += Time.deltaTime;
            }
        }

        #endregion

        public void StartGame()
        {
            SceneManager.LoadScene("SB_Build3");
            StartTimer();
            //m_isPaused = false;
            PauseGame(false);
        }

        private void StartTimer()
        {
            GameTimer = 0.0f;
            m_isCountingTime = true;
        }

        public void PauseGame(bool value)
        {
            m_isPaused = value;
        }


        public void EndGame()
        {
            m_endGameScreen.ActivateScreen();
            //m_isPaused = true;
            PauseGame(true);
            m_isCountingTime = false;
        }

        public void DisplayTutorialWindow(ETutorialType type, Vector3 position)
        {
            //PauseGame(true);
            m_tutorialWindow.Display(type, position);
        }



        #region Setters

        public void SetPlayer(Player player)
        {
            Player = player;
        }

        public void SetEnemySpawner(EnemySpawner enemySpawner)
        {
            EnemySpawner = enemySpawner;
        }

        public void SetEndGameScreenScript(EndGameScreen script)
        {
            m_endGameScreen = script;
        }

        public void SetTutorialWindow(TutorialPopUpWindow window)
        {
            m_tutorialWindow = window;
        }

        #endregion
    }
}
