using SpaceBaboon.FXSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceBaboon
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance;
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
    }
}
