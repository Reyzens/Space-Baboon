using SpaceBaboon.Crafting;

namespace SpaceBaboon
{
    public class HealthCollectable : InteractableResource
    {
        protected override void Start()
        {
            m_collectRange = GameManager.Instance.Player.GetPlayerCollectRange();
            m_circleCollider.radius = m_collectRange;
            m_rendereInitialColor = m_renderer.color;
        }
        public override void Collect(Player collectingPlayer)
        {

        }
    }
}
