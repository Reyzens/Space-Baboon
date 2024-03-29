using UnityEngine;

namespace SpaceBaboon
{
    [CreateAssetMenu(fileName = "New ResourceData", menuName = "SpaceBaboon/ScriptableObjects/ResourceData", order = 0)]
    public class ResourceData : ScriptableObject
    {
        // Collect data
        public float m_cooldownMax;

        //Resource
        public Sprite m_icon;
        public SpaceBaboon.InteractableResource.EResourceType m_resourceType;
        public int m_resourceAmount;
    }
}
