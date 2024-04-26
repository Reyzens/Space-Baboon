using SpaceBaboon.WeaponSystem;
using UnityEngine;

namespace SpaceBaboon.EnemySystem
{
    public class ChasingPlayer : BossEnemyState
    {
        private float m_basicAttackTimer;
        private int m_basicAttacksDone;

        public override void OnEnter()
        {
            Debug.Log("BossEnemy entering state: ChasingPlayer\n");

            m_stateMachine.SpecialAttackReady = false;
            m_stateMachine.SpecialAttackTimer = m_stateMachine.UniqueData.craftingStationAttackDelay;
            m_basicAttackTimer = m_stateMachine.UniqueData.basicAttackDelay;
            m_basicAttacksDone = 0;
        }

        public override void OnExit()
        {
            Debug.Log("BossEnemy exiting state: ChasingPlayer\n");
        }

        public override void OnUpdate()
        {
            if (!m_stateMachine.ControllerIsActive)
                return;

            if (m_basicAttacksDone == m_stateMachine.UniqueData.basicAttacksBeforeSpecial)
            {
                if (!m_stateMachine.SpecialAttackReady)
                {
                    m_stateMachine.SpecialAttackTimer -= Time.deltaTime;
                }

                if (m_stateMachine.SpecialAttackTimer < 0)
                {
                    m_stateMachine.SpecialAttackReady = true;
                }
                return;
            }

            m_basicAttackTimer -= Time.deltaTime;

            if (m_basicAttackTimer < 0)
            {
                ExecuteBasicAttack();
                m_basicAttackTimer = m_stateMachine.UniqueData.basicAttackDelay;
                m_basicAttacksDone++;
            }
        }

        public override void OnFixedUpdate()
        {
            if (!m_stateMachine.ControllerIsActive)
                return;

            m_stateMachine.Move(m_stateMachine.Player.transform.position);
        }

        public override bool CanEnter(IState currentState)
        {
            if (currentState is MovingToStation || currentState is AttackingStation || currentState is DoSpecialAttack)
            {
                if (m_stateMachine.PlayerInAggroRange && m_stateMachine.PlayerInTargetedCraftingStationRange)
                {
                    return true;
                }
            }

            return false;
        }

        public override bool CanExit()
        {
            if (m_stateMachine.SpecialAttackReady)
            {
                return true;
            }

            if (!m_stateMachine.PlayerInTargetedCraftingStationRange)
            {
                return true;
            }

            return false;
        }

        private void ExecuteBasicAttack()
        {                      
            m_stateMachine.SineGun.GetTarget(m_stateMachine.Player.transform);
        }
    }
}
