using SpaceBaboon.WeaponSystem;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceBaboon
{
    public class CraftingStation : MonoBehaviour
    {
        //Serializable variables
        [SerializeField] private Weapon m_linkedWeapon;
        [SerializeField] private float m_MaxHealth;
        [SerializeField] private List<ResourceDropPoint> m_resourceDropPoints = new List<ResourceDropPoint>();
        [SerializeField] private float m_levelScaling;
        [SerializeField] private bool m_DebugMode;
        [SerializeField] private float m_maxUpgradeCooldown;

        //Private variables
        private Transform m_position;
        private float m_currentHealth;
        private int m_currentStationLevel;

        //Serialized for test purpose
        [SerializeField] private List<SpaceBaboon.InteractableResource.EResourceType> m_resourceNeeded = new List<SpaceBaboon.InteractableResource.EResourceType>();
        [SerializeField] private List<SpaceBaboon.InteractableResource.EResourceType> m_currentResources = new List<SpaceBaboon.InteractableResource.EResourceType>();
        [SerializeField] private List<SpaceBaboon.InteractableResource.EResourceType> m_possibleResources = new List<SpaceBaboon.InteractableResource.EResourceType>();
        [SerializeField] private float m_currentUpgradeCD = 0.0f;
        [SerializeField] private bool m_isUpgrading = false;

        //Static variables
        //static Upgrade currentUpgrade;
        static List<CraftingStation> m_craftingStationsList = new List<CraftingStation>();



        // Start is called before the first frame update
        void Start()
        {
            m_currentStationLevel = 1;
            ResourceNeededAllocation();
        }

        // Update is called once per frame
        void Update()
        {
            if (m_isUpgrading)
            {
                m_currentUpgradeCD -= Time.deltaTime;

                if (m_currentUpgradeCD < 0.0f)
                {
                    ResetDropStation();
                }
            }
        }

        public bool AddResource(SpaceBaboon.InteractableResource.EResourceType resourceType)
        {

            //Check if the resource is needed
            if (m_resourceNeeded.Contains(resourceType))
            {
                m_currentResources.Add(resourceType);
                m_currentResources.Sort();
                m_resourceNeeded.Remove(resourceType);
                CheckIfUpgradable();

                if (m_DebugMode)
                {
                    Debug.Log("AddResource called on " + gameObject.name);
                    foreach (SpaceBaboon.InteractableResource.EResourceType resource in m_currentResources)
                    {
                        Debug.Log("For crafting station " + gameObject.name + " there is a " + resource);
                    }
                }

                return true;
            }
            return false;
        }

        private void ResetDropStation()
        {
            m_isUpgrading = false;
            ResetPossibleResourceList();
            ResourceNeededAllocation();
        }
        private void CheckIfUpgradable()
        {
            //Sort both list before comparing their values
            if (m_resourceNeeded.Count == 0)
            {
                if (m_DebugMode) { Debug.Log("CrafingStation " + gameObject.name + " is upgrading weapon"); }

                m_linkedWeapon.Upgrade();
                m_currentResources.Clear();
                m_isUpgrading = true;
                m_currentUpgradeCD = m_maxUpgradeCooldown;
                m_currentStationLevel++;
            }
        }

        private void ResetPossibleResourceList()
        {
            m_possibleResources.Clear();
            for (int i = 0; i != (int)SpaceBaboon.InteractableResource.EResourceType.Count; i++)
            {
                if (m_DebugMode) { Debug.Log("Added to m_possibleResource : " + (SpaceBaboon.InteractableResource.EResourceType)i); }
                m_possibleResources.Add((SpaceBaboon.InteractableResource.EResourceType)i);
            }
        }

        private void ResourceNeededAllocation()
        {
            //Clear resource needed before allocation
            m_resourceNeeded.Clear();

            //Reset possible resources list
            ResetPossibleResourceList();

            //Variables needed for while loop
            int amountOfResourceNeeded = m_currentStationLevel * (int)m_levelScaling;
            int currentResourceAllocation;
            int initialResourceIndex;
            //int dropPointIndex = 0;
            //int whileIterations = 0;

            //Choose first resource to add
            initialResourceIndex = Random.Range(0, m_possibleResources.Count);

            for (int i = 0; i < m_resourceDropPoints.Count; i++)
            {
                //Use modulo to have the correct resource index no matter the iteration order
                int resourceIndex = (initialResourceIndex + i) % (int)InteractableResource.EResourceType.Count;

                //Randomly select an amount to give to the drop point
                currentResourceAllocation = Random.Range(0, amountOfResourceNeeded);
                amountOfResourceNeeded -= currentResourceAllocation;

                //Add resource to the point
                m_resourceDropPoints[i].AllocateResource((InteractableResource.EResourceType)resourceIndex, currentResourceAllocation);

                //Check if the amount is 0 before adding it to neededResources
                if (currentResourceAllocation != 0) { m_resourceNeeded.Add((InteractableResource.EResourceType)resourceIndex); }
            }
        }
    }
}
