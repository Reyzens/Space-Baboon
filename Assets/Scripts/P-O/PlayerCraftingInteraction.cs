using UnityEngine;

namespace SpaceBaboon
{
    public class PlayerCraftingInteraction : MonoBehaviour
    {
        [SerializeField]
        private bool m_DebugMode = false;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (m_DebugMode && collision.gameObject.tag == "Structure") { Debug.Log("CollisionDetected with structure"); }

            if (collision.gameObject.tag == "Structure")
            {

            }
        }
    }
}
