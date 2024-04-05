using UnityEngine;

namespace SpaceBaboon
{
    [CreateAssetMenu(fileName = "New ResourceData", menuName = "SpaceBaboon/ScriptableObjects/PlayerData", order = 0)]
    public class PlayerData : CharacterData
    {
        [Header("PlayerUniqueStats")]
        public float defaultDashCD;
        public float defaultDashDistance;
        public float defaultDashSpeed;
        public int defaultDashStatck;
    }


}
