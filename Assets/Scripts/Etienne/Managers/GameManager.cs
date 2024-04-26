using UnityEngine;

namespace SpaceBaboon
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance;

        public Transform m_dmgPopUpPrefab;
        public Player Player { get; private set; }
        public float WindowSizeScale { get; set; }

        public static GameManager Instance
        {
            get
            {
                if (instance != null) { return instance; }

                Debug.LogError("FXManager instance is null");
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
        }

        public void SetPlayer(Player player)
        {
            Player = player;
            Debug.Log("setting up player : " + Player);
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
