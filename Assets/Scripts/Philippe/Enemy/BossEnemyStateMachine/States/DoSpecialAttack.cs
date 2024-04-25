using UnityEngine;

namespace SpaceBaboon.EnemySystem
{
    public class DoSpecialAttack : BossEnemyState
    {
        private float m_chargeSpecialAttackTimer;
        private bool m_specialAttackDone;

        public override void OnEnter()
        {
            Debug.Log("BossEnemy entering state: DoSpecialAttack\n");
            m_stateMachine.Agent.isStopped = true;
            m_specialAttackDone = false;
            m_chargeSpecialAttackTimer = m_stateMachine.UniqueData.specialAttackChargeDelay;
        }

        public override void OnExit()
        {
            m_stateMachine.Agent.isStopped = false;
            Debug.Log("BossEnemy exiting state: DoSpecialAttack\n");
        }

        public override void OnUpdate()
        {
            m_chargeSpecialAttackTimer -= Time.deltaTime;

            if (m_chargeSpecialAttackTimer < 0)
            {
                ExecuteSpecialAttack();
                m_specialAttackDone = true;
            }
        }

        public override void OnFixedUpdate()
        {
           
        }

        public override bool CanEnter(IState currentState)
        {
            if (currentState is ChasingPlayer && m_stateMachine.SpecialAttackReady)
            {
                return true;
            }

            return false;
        }

        public override bool CanExit()
        {
            if (m_specialAttackDone) 
            {
                return true;
            }

            return false;
        }

        private void ExecuteSpecialAttack()
        {
            Debug.Log("!!!Special attack launched!!!");
        }
    }
}

