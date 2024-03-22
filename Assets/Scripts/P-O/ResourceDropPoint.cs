using UnityEngine;

namespace SpaceBaboon
{
    public class ResourceDropPoint : MonoBehaviour
    {
        //Serialized variables
        [SerializeField]
        private bool m_DebugMode = false;
        [SerializeField]
        private CraftingStation m_craftingStation;

        //Private variables
        private int m_resourceAmountNeeded;
        private SpaceBaboon.InteractableResource.EResourceType m_resourceTypeNeeded;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void CollectResource(Player playerRef)
        {
            if (m_DebugMode) { Debug.Log("Player activated CollectResource on station"); }

            if (playerRef.DropResource(m_resourceTypeNeeded, m_resourceAmountNeeded))
            {
                if (m_DebugMode) { Debug.Log("Calling AddResource on " + m_craftingStation.gameObject.name); }
                m_craftingStation.AddResource(m_resourceTypeNeeded);
            }
        }

        public void AllocateResource(SpaceBaboon.InteractableResource.EResourceType resourceType, int resourceAmount)
        {
            if (m_DebugMode) { Debug.Log("To " + gameObject.name + " was allocated " + resourceAmount + " " + resourceType); }
            m_resourceTypeNeeded = resourceType;
            m_resourceAmountNeeded = resourceAmount;
        }
    }
}
