using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceBaboon
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance;

        public Transform m_dmgPopUpPrefab;
        public Player Player { get; set; }

        private EndGameScreen m_endGameScreen;

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

        public void SetPlayer(Player player)
        {
            Player = player;
        }

        public void StartGame()
        {
            SceneManager.LoadScene("SB_Build3");
            StartTimer();
            m_isPaused = false;
        }

        private void StartTimer()
        {
            GameTimer = 0.0f;
            m_isCountingTime = true;
        }

        public void EndGame()
        {
            m_endGameScreen.ActivateScreen();
            m_isPaused = true;
            m_isCountingTime = false;
        }

        public void SetEndGameScreenScript(EndGameScreen script)
        {
            m_endGameScreen = script;
        }

        //#region Getters
        //public Player GetPlayerRef()
        //{
        //    return player;
        //}
        //#endregion
        //#region Setters
        //public void SetPlayer(Player playerRef)
        //{
        //    player = playerRef;
        //    Debug.Log(player);
        //}
        //#endregion
    }
}
