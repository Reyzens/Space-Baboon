using System.Collections.Generic;
using UnityEngine;

namespace SpaceBaboon.FXSystem
{
    public enum EFXType
    {
        PlayerHit,
        PlayerDash,
        GrenadeLaunch,
        GrenadeExplode,
        Shockwave,
        LaserBeam,
        BananaSword,
        FlameThrower,
        EnemyMelee,
        EnemyShoot,
        EnemyExplode,
        Mining,
        MiningCompleted,
        CoinCollected,
        DroppingCoins,
        WeaponUpgrading,
        Count

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
            m_audioPool.SetPoolSize(10);
            m_audioPool.CreatePool(m_audioSourcePrefab);

            foreach (var item in m_fxEvents)
            {
                m_dictionary.Add(item.type, item.clip);
            }
        }

        public void PlayAudio(EFXType type)
        {
            //Debug.Log("PlayAudio called :  " + type);
            GameObject obj = m_audioPool.Spawn(transform.position);

            AudioSource audioSource = obj.GetComponent<AudioInstance>().AudioSource;
            //Debug.Log("audioSource " + audioSource);

            if (type == EFXType.GrenadeExplode || type == EFXType.WeaponUpgrading)
            {
                audioSource.PlayOneShot(m_dictionary[type], 0.2f);
                return;
            }
            audioSource.PlayOneShot(m_dictionary[type]);
            //Debug.Log("dictionary value: " + m_dictionary[type].name);
        }

    }

    [System.Serializable]
    public struct FXEvent
    {
        public EFXType type;
        public AudioClip clip;
    }
}
