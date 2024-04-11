using TMPro;
using UnityEngine;

namespace SpaceBaboon
{
    public class ResourceDropPoint : MonoBehaviour
    {
        //Serialized variables
        [SerializeField]
        private bool m_DebugMode = false;
        [SerializeField]
        private CraftingStation m_craftingStation;

        //Private variables
        private int m_resourceAmountNeeded;
        private TextMeshPro m_resourceAmountDisplay;
        private SpaceBaboon.InteractableResource.EResourceType m_resourceTypeNeeded;

        // Start is called before the first frame update
        void Start()
        {
            m_resourceAmountDisplay = GetComponentInChildren<TextMeshPro>();
        }

        // Update is called once per frame
        void Update()
        {
            m_resourceAmountDisplay.text = m_resourceAmountNeeded.ToString();
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

        public void AllocateResource(SpaceBaboon.InteractableResource.EResourceType resourceType, int resourceAmount)
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

                GetComponent<SpriteRenderer>().color = newColor;
                GetComponent<CircleCollider2D>().enabled = true;
                m_resourceAmountDisplay.enabled = true;
            }
            else
            {
                //Invisible
                GetComponent<SpriteRenderer>().color = Color.clear;
                GetComponent<CircleCollider2D>().enabled = false;
                m_resourceAmountDisplay.enabled = false;
            }
        }
    }
}
