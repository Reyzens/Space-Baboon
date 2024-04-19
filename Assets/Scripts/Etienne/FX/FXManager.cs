using UnityEngine;

namespace SpaceBaboon.FXSystem
{
    public class FXManager : MonoBehaviour
    {

        private static FXManager instance;
        public static FXManager Instance
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


    }
}
