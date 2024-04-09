using UnityEngine;

namespace SpaceBaboon.WeaponSystem
{
    public class ShockWave : Weapon
    {
        protected override Transform GetTarget()
        {
            return transform.parent;
        }
    }
}
