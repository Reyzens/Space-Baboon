using SpaceBaboon.WeaponSystem;
using UnityEngine;

namespace SpaceBaboon
{
    public class GrenadeLauncher : PlayerWeapon
    {
        protected override Transform GetTarget()
        {
            for (int i = 0; i < m_weaponData.maxRange; i++)
            {
                var colliders = Physics2D.OverlapCircleAll(transform.position, i);

                foreach (var collider in colliders)
                {
                    if (collider.gameObject.tag == "Enemy")
                    {
                        return collider.gameObject.transform;
                    }
                }
            }

            //Didn't find an enemy
            return transform;
        }
    }
}
