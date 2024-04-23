using System.Collections.Generic;
using UnityEngine;

namespace SpaceBaboon.FXSystem
{
    public enum EFXType
    {
        PlayerHit,
        EnemyHit,

    }

    public class FXManager : MonoBehaviour
    {

        [SerializeField] private List<FXEvent> m_fxEvents = new List<FXEvent>();

        private Dictionary<EFXType, AudioClip> m_dictionary = new Dictionary<EFXType, AudioClip>();

        [SerializeField] private GameObject m_audioSourcePrefab;
        private ObjectPool m_audioPool = new ObjectPool();

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

        private void Start()
        {
            m_audioPool.CreatePool(m_audioSourcePrefab);

            foreach (var item in m_fxEvents)
            {
                m_dictionary.Add(item.type, item.clip);
            }
        }

        public void PlayAudio(EFXType type)
        {
            Debug.Log("PLayAudio called");
            GameObject obj = m_audioPool.Spawn(transform.position);

            AudioSource audioSource = obj.GetComponent<AudioInstance>().AudioSource;
            audioSource.PlayOneShot(m_dictionary[type]);
            
            //m_audioPool.UnSpawn(obj);
        }

    }

    [System.Serializable]
    public struct FXEvent
    {
        public EFXType type;
        public AudioClip clip;
    }
}
