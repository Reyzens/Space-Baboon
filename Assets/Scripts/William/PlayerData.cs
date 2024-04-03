using System.Runtime.CompilerServices;
using UnityEngine;

namespace SpaceBaboon
{
    [CreateAssetMenu(fileName = "New ResourceData", menuName = "SpaceBaboon/ScriptableObjects/PlayerData", order = 0)]
    public class PlayerData : CharacterData
    {
        [Header("PlayerUniqueStats")]
        public float DefaultDashCD;
        public float DefaultDashDistance;
        public float DefaultDashSpeed;
        public int DefaultDashStatck;
    }


}
