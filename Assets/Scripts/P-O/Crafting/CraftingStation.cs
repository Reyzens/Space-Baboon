using SpaceBaboon.WeaponSystem;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceBaboon.Crafting
{
    public class CraftingStation : MonoBehaviour
    {
        //Serializable variables
        [SerializeField] private PlayerWeapon m_linkedWeapon;
        [SerializeField] public int m_maxHealth;
        [SerializeField] private List<ResourceDropPoint> m_resourceDropPoints = new List<ResourceDropPoint>();
        [SerializeField] private float m_levelScaling;
        [SerializeField] private bool m_debugMode;
        [SerializeField] private float m_maxUpgradeCooldown;
        [SerializeField] private SpriteRenderer m_weaponIcon;

        //Private variables
        private Transform m_position;
        public int m_currentHealth;
        private int m_currentStationLevel;
        private bool m_isDisabled = false;
        private CraftingPuzzle m_puzzleScript;

        //Serialized for test purpose
        [SerializeField] private List<Crafting.InteractableResource.EResourceType> m_resourceNeeded = new List<Crafting.InteractableResource.EResourceType>();
        [SerializeField] private List<Crafting.InteractableResource.EResourceType> m_currentResources = new List<Crafting.InteractableResource.EResourceType>();
        [SerializeField] private List<Crafting.InteractableResource.EResourceType> m_possibleResources = new List<Crafting.InteractableResource.EResourceType>();
        [SerializeField] private float m_currentUpgradeCD = 0.0f;
        [SerializeField] private bool m_isUpgrading = false;

        //Static variables
        //static Upgrade currentUpgrade;
        static List<CraftingStation> m_craftingStationsList = new List<CraftingStation>();
        static List<EWeaponUpgrades> m_lastsUpgrades = new List<EWeaponUpgrades>();
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
        }
        // Update is called once per frame
        void Update()
        {
            // TODO FOR TESTING TO DELETE
            //if (Input.GetKeyDown(KeyCode.Y))
            //{
            //    m_isDisabled = false;
            //    m_currentHealth = m_maxHealth;
            //    m_puzzleScript.SetCraftingStationPuzzleEnabled(false);
            //}
            //if (Input.GetKeyDown(KeyCode.U))
            //{
            //    m_isDisabled = true;
            //    m_currentHealth = 0;
            //    m_puzzleScript.SetCraftingStationPuzzleEnabled(true);
            //}

            if (m_isDisabled)
            {
                //Debug.Log("Station disabled");
                return;
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
            m_currentStationLevel = 1;
            m_isDisabled = true;
            m_currentHealth = 0;
            ResourceNeededAllocation();
            if (m_currentUpgrade == EWeaponUpgrades.Count)
            {
                ResetUpgrade();
            }
            m_puzzleScript = GetComponent<CraftingPuzzle>();
            m_puzzleScript.Initialisation();
        }
        private void ResetDropStation()
        {
            m_isUpgrading = false;
            ResetPossibleResourceList();
            ResourceNeededAllocation();
        }
        public void ToggleStationStatus(bool status)
        {
            if (status)
            {
                m_isDisabled = false;
                m_currentHealth = m_maxHealth;
                m_puzzleScript.SetCraftingStationPuzzleEnabled(false);
            }
            else
            {
                m_isDisabled = true;
                m_currentHealth = 0;
                m_puzzleScript.SetCraftingStationPuzzleEnabled(true);
            }
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
        public bool ResourceIsNeeded(Crafting.InteractableResource.EResourceType resourceToCheck)
        {
            return m_resourceNeeded.Contains(resourceToCheck);
        }
        public void ReceiveDamage(float damage)
        {
            m_currentHealth -= (int)damage;
            if (m_currentHealth <= 0)
            {
                m_isDisabled = true;
                m_puzzleScript.SetCraftingStationPuzzleEnabled(true);
            }
        }
        public bool GetIsDisabled() { return m_isDisabled; }
        #region UpgradeManagement
        private void CheckIfUpgradable()
        {
            if (m_resourceNeeded.Count == 0)
            {
                if (m_debugMode) { Debug.Log("CrafingStation " + gameObject.name + " is upgrading weapon"); }

                LocalStationUpgrading();
                ResetUpgrade();

                FXSystem.FXManager fxManager = FXSystem.FXManager.Instance;
                if (fxManager != null)
                {
                    fxManager.PlayAudio(FXSystem.ESFXType.WeaponUpgrading);
                }
            }
        }
        private void LocalStationUpgrading()
        {
            m_linkedWeapon.Upgrade(m_currentUpgrade);
            m_currentResources.Clear();
            m_isUpgrading = true;
            m_currentUpgradeCD = m_maxUpgradeCooldown;
            m_currentStationLevel++;
            m_lastsUpgrades.Add(m_currentUpgrade);
        }
        private void LastUpgradesCheck()
        {
            bool isChoosingUpgrade = true;
            EWeaponUpgrades newUpgrade = ResetUpgrade();

            while (isChoosingUpgrade)
            {
                if (CheckLastTwoUpgrades(newUpgrade))
                {
                    newUpgrade = ResetUpgrade();
                }
                else
                {
                    isChoosingUpgrade = false;
                }
            }
        }
        private bool CheckLastTwoUpgrades(EWeaponUpgrades newUpgrade)
        {
            if (m_lastsUpgrades.Count < 2)
            {
                return false;
            }

            bool isUpgradeValid = (m_lastsUpgrades.Count >= 2 &&
                                   m_lastsUpgrades[0] != newUpgrade &&
                                   m_lastsUpgrades[1] != newUpgrade);

            //Check if the list countain more than two upgrades
            if (m_lastsUpgrades.Count >= 2)
            {
                m_lastsUpgrades.RemoveAt(0);
            }
            m_lastsUpgrades.Add(newUpgrade);

            return isUpgradeValid;
        }
        static EWeaponUpgrades ResetUpgrade()
        {
            //Debug.Log("Chosen upgrade is " + m_currentUpgrade);
            m_currentUpgrade = (EWeaponUpgrades)Random.Range(0, (int)EWeaponUpgrades.Count);

            UISystem.UIManager uiManager = UISystem.UIManager.Instance;
            if (uiManager != null)
            {
                uiManager.UpdateCurrentUpgrade(m_currentUpgrade);
            }

            return m_currentUpgrade;
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
                int resourceIndex = (initialResourceIndex + i) % ((int)InteractableResource.EResourceType.Count - 1);

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
