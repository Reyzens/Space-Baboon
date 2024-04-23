using UnityEngine;

namespace SpaceBaboon.EnemySystem
{
    public class TestState : BossEnemyState
    {
        public override void OnEnter()
        {
            Debug.Log("BossEnemy entering state: TestState\n");
        }

        public override void OnExit()
        {
            Debug.Log("BossEnemy exiting state: TestState\n");
        }

        public override void OnUpdate()
        {
        }

        public override void OnFixedUpdate()
        {
            DebugDrawCircleRange(m_stateMachine.CraftingStations[m_stateMachine.TargetedCraftingStation].transform.position, 64, m_stateMachine.UniqueData.possibleAggroRange, Color.green);
            DebugDrawCircleRange(m_stateMachine.transform.position, 64, m_stateMachine.UniqueData.playerAggroRange, Color.red);

            bool playerInRange = m_stateMachine.DistanceToPlayer < m_stateMachine.UniqueData.playerAggroRange;

            bool craftingStationInRange = DistanceToTargetedCraftingStation() < m_stateMachine.UniqueData.possibleAggroRange;

            bool playerToCraftingStationInRange = PlayerDistanceToTargetedCraftingStation() < m_stateMachine.UniqueData.possibleAggroRange;

            if (playerInRange && playerToCraftingStationInRange)
            {
                Move(m_stateMachine.PlayerYo.transform.position);
            }
            else
            {
                Move(m_stateMachine.CraftingStations[m_stateMachine.TargetedCraftingStation].transform.position);
            }
        }

        private void Move(Vector2 value)
        {
            m_stateMachine.Agent.SetDestination(value);
        }

        public override bool CanEnter(IState currentState)
        {
            return true;
        }

        public override bool CanExit()
        {
            return false;
        }
        

        private float DistanceToTargetedCraftingStation()
        {
            return Vector3.Distance(m_stateMachine.transform.position, m_stateMachine.CraftingStations[m_stateMachine.TargetedCraftingStation].transform.position);
        }

        private float PlayerDistanceToTargetedCraftingStation()
        {
            return Vector3.Distance(m_stateMachine.PlayerYo.transform.position, m_stateMachine.CraftingStations[m_stateMachine.TargetedCraftingStation].transform.position);
        }

        private void DebugDrawCircleRange(Vector3 origin, int segments, float radius, Color color)
        {
            float angleStep = 360f / segments;
            float angle = 0f;

            for (int i = 0; i < segments; i++)
            {
                float x = origin.x + Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
                float y = origin.y + Mathf.Cos(Mathf.Deg2Rad * angle) * radius; // Use origin.y for the vertical position

                Vector3 startPoint = new Vector3(x, origin.y, y);
                angle += angleStep;

                x = origin.x + Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
                y = origin.y + Mathf.Cos(Mathf.Deg2Rad * angle) * radius;

                Vector3 endPoint = new Vector3(x, origin.y, y);

                Debug.DrawLine(startPoint, endPoint, color);
            }
        }


    }
}
