using UnityEngine;
using UnityEngine.UIElements;

namespace SpaceBaboon
{
    public class GameStaticUI : MonoBehaviour
    {
        private Player m_player;
        
        private UIDocument m_uiDoc;

        private Label m_crystalAmount;
        private Label m_technologyAmount;
        private Label m_metalAmount;

        private Label m_currentUpgrade;
        private Label m_gameTimer;

        private Button m_pauseButton;


        private void Awake()
        {
            m_player = GameManager.Instance.Player;
            
            m_uiDoc = GetComponent<UIDocument>();
            VisualElement visualElement = m_uiDoc.rootVisualElement;

            m_crystalAmount = visualElement.Q<Label>("CrystalRessourceAmount");
            m_technologyAmount = visualElement.Q<Label>("TechnologyRessourceAmount");
            m_metalAmount = visualElement.Q<Label>("MetalRessourceAmount");

            m_currentUpgrade = visualElement.Q<Label>("UpgradeName");
            m_gameTimer = visualElement.Q<Label>("TimerAmount");
        }

        private void Update()
        {
            m_metalAmount.text = m_player.GetResources(0).ToString();
            m_crystalAmount.text = m_player.GetResources(1).ToString();
            m_technologyAmount.text = m_player.GetResources(2).ToString();

            m_currentUpgrade.text = Crafting.CraftingStation.CurrentUpgrade.ToString();
        }
    }
}
