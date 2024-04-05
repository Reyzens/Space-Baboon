using SpaceBaboon.WeaponSystem;
using UnityEngine;

namespace SpaceBaboon
{
    public class DefaultWeapon : Weapon
    {
        // Start is called before the first frame update
        protected override Vector2 GetTarget()
        {
            //Aim for closest enemy
            for (int i = 0; i < m_weaponData.maxRange; i++)
            {
                var colliders = Physics2D.OverlapCircleAll(transform.position, i);

                foreach (var collider in colliders)
                {
                    if (collider.gameObject.tag == "Enemy")
                    {
                        Vector2 enemyPosition = collider.gameObject.transform.position;
                        Vector2 enemyDirection = enemyPosition - new Vector2(transform.position.x, transform.position.y);
                        return enemyDirection;
                    }
                }


            }

            //Didn't find an enemy
            return Vector2.up;
        }
    }
}
