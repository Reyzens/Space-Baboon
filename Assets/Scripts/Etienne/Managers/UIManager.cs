using SpaceBaboon.WeaponSystem;
using UnityEngine;
using UnityEngine.UIElements;

namespace SpaceBaboon.UISystem
{
    public class UIManager : MonoBehaviour
    {
        private static UIManager instance;
        public static UIManager Instance
        {
            get
            {
                if (instance != null) { return instance; }

                Debug.LogError("UIManager instance is null");
                return null;
            }
        }


        [SerializeReference] private UITopBox m_topBoxScript = new UITopBox();
        [SerializeReference] private UIWeaponBox m_weaponBoxScript = new UIWeaponBox();
        private UIInfoBox m_infoBoxScript = new UIInfoBox();

        private UIDocument m_uiDoc;

        private VisualElement m_topBox;
        private VisualElement m_weaponBox;
        private VisualElement m_infoBox;


        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this);
                return;
            }
            instance = this;

            //---------------------

            m_uiDoc = GetComponent<UIDocument>();
            VisualElement visualElement = m_uiDoc.rootVisualElement;

            m_topBox = visualElement.Q<VisualElement>("TopBox");
            m_topBoxScript.Create(this, m_topBox);

            m_weaponBox = visualElement.Q<VisualElement>("SideBox");
            m_weaponBoxScript.Create(this, m_weaponBox, m_infoBoxScript);

            m_infoBox = visualElement.Q<VisualElement>("InfoBox");
            m_infoBoxScript.Create(this, m_infoBox);

        }

        private void OnDisable()
        {
            m_topBoxScript.Disable();
            m_weaponBoxScript.Disable();
            m_infoBoxScript.Disable();
        }

        private void Update()
        {
            int timer = (int)GameManager.Instance.GameTimer;
            m_topBoxScript.UpdateTimer(timer);
        }

        public void UpdateResource(Crafting.InteractableResource.EResourceType resourceType, int amount)
        {
            m_topBoxScript.UpdateResource(resourceType, amount);
        }

        public void UpdateCurrentUpgrade(Crafting.CraftingStation.EWeaponUpgrades upgradeType)
        {
            m_topBoxScript.UpdateCurrentUpgrade(upgradeType);
        }

        public void UpdateWeapon(WeaponSystem.EPlayerWeaponType weaponType, int totalLevel, int dLevel, int sLevel, int rLevel, int zLevel)
        {
            m_weaponBoxScript.UpdateWeapon(weaponType, totalLevel, dLevel, sLevel, rLevel, zLevel);
        }

    }
}
