using System;
using System.Collections.Generic;
using SpaceBaboon.Crafting;
using SpaceBaboon.EnemySystem;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace SpaceBaboon
{
    public class CraftingPuzzle : MonoBehaviour
    {
        [SerializeField]
        private bool m_craftingPuzzleEnable;
        [SerializeField]
        private int m_killneeded;
        [SerializeField]
        private int m_currentkill;
        [SerializeField] 
        private List<ResourceDropPoint> m_dropPointList;
        [SerializeField]
        private CraftingStation m_craftingStationScript;
        private GameObject m_zoneCircle;

        
        


        private void Initialisation()
        {
            m_craftingPuzzleEnable = true;
            m_craftingStationScript = GetComponent<CraftingStation>();
            m_dropPointList = m_craftingStationScript.GetDropPopint();
            m_currentkill = 0;
            m_zoneCircle = GameObject.Find("Circle");
            SetDropPoints();
           
        }

        // Start is called before the first frame update
        void Start()
        {
            Initialisation();
        }

        // Update is called once per frame
        void Update()
        {
            PuzzleDisabler();
            ReactivateCraftingStation();
        }

        private void SetDropPoints()
        {
            foreach (var dropPoint in m_dropPointList)
            {
                dropPoint.gameObject.SetActive(false);
            }
        }

        private void PuzzleDisabler()
        {
            if (m_currentkill >= m_killneeded)
            {
                m_craftingPuzzleEnable = false;
            }
        }

        public void PuzzleCounter()
        {
            if (m_craftingPuzzleEnable == true)
            {
                m_currentkill += 1;
            }
        }

        private void ReactivateCraftingStation()
        {
            if(m_craftingPuzzleEnable == false)
            {
                m_zoneCircle.gameObject.SetActive(false);
                foreach (var dropPoint in m_dropPointList)
                {
                    dropPoint.gameObject.SetActive(true);
                }
            }
        }
        private void OnEnemyDeathSubsribe(GameObject collider)
        {          
            collider.GetComponent<Enemy>().registerPuzzle(this);
        }
        private void OnEnemyDeathUnsubscribe(GameObject collider)
        {
            collider.GetComponent<Enemy>().UnregisterPuzzle(this);
        }
        private void OnEnemyDetected(GameObject collider)
        {
            if(m_craftingPuzzleEnable) 
            { 
                OnEnemyDeathSubsribe(collider);
            }    
        }
        private void OnEnemyExit(GameObject collider)
        {
            if (m_craftingPuzzleEnable)
            { 
                OnEnemyDeathUnsubscribe(collider);
            }       
        }
    }
}
