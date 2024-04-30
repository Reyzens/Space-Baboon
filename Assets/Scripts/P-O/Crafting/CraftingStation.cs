using SpaceBaboon.WeaponSystem;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceBaboon.Crafting
{
    public class CraftingStation : MonoBehaviour
    {
        //Serializable variables
        [SerializeField] private PlayerWeapon m_linkedWeapon;
        [SerializeField] private float m_maxHealth;
        [SerializeField] private List<ResourceDropPoint> m_resourceDropPoints = new List<ResourceDropPoint>();
        [SerializeField] private float m_levelScaling;
        [SerializeField] private bool m_debugMode;
        [SerializeField] private float m_maxUpgradeCooldown;
        [SerializeField] private SpriteRenderer m_weaponIcon;

        //Private variables
        private Transform m_position;
        private float m_currentHealth;
        private int m_currentStationLevel;
        private bool m_isDisabled = false;

        //Serialized for test purpose
        [SerializeField] private List<Crafting.InteractableResource.EResourceType> m_resourceNeeded = new List<Crafting.InteractableResource.EResourceType>();
        [SerializeField] private List<Crafting.InteractableResource.EResourceType> m_currentResources = new List<Crafting.InteractableResource.EResourceType>();
        [SerializeField] private List<Crafting.InteractableResource.EResourceType> m_possibleResources = new List<Crafting.InteractableResource.EResourceType>();
        [SerializeField] private float m_currentUpgradeCD = 0.0f;
        [SerializeField] private bool m_isUpgrading = false;

        //Static variables
        //static Upgrade currentUpgrade;
        static List<CraftingStation> m_craftingStationsList = new List<CraftingStation>();
        static EWeaponUpgrades m_currentUpgrade = EWeaponUpgrades.Count;

        public static EWeaponUpgrades CurrentUpgrade { get { return m_currentUpgrade; } }

        // Start is called before the first frame update
        private void Awake()
        {
            m_craftingStationsList.Add(this);            
        }
        void Start()
        {
            Initialization();
            //Debug.Log("Crafting station health is " + m_maxHealth );
        }
        // Update is called once per frame
        void Update()
        {
            if (m_isDisabled)
            {
                Debug.Log("Station disabled");
                return;
            }                

            if (m_currentHealth <= 0)
            {
                m_isDisabled = true;
            }

            if (m_isUpgrading)
            {
                m_currentUpgradeCD -= Time.deltaTime;

                if (m_currentUpgradeCD < 0.0f)
                {
                    ResetDropStation();
                }
            }
        }
        public static List<CraftingStation> GetCraftingStations()
        {            
            return m_craftingStationsList;
        }
        #region StationManagement
        private void Initialization()
        {
            m_isDisabled = false;
            m_currentStationLevel = 1;
            m_currentHealth = m_maxHealth;
            ResourceNeededAllocation();
            if (m_currentUpgrade == EWeaponUpgrades.Count)
            {
                ResetUpgrade();
            }
        }
        static void ResetUpgrade()
        {
            m_currentUpgrade = (EWeaponUpgrades)Random.Range(0, (int)EWeaponUpgrades.Count);
            //Debug.Log("Chosen upgrade is " + m_currentUpgrade);
        }
        private void ResetDropStation()
        {
            m_isUpgrading = false;
            ResetPossibleResourceList();
            ResourceNeededAllocation();
        }
        public void StationSetup(WeaponSystem.PlayerWeapon weapon)
        {
            m_linkedWeapon = weapon;
            m_weaponIcon.sprite = weapon.GetComponent<SpriteRenderer>().sprite;
        }
        #endregion
        public List<ResourceDropPoint> GetDropPopint()
        {
            return m_resourceDropPoints;
        }
        public void ReceiveDamage(float damage)
        {
            m_currentHealth -= damage;
            Debug.Log("Crafting Station current hit points " + m_currentHealth);
        }
        public bool GetIsDisabled() { return m_isDisabled; }
        #region UpgradeManagement
        private void CheckIfUpgradable()
        {
            //Sort both list before comparing their values
            if (m_resourceNeeded.Count == 0)
            {
                if (m_debugMode) { Debug.Log("CrafingStation " + gameObject.name + " is upgrading weapon"); }

                m_linkedWeapon.Upgrade(m_currentUpgrade);
                m_currentResources.Clear();
                m_isUpgrading = true;
                m_currentUpgradeCD = m_maxUpgradeCooldown;
                m_currentStationLevel++;
                ResetUpgrade();

                FXSystem.FXManager fxManager = FXSystem.FXManager.Instance;
                if (fxManager != null)
                {
                    fxManager.PlayAudio(FXSystem.ESFXType.WeaponUpgrading);
                }
            }
        }
        #endregion
        #region ResourceManagement
        public bool AddResource(Crafting.InteractableResource.EResourceType resourceType)
        {
            //Check if the resource is needed
            if (m_resourceNeeded.Contains(resourceType))
            {
                m_currentResources.Add(resourceType);
                m_currentResources.Sort();
                m_resourceNeeded.Remove(resourceType);
                CheckIfUpgradable();

                if (m_debugMode)
                {
                    Debug.Log("AddResource called on " + gameObject.name);
                    foreach (Crafting.InteractableResource.EResourceType resource in m_currentResources)
                    {
                        Debug.Log("For crafting station " + gameObject.name + " there is a " + resource);
                    }
                }

                return true;
            }
            return false;
        }
        private void ResetPossibleResourceList()
        {
            m_possibleResources.Clear();
            for (int i = 0; i != (int)Crafting.InteractableResource.EResourceType.Count; i++)
            {
                if (m_debugMode) { Debug.Log("Added to m_possibleResource : " + (Crafting.InteractableResource.EResourceType)i); }
                m_possibleResources.Add((Crafting.InteractableResource.EResourceType)i);
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
                m_resourceDropPoints[i].AllocateResource((Crafting.InteractableResource.EResourceType)resourceIndex, currentResourceAllocation);

                //Check if the amount is 0 before adding it to neededResources
                if (currentResourceAllocation != 0) { m_resourceNeeded.Add((InteractableResource.EResourceType)resourceIndex); }
            }
        }
        #endregion
        #region Enums
        public enum EWeaponUpgrades
        {
            AttackSpeed,
            AttackZone,
            AttackRange,
            AttackDamage,
            Count
        }
        #endregion
    }
}
