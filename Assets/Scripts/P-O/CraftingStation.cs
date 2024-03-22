using System.Collections.Generic;
using UnityEngine;

namespace SpaceBaboon
{
    public class CraftingStation : MonoBehaviour
    {
        //Serializable variables
        //[SerializeField]
        //private Weapon m_linkedWeapon;
        [SerializeField]
        private float m_MaxHealth;
        [SerializeField]
        private List<ResourceDropPoint> resourceDropPoints = new List<ResourceDropPoint>();
        [SerializeField]
        private float levelScaling;
        [SerializeField]
        private bool m_DebugMode;

        //Private variables
        private Transform m_position;
        private float currentHealth;
        private List<SpaceBaboon.InteractableResource.EResourceType> resourceNeeded = new List<SpaceBaboon.InteractableResource.EResourceType>();
        private List<SpaceBaboon.InteractableResource.EResourceType> currentResources = new List<SpaceBaboon.InteractableResource.EResourceType>();

        //Static variables
        //static Upgrade currentUpgrade;
        static List<CraftingStation> craftingStationsList = new List<CraftingStation>();



        // Start is called before the first frame update
        void Start()
        {
            TemporaryInitialization();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void TemporaryInitialization()
        {
            foreach (ResourceDropPoint resourceDropPoint in resourceDropPoints)
            {
                resourceDropPoint.AllocateResource(SpaceBaboon.InteractableResource.EResourceType.Two, 3);
            }
        }

        public void AddResource(SpaceBaboon.InteractableResource.EResourceType resourceType)
        {
            currentResources.Add(resourceType);
            currentResources.Sort();
            if (m_DebugMode)
            {
                Debug.Log("AddResource called on " + gameObject.name);
                foreach (SpaceBaboon.InteractableResource.EResourceType resource in currentResources)
                {
                    Debug.Log("For crafting station " + gameObject.name + " there is a " + resource);
                }
            }

            CheckIfUpgradable();
        }

        private void CheckIfUpgradable()
        {

        }
    }
}
