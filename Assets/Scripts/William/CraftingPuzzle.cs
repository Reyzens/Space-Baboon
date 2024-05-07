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
        [SerializeField]
        private GameObject m_blueCircle;
        [SerializeField]
        private GameObject m_circleMask;
        [SerializeField]
        private GameObject m_blueCircleFiller;
        [SerializeField]
        private Sprite m_enableStationSprite;
        [SerializeField]
        private Sprite m_disableStationSprite;
        [SerializeField]
        private GameObject m_stationSpriteRef;
        private SpriteRenderer m_stationRenderer;

        private float m_transparentCirclePercentage;
        Vector3 m_transparentCircleNewPosition;
        private bool m_transparentCircleMorphing;
        




        private void Initialisation()
        {
            m_craftingPuzzleEnable = true;
            m_craftingStationScript = GetComponent<CraftingStation>();
            m_stationRenderer = m_stationSpriteRef.GetComponent<SpriteRenderer>();
            m_dropPointList = m_craftingStationScript.GetDropPopint();
            m_currentkill = 0;
            m_zoneCircle = GameObject.Find("Circle");
            SetDropPoints();
            m_transparentCirclePercentage = 0.0f;
            m_transparentCircleNewPosition = new Vector3();
            m_transparentCircleMorphing = false;
            
            
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
            //CircleLerping();
            m_blueCircleFiller.transform.localScale = Vector3.Lerp(m_blueCircleFiller.transform.localScale, m_transparentCircleNewPosition, Time.deltaTime * 1.0f);
            //m_blueCircleFiller.transform.localScale = m_transparentCircleNewPosition;

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
                m_stationRenderer.sprite = m_enableStationSprite;
            }
        }

        public void PuzzleCounter()
        {
            if (m_craftingPuzzleEnable == true)
            {
                m_currentkill += 1;
                m_transparentCirclePercentage = (float)m_currentkill / m_killneeded * 1.8f;
                m_transparentCircleNewPosition = new Vector3(m_transparentCirclePercentage, m_transparentCirclePercentage, m_transparentCirclePercentage);
                m_transparentCircleMorphing = true;
            }
        }

        public void CircleLerping()
        {
            if (m_transparentCircleMorphing == true)
            {
                m_blueCircleFiller.transform.localScale = m_transparentCircleNewPosition;
                //Vector3.Lerp(m_blueCircleFiller.transform.localScale, m_transparentCircleNewPosition, Time.deltaTime * 2.0f);
                m_transparentCircleMorphing = false;
            }
        }

        private void ReactivateCraftingStation()
        {
            if(m_craftingPuzzleEnable == false)
            {
                m_blueCircle.gameObject.SetActive(false);
                m_circleMask.gameObject.SetActive(false);
                m_blueCircleFiller.gameObject.SetActive(false);
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
