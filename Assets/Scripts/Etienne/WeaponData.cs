using UnityEngine;

namespace SpaceBaboon
{
    [CreateAssetMenu(fileName = "NewWeaponData", menuName = "SpaceBaboon/ScriptableObjects/WeaponData")]
    public class WeaponData : ScriptableObject
    {
        public int damage;
        public int maxLevel;
        public float maxRange;
        public float attackSpeed;
        public string weaponName;
        public Sprite icon;
        public GameObject projectilePrefab;
    }
}
