using System;
using System.Collections.Generic;
using SpaceBaboon.Crafting;
using Unity.VisualScripting;
using UnityEngine;

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
        private List<GameObject> m_enemiesInArea;
        [SerializeField] 
        private List<ResourceDropPoint> m_dropPointList;
        [SerializeField]
        private CraftingStation m_craftingStationScript;


        private void Initialisation()
        {
            m_craftingPuzzleEnable = true;
            m_craftingStationScript = GetComponent<CraftingStation>();
            m_dropPointList = m_craftingStationScript.m_resourceDropPoints;
            m_enemiesInArea = new List<GameObject>();
            m_currentkill = 0;
            SetDropPoints();
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

        private void PuzzleCounter()
        {
            if (m_craftingPuzzleEnable == true)
            {
                foreach (GameObject enemy in m_enemiesInArea)
                {
                    if (enemy == null)
                    {
                        m_currentkill += 1;
                        m_enemiesInArea.Remove(enemy);
                    }
                }
            }
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
            PuzzleCounter();
        }

        private void OnTriggerEnter2D(Collider2D collidedObject)
        {
            if (collidedObject.gameObject.CompareTag("Enemy") && m_craftingPuzzleEnable == true)
            {
                m_enemiesInArea.Add(collidedObject.gameObject);
            }
        }

        private void OnTriggerExit(Collider collidedObject)
        {
            if (collidedObject.gameObject.CompareTag("Enemy") && m_craftingPuzzleEnable == true)
            {
                m_enemiesInArea.Remove(collidedObject.gameObject);
            }
        }
    }
}
