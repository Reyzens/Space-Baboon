using System.Runtime.CompilerServices;
using UnityEngine;

namespace SpaceBaboon
{
    [CreateAssetMenu(fileName = "New ResourceData", menuName = "SpaceBaboon/ScriptableObjects/PlayerData", order = 0)]
    public class PlayerData : ScriptableObject
    {
        public int m_playerIndex;
        public string m_playerName;
        public int m_playerMaxHeatlh;
        public int m_playerMovespeed;
        public int m_playerDashCD;
    }


}
