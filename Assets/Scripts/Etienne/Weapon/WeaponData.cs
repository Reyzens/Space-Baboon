using UnityEngine;

namespace SpaceBaboon.WeaponSystem
{
    [CreateAssetMenu(fileName = "NewWeaponData", menuName = "SpaceBaboon/ScriptableObjects/WeaponData")]
    public class WeaponData : ScriptableObject
    {
        public int maxLevel;
        public float maxRange;
        public float attackZone;
        public float attackSpeed;
        public string weaponName;
        public Sprite icon;
        public GameObject projectilePrefab;
    }
}
