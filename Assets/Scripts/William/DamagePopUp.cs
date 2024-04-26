using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

namespace SpaceBaboon
{
    public class DamagePopUp : MonoBehaviour
    {
        private TextMeshPro m_popUpDmgText;
        
        public static DamagePopUp Create( Vector3 position, int damage)
        {
            // Cree le dmg
            Transform damagePopUpTransform = Instantiate(GameManager.Instance.dmgPopUpPrefab,position,Quaternion.identity);
            // Set ref
            DamagePopUp damagePopUp = damagePopUpTransform.GetComponent<DamagePopUp>();
            damagePopUp.SetupDmgPopUp(300);
            // return dmg
            return damagePopUp;
        }
        private void Awake()
        {
            m_popUpDmgText = transform.GetComponent<TextMeshPro>();
        }
       
        public void SetupDmgPopUp(int damageAmount)
        {
            m_popUpDmgText.text = damageAmount.ToString();
        }
    }
}
