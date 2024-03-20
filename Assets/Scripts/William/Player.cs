using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceBaboon
{
    public class Player : MonoBehaviour
    {
        [SerializeField]
        private PlayerData m_playerData;
        private Dictionary<SpaceBaboon.InteractableResource.EResourceType,int> m_collectibleInventory;
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
    }
}
