using UnityEngine;

namespace SpaceBaboon.EnemySystem
{
    public class MovingToStation : BossEnemyState
    {
        public override void OnEnter()
        {
            Debug.Log("BossEnemy entering state: MovingToStation\n");
        }

        public override void OnExit()
        {
            Debug.Log("BossEnemy exiting state: MovingToStation\n");
        }

        public override void OnUpdate()
        {
        }

        public override void OnFixedUpdate()
        {
            m_stateMachine.Move(m_stateMachine.CraftingStations[m_stateMachine.TargetedCraftingStation].transform.position);
        }

        public override bool CanEnter(IState currentState)
        {
            if(m_stateMachine.SpecialAttackReady)
            {
                return false;
            }

            return true;
        }

        public override bool CanExit()
        {
            if (m_stateMachine.InTargetedCraftingStationAttackRange)
            {
                return true;
            }
            if (m_stateMachine.PlayerInAggroRange && m_stateMachine.PlayerInTargetedCraftingStationRange)
            {
                return true;
            }

            return false;
        }
    }
}
