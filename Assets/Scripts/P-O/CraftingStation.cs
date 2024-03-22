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

        //Private variables
        private Transform m_position;
        private float currentHealth;
        private List<ResourceData> resourceNeeded = new List<ResourceData>();
        private List<InteractableResource> currentResources = new List<InteractableResource>();

        //Static variables
        //static Upgrade currentUpgrade;
        static List<CraftingStation> craftingStationsList = new List<CraftingStation>();



        // Start is called before the first frame update
        void Start()
        {

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
    }
}
