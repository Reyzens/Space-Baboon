using UnityEngine;

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






        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this);
                return;
            }
            instance = this;
        }


        private void UpdateTimer()
        {

        }

        public void UpdateResource(Crafting.InteractableResource.EResourceType resourceType, int amount)
        {

        }

        public void UpdateCurrentUpgrade(Crafting.CraftingStation.EWeaponUpgrades upgradeType)
        {

        }

        public void UpdateWeapon(WeaponSystem.EPlayerWeaponType weaponType, int totalLevel)
        {

        }
    }
}
