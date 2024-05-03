using UnityEngine;

namespace SpaceBaboon.WeaponSystem
{
    [CreateAssetMenu(fileName = "NewBossSpecialProjectileData", menuName = "SpaceBaboon/ScriptableObjects/BossSpecialProjectileData")]
    public class BossSpecialProjectileData : ProjectileData
    {
        [Header("BossSpecialProjectileUniqueStats")]        
        public AnimationCurve sizeScalingCurve;
        public float radiusSizeMultiplier;
        public float sizeScalingDuration;
    }
}
