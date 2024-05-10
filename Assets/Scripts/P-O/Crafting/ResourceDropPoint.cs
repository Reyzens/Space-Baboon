using TMPro;
using UnityEngine;

namespace SpaceBaboon.Crafting
{
    public class ResourceDropPoint : MonoBehaviour
    {
        //Serialized variables
        [SerializeField]
        private bool m_DebugMode = false;
        [SerializeField]
        private CraftingStation m_craftingStation;
        [SerializeField]
        private GameObject m_circleRef;

        //Private variables
        private int m_resourceAmountNeeded;
        private TextMeshPro m_resourceAmountDisplay;
        private Crafting.InteractableResource.EResourceType m_resourceTypeNeeded;
        private SpriteMask m_dropPointMask;
        private SpriteRenderer m_circleDropPointref;
        

        // Start is called before the first frame update
        void Awake()
        {
            m_resourceAmountDisplay = GetComponentInChildren<TextMeshPro>();
            m_dropPointMask = GetComponentInChildren<SpriteMask>();
            m_circleDropPointref = m_circleRef.GetComponent<SpriteRenderer>();
        }

        // Update is called once per frame
        void Update()
        {
           //UpdateMaskSize();
        }

        private void UpdateMaskSize()
        {
            m_dropPointMask.transform.localScale = new Vector2(NewSize(), NewSize());
        }
        private float NewSize()
        {
            float playerResources = GameManager.Instance.Player.GetResources((int)m_resourceTypeNeeded);

            // Ensure we don't divide by zero
            if (playerResources == 0)
                return 1;  // Return full size when no resources are available

            float sizeRatio = m_resourceAmountNeeded / playerResources;

            // Calculate the inverse ratio to make the mask smaller as resources increase
            float newSize = 1 - Mathf.Clamp(sizeRatio, 0, 1);

            return newSize;
        }
        public void CollectResource(Player playerRef)
        {
            if (m_DebugMode) { Debug.Log("Player activated CollectResource on station"); }

            if (playerRef.DropResource(m_resourceTypeNeeded, m_resourceAmountNeeded))
            {
                if (m_DebugMode) { Debug.Log("Calling AddResource on " + m_craftingStation.gameObject.name); }

                //TODO Clean this up
                if (m_craftingStation.AddResource(m_resourceTypeNeeded))
                {
                    if (m_DebugMode) { Debug.Log(gameObject.name + " collected " + m_resourceTypeNeeded); }
                    GetComponent<SpriteRenderer>().color = Color.clear;
                    GetComponent<CircleCollider2D>().enabled = false;
                    m_resourceAmountDisplay.enabled = false;
                }
            }

        }
        private void CheatUpgrade()
        {
            m_craftingStation.AddResource(m_resourceTypeNeeded);
            GetComponent<SpriteRenderer>().color = Color.clear;
            GetComponent<CircleCollider2D>().enabled = false;
            m_resourceAmountDisplay.enabled = false;
        }
        public void AllocateResource(Crafting.InteractableResource.EResourceType resourceType, int resourceAmount)
        {
            if (m_DebugMode) { Debug.Log("To " + gameObject.name + " was allocated " + resourceAmount + " " + resourceType); }
            m_resourceTypeNeeded = resourceType;
            m_resourceAmountNeeded = resourceAmount;

            Color newColor = Color.white;
            if (resourceAmount > 0)
            {
                if (resourceType == InteractableResource.EResourceType.Metal)
                {
                    //Yellow
                    newColor = Color.yellow;
                }
                else if (resourceType == InteractableResource.EResourceType.Crystal)
                {
                    //Pink
                    newColor = Color.magenta;
                }
                else if (resourceType == InteractableResource.EResourceType.Technologie)
                {
                    //Light blue
                    newColor = Color.cyan;
                }
                m_resourceAmountDisplay.text = m_resourceAmountNeeded.ToString();
                m_circleDropPointref.color = newColor;
                GetComponent<CircleCollider2D>().enabled = true;
                m_resourceAmountDisplay.enabled = true;
            }
            else
            {
                //Invisible
                m_circleDropPointref.color = Color.clear;
                GetComponent<CircleCollider2D>().enabled = false;
                m_resourceAmountDisplay.enabled = false;
            }
        }
        public void SetRef()
        {
            m_circleDropPointref = m_circleRef.GetComponent<SpriteRenderer>();
        }
    }
}
