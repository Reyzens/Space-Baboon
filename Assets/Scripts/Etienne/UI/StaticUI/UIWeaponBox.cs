using SpaceBaboon.WeaponSystem;
using UnityEngine;
using UnityEngine.UIElements;

namespace SpaceBaboon.UISystem
{
    public class UIWeaponBox
    {
        private UIManager m_manager;
        private VisualElement m_root;

        private VisualElement m_swordBox;
        private VisualElement m_flameThrowerBox;
        private VisualElement m_grenadeLauncherBox;
        private VisualElement m_shockwaveBox;
        private VisualElement m_laserBeamBox;

        private WeaponBoxData[] m_weaponsData = new WeaponBoxData[5];

        public void Create(UIManager manager, VisualElement root)
        {
            m_manager = manager;
            m_root = root;

            m_swordBox = root.Q<VisualElement>("SwordBox");
            m_weaponsData[0] = new WeaponBoxData(m_swordBox);
            m_flameThrowerBox = root.Q<VisualElement>("FlameThrowerBox");
            m_weaponsData[1] = new WeaponBoxData(m_flameThrowerBox);
            m_grenadeLauncherBox = root.Q<VisualElement>("GrenadeLauncherBox");
            m_weaponsData[2] = new WeaponBoxData(m_grenadeLauncherBox);
            m_shockwaveBox = root.Q<VisualElement>("ShockwaveBox");
            m_weaponsData[3] = new WeaponBoxData(m_shockwaveBox);
            m_laserBeamBox = root.Q<VisualElement>("LaserBeamBox");
            m_weaponsData[4] = new WeaponBoxData(m_laserBeamBox);


            Enable();
        }

        public void UpdateWeapon(EPlayerWeaponType type, int totalLevel)
        {
            m_weaponsData[(int)type].SetLevels(totalLevel);
        }



        private void Enable()
        {

        }

        public void Disable()
        {

        }


    }

    public struct WeaponBoxData
    {
        public WeaponBoxData(VisualElement vE)
        {
            visualElement = vE;
            levelDisplay = visualElement.Q<Label>("WeaponLevel");
            totalLevel = 1;

        }

        public void SetLevels(int tLevel)
        {
            totalLevel = tLevel;
            levelDisplay.text = totalLevel.ToString();
        }

        public VisualElement visualElement;
        public Label levelDisplay;
        public int totalLevel;
    }
}
