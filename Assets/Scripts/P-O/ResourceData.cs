using UnityEngine;

namespace SpaceBaboon
{
    [CreateAssetMenu(fileName = "New ResourceData", menuName = "SpaceBaboon/ResourceData", order = 0)]
    public class ResourceData : ScriptableObject
    {
        // Collect data
        public float cooldownMax;
    }
}
