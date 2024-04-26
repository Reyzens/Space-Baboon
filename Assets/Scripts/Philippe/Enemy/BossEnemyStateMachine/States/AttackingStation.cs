using UnityEngine;
//using static UnityEditor.PlayerSettings;

namespace SpaceBaboon.EnemySystem
{
    public class AttackingStation : BossEnemyState
    {
        private float m_craftingStationAttackTimer;

        public override void OnEnter()
        {
            Debug.Log("BossEnemy entering state: AttackingStation\n");

            m_craftingStationAttackTimer = m_stateMachine.UniqueData.craftingStationAttackDelay;
        }

        public override void OnExit()
        {
            Debug.Log("BossEnemy exiting state: AttackingStation\n");
        }

        public override void OnUpdate()
        {
            m_craftingStationAttackTimer -= Time.deltaTime;

            if (m_craftingStationAttackTimer < 0)
            {
                Debug.Log("Crafting station attacked");
                m_stateMachine.TemporaryCraftingStationAttack(m_stateMachine.CraftingStations[m_stateMachine.TargetedCraftingStation].transform.position);
                m_craftingStationAttackTimer = m_stateMachine.UniqueData.craftingStationAttackDelay;
            }
        }

        public override void OnFixedUpdate()
        {
            
        }

        public override bool CanEnter(IState currentState)
        {
            if (currentState is MovingToStation)
            {
                if (m_stateMachine.InTargetedCraftingStationAttackRange)
                {
                    return true;
                }
            }
            
            return false;
        }

        public override bool CanExit()
        {
            if (m_stateMachine.PlayerInAggroRange && m_stateMachine.PlayerInTargetedCraftingStationRange)
            {
                return true;
            }

            return false;
        }
    }
}
