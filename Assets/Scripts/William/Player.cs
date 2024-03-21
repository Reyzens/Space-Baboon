using System.Collections.Generic;
using UnityEngine;

namespace SpaceBaboon
{
    public class Player : MonoBehaviour
    {
        [SerializeField]
        private PlayerData m_playerData;
        [SerializeField]
        private bool m_DebugMode = false;
        private Dictionary<SpaceBaboon.InteractableResource.EResourceType, int> m_collectibleInventory;
        private List<Weapon> m_equipedWeapon;
        private List<Weapon> m_blockedWeapon;

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
                collision.GetComponent<ResourceDropPoint>().CollectResource();
            }
        }
    }
}
