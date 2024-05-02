using SpaceBaboon.EnemySystem;
using UnityEngine;

namespace SpaceBaboon.WeaponSystem
{
    public class BossSpecialProjectile : Projectile
    {
        private BossSpecialProjectileData m_uniqueData;

        [SerializeField] private AnimationCurve m_scalingCurve;

        private EnemySpawner m_enemySpawner;
        private Vector2 m_targetSavedPos = Vector2.zero;
        private float m_distanceToPosThreshold = 1.0f;
        private bool m_isAtTargetPos = false;
        private int m_maxRadiusSizeExpansion = 10;
        private Vector3 m_originalScale;
        public float m_scalingDuration = 2.0f;

        private float m_scalingTimer = 0.0f;


        protected void Start()
        {
            m_enemySpawner = GameManager.Instance.EnemySpawner;
            m_originalScale = transform.localScale;
            m_uniqueData = m_projectileData as BossSpecialProjectileData;
        }

        protected override void Update()
        {
            base.Update();

            if (!m_isActive)
                return;

            if(m_isActive && !m_isAtTargetPos)
            {
                m_collider.enabled = false;
            }

            if (m_isAtTargetPos)
            {
                float scaleProgress = Mathf.Clamp01(m_scalingTimer / m_scalingDuration);
                float curveValue = m_scalingCurve.Evaluate(scaleProgress);

                float targetScaleFactor = curveValue * m_maxRadiusSizeExpansion;
                Vector3 targetScale = m_originalScale * targetScaleFactor;

                transform.localScale = targetScale;

                m_scalingTimer += Time.deltaTime;

                return;
            }

            Move();
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Do something to player");
                //Do something to player
            }
        }

        protected override void Move()
        {
            m_direction.x = m_targetSavedPos.x - transform.position.x;
            m_direction.y = m_targetSavedPos.y - transform.position.y;
            m_direction = m_direction.normalized;

            float distanceToPos = Vector2.Distance(transform.position, m_targetSavedPos);

            Debug.Log("distance to pos " + distanceToPos);

            if (distanceToPos > m_distanceToPosThreshold)
            {
                m_rb.AddForce(m_direction * m_projectileData.defaultAcceleration, ForceMode2D.Force);
                RegulateVelocity();
            }
            else
            {
                m_isAtTargetPos = true;
                m_collider.enabled = true;
                m_rb.bodyType = RigidbodyType2D.Kinematic;
                m_rb.constraints = RigidbodyConstraints2D.FreezeAll;
                m_rb.velocity = Vector2.zero;
            }          
        }        

        public override void Shoot(Transform direction, float maxRange, float attackZone, float damage, Transform playerPosition)
        {
            m_targetSavedPos = FindValidTargetPosition(direction);            
            Vector2 currentPosition = transform.position;
            m_direction = (m_targetSavedPos - currentPosition).normalized;
            m_damage = damage;
        }

        private Vector2Int FindValidTargetPosition(Transform targetPosition)
        {
            Vector3Int currentTargetTilePos = m_enemySpawner.m_obstacleTilemapRef.WorldToCell(targetPosition.position);            
            float closestDistance = Mathf.Infinity;
            Vector3Int closestObstaclePos = Vector3Int.zero;

            foreach (var obstacleTilePos in m_enemySpawner.m_obstacleTilemapRef.cellBounds.allPositionsWithin)
            {
                if (m_enemySpawner.m_obstacleTilemapRef.HasTile(obstacleTilePos))
                {
                    float distanceBetweenTargetAndObstacle = Vector3Int.Distance(obstacleTilePos, currentTargetTilePos);

                    if (distanceBetweenTargetAndObstacle <= m_maxRadiusSizeExpansion && distanceBetweenTargetAndObstacle < closestDistance)
                    {
                        closestDistance = distanceBetweenTargetAndObstacle;
                        closestObstaclePos = obstacleTilePos;
                    }
                }
            }

            if (closestObstaclePos == Vector3Int.zero)
            {
                return new Vector2Int((int)targetPosition.position.x, (int)targetPosition.position.y);
            }

            Vector3 closestObstacleVec3 = closestObstaclePos; 
            Vector3 directionToObstacle = (closestObstacleVec3 - currentTargetTilePos).normalized;
            Vector3 adjustedPosition = targetPosition.position + directionToObstacle * -m_maxRadiusSizeExpansion;

            return new Vector2Int((int)adjustedPosition.x, (int)adjustedPosition.y);
        }





    }
}
