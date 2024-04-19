using UnityEngine;

namespace SpaceBaboon.EnemySystem
{
    public class BossEnemy : Enemy
    {
        private BossEnemyData m_uniqueData;

        protected override void Start()
        {
            base.Start();

            m_uniqueData = m_characterData as BossEnemyData;
        }
    }
}
