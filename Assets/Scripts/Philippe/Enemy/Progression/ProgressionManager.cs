using SpaceBaboon.EnemySystem;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceBaboon
{
    public enum EGameMajorPhase
    {
        Tutorial,
        EarlyGame,
        MidGame,
        LateGame,
        Count
    }

    public enum EGameMinorPhase
    {
        SpawnWithRatio,
        SpawnBoss,
        SpawnGroup,
        SpawnOneElite,
        SpawnGroupElite,
        Count
    }

    [System.Serializable]
    public struct GameMinorPhase
    {
        public EGameMajorPhase gameMajorPhase;
        public float duration;
        public Event gameEvent;        
    }

    [System.Serializable]
    public struct Event
    {
        public EGameMinorPhase gameMinorPhase;
        public List<SpawningRatio> spawningRatios;
        //public Boss?
        //public GameObject for elite?
    }

    [System.Serializable]
    public struct SpawningRatio
    {
        public EEnemyTypes enemyType;
        [Range(0, 10)] public int spawnProbability;
    }

    public class ProgressionManager : MonoBehaviour
    {
        private EnemySpawner m_enemySpawner;
        private List<GameMinorPhase> m_minorPhases;

        private float m_totalGameTime;
        private float m_currentPlayTime;
        private float m_totalTimeAtThisPhaseStart;
        private int m_currentPhaseIndex = 0;

        private void Start()
        {        
            m_enemySpawner = GetComponent<EnemySpawner>();
            m_totalGameTime = CalculateTotalGameTimeRegisteredInMinorPhases();
            m_currentPhaseIndex = 0;
            m_totalTimeAtThisPhaseStart = 0;
        }

        private void Update()
        {
            m_currentPlayTime = Time.time;

            if (m_currentPlayTime >= m_totalTimeAtThisPhaseStart + m_minorPhases[m_currentPhaseIndex].duration)
            {
                NextMinorPhase();
            }
        }

        private void NextMinorPhase()
        {
            m_currentPhaseIndex++;

            if (m_currentPhaseIndex >= m_minorPhases.Count)
            {
                // when all phases are complet, maybe check the total elapsed time instead
                return;
            }

            foreach (var enemy in m_enemySpawner.m_pooledEnemies)
            {
                switch (enemy.enemyType)
                {
                    case EEnemyTypes.Melee:
                        m_enemySpawner.SetSpawnProbability(EEnemyTypes.Melee, m_minorPhases[m_currentPhaseIndex].gameEvent.spawningRatios[(int)EEnemyTypes.Melee].spawnProbability);
                        break;
                    case EEnemyTypes.Shooting:
                        m_enemySpawner.SetSpawnProbability(EEnemyTypes.Melee, m_minorPhases[m_currentPhaseIndex].gameEvent.spawningRatios[(int)EEnemyTypes.Shooting].spawnProbability);
                        break;
                    case EEnemyTypes.Kamikaze:
                        m_enemySpawner.SetSpawnProbability(EEnemyTypes.Melee, m_minorPhases[m_currentPhaseIndex].gameEvent.spawningRatios[(int)EEnemyTypes.Kamikaze].spawnProbability);
                        break;
                    case EEnemyTypes.Boss:
                        break;
                    case EEnemyTypes.Count:
                        break;
                    default:
                        break;
                }
            }

            m_totalTimeAtThisPhaseStart = m_currentPlayTime;
        }

        private float CalculateTotalGameTimeRegisteredInMinorPhases()
        {
            float totalGameTime = 0;
            foreach (var phase in m_minorPhases) { totalGameTime += phase.duration; }
            return totalGameTime;
        }
    }
}
