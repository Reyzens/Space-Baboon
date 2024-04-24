using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SpaceBaboon.EnemySystem
{
    public class BossEnemy : Enemy
    {
        private BossEnemyData m_uniqueData;

        private NavMeshAgent m_agent;

        [SerializeField] private List<GameObject> m_craftingStations = new List<GameObject>();
        [SerializeField] private int m_targetedCraftingStation;

       

        protected override void Start()
        {
            base.Start();

            m_uniqueData = m_characterData as BossEnemyData;
            
            m_agent = GetComponent<NavMeshAgent>();
            m_agent.updateRotation = false;
            m_agent.updateUpAxis = false;            

            m_craftingStations.Add(GameObject.Find("CraftingStationOne"));
            m_craftingStations.Add(GameObject.Find("CraftingStationOne (1)"));
            m_craftingStations.Add(GameObject.Find("CraftingStationOne (2)"));
            m_craftingStations.Add(GameObject.Find("CraftingStationOne (3)"));
            m_craftingStations.Add(GameObject.Find("CraftingStationOne (4)"));

            m_targetedCraftingStation = GetRandomCraftingStationIndex();
        }

        protected override void FixedUpdate()
        {
            if (!m_isActive)
                return;

            DebugDrawCircleRange(m_craftingStations[m_targetedCraftingStation].transform.position, 64, m_uniqueData.possibleAggroRange, Color.green);
            DebugDrawCircleRange(transform.position, 64, m_uniqueData.playerAggroRange, Color.red);
                        
            bool playerInRange = m_distanceToPlayer < m_uniqueData.playerAggroRange;
            
            //bool craftingStationInRange = DistanceToTargetedCraftingStation() < m_uniqueData.possibleAggroRange;
            
            bool playerToCraftingStationInRange = PlayerDistanceToTargetedCraftingStation() < m_uniqueData.possibleAggroRange;

            if (playerInRange && playerToCraftingStationInRange)
            {
                Move(m_player.transform.position);
            }
            else
            {
                Move(m_craftingStations[m_targetedCraftingStation].transform.position);
            }
        }

        protected override void Move(Vector2 value)
        {
            m_agent.SetDestination(value);
        }

        protected override void SlightPushFromObstructingObject(Collision2D collision)
        {
            // Overriding this method
        }

        private int GetRandomCraftingStationIndex()
        {
            return Random.Range(0, m_craftingStations.Count);
        }

        private float DistanceToTargetedCraftingStation()
        {
            return Vector3.Distance(transform.position, m_craftingStations[m_targetedCraftingStation].transform.position);
        }

        private float PlayerDistanceToTargetedCraftingStation()
        {
            return Vector3.Distance(m_player.transform.position, m_craftingStations[m_targetedCraftingStation].transform.position);
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



























//private void DebugDrawCircleRange(Vector3 origin, int segments, float radius, Color color)
//{
//    float angleStep = 360f / segments;
//    float angle = 0f;
//
//    for (int i = 0; i < segments; i++)
//    {
//        float x = origin.x + Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
//        float y = origin.y + Mathf.Cos(Mathf.Deg2Rad * angle) * radius; // Use origin.y for the vertical position
//
//        Vector3 startPoint = new Vector3(x, origin.y, y);
//        angle += angleStep;
//
//        x = origin.x + Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
//        y = origin.y + Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
//
//        Vector3 endPoint = new Vector3(x, origin.y, y);
//
//        Debug.DrawLine(startPoint, endPoint, color);
//    }
//}





