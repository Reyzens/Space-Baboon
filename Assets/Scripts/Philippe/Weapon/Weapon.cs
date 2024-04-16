using UnityEngine;

namespace SpaceBaboon.WeaponSystem
{
    public class Weapon : BaseStats<MonoBehaviour>, IStatsEditable
    {
        public override ScriptableObject GetData()
        {
            throw new System.NotImplementedException();
        }

        protected virtual void Attack() { }
    }
}
