using UnityEngine;

namespace SpaceBaboon
{
    public class ResourceDropPoint : MonoBehaviour
    {
        //Serialized variables
        [SerializeField]
        private bool m_DebugMode = false;

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

            playerRef.DropResource(m_resourceTypeNeeded, m_resourceAmountNeeded);
        }

        public void AllocateResource(SpaceBaboon.InteractableResource.EResourceType resourceType, int resourceAmount)
        {
            m_resourceTypeNeeded = resourceType;
            m_resourceAmountNeeded = resourceAmount;
        }
    }
}
