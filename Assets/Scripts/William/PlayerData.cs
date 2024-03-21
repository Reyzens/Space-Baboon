using System.Runtime.CompilerServices;
using UnityEngine;

namespace SpaceBaboon
{
    [CreateAssetMenu(fileName = "New ResourceData", menuName = "SpaceBaboon/ScriptableObjects/PlayerData", order = 0)]
    public class PlayerData : ScriptableObject
    {
        public int Index;
        public string Name;
        public int MaxHeatlh;
        public int Movespeed;
        public int DashCD;
        public int DashDistance;
        public int DashSpeed;
        public int DashStatck;
    }


}
