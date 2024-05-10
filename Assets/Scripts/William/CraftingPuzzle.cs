using System;
using System.Collections.Generic;
using SpaceBaboon.Crafting;
using SpaceBaboon.EnemySystem;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;
using static UnityEngine.Rendering.DebugUI;

namespace SpaceBaboon
{
    public class CraftingPuzzle : MonoBehaviour
    {
        [SerializeField]
        private bool m_craftingPuzzleEnabled;
        [SerializeField]
        private int m_killneeded;
        [SerializeField]
        private int m_currentkill;
        [SerializeField] 
        private List<ResourceDropPoint> m_dropPointList;
        [SerializeField]
        private CraftingStation m_craftingStationScript;
        [SerializeField]
        private ResourceDropPoint m_ressourceDropPointScript;
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
        [SerializeField]
        private Light2D m_light2D;

        private float m_transparentCirclePercentage;
        Vector3 m_transparentCircleNewPosition;
        private bool m_transparentCircleMorphing;
        




        public void Initialisation()
        {
            m_craftingPuzzleEnabled = true;
            m_craftingStationScript = GetComponent<CraftingStation>();
            m_ressourceDropPointScript = GetComponentInChildren<ResourceDropPoint>();
            m_stationRenderer = m_stationSpriteRef.GetComponent<SpriteRenderer>();
            m_dropPointList = m_craftingStationScript.GetDropPopint();
            m_currentkill = 0;
            m_zoneCircle = GameObject.Find("Circle");
            //SetDropPoints();
            m_transparentCirclePercentage = 0.0f;
            m_transparentCircleNewPosition = new Vector3();
            m_transparentCircleMorphing = false;
            
            
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            PuzzleDisabler();
            //ReactivateCraftingStation();
            //DisableCraftinStation();

            if (Input.GetKeyDown(KeyCode.Y))
            {
                SetCraftingStationPuzzleVariable(true);
            }
            if (Input.GetKeyDown(KeyCode.U))
            {
                SetCraftingStationPuzzleVariable(false);
            }

            SetCraftingStationPuzzle(m_craftingPuzzleEnabled);

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
                m_craftingPuzzleEnabled = false;
                m_stationRenderer.sprite = m_enableStationSprite;
                m_light2D.color = Color.green;
            }
        }

        public void PuzzleCounter()
        {
            if (m_craftingPuzzleEnabled == true)
            {
                m_currentkill += 1;
                m_transparentCirclePercentage = (float)m_currentkill / m_killneeded * m_blueCircle.transform.localScale.x;
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

        //private void ReactivateCraftingStation()
        //{
        //    if(m_craftingPuzzleEnabled == false)
        //    {
        //        m_blueCircle.gameObject.SetActive(false);
        //        m_circleMask.gameObject.SetActive(false);
        //        m_blueCircleFiller.gameObject.SetActive(false);
        //        foreach (var dropPoint in m_dropPointList)
        //        {
        //            dropPoint.gameObject.SetActive(true);
        //        }
        //    }
        //}
        //
        //private void DisableCraftinStation()
        //{
        //    if (m_craftingPuzzleEnabled == true)
        //    {
        //        m_blueCircle.gameObject.SetActive(true);
        //        m_circleMask.gameObject.SetActive(true);
        //        m_blueCircleFiller.gameObject.SetActive(true);
        //        foreach (var dropPoint in m_dropPointList)
        //        {
        //            dropPoint.gameObject.SetActive(false);
        //        }
        //    }
        //}

        private void SetCraftingStationPuzzle(bool value)
        {
            m_blueCircle.gameObject.SetActive(value);
            m_circleMask.gameObject.SetActive(value);
            m_blueCircleFiller.gameObject.SetActive(value);
            
            if (value == true) 
            {
                m_stationRenderer.sprite = m_disableStationSprite;
                m_light2D.color = Color.red;
            }
            if (value == false)
            {
                m_stationRenderer.sprite = m_enableStationSprite;                
                m_light2D.color = Color.green;
            }       

            foreach (var dropPoint in m_dropPointList)
            {
                
                dropPoint.gameObject.SetActive(!value);
            }
        }

        public void SetCraftingStationPuzzleVariable(bool value)
        {
            m_craftingPuzzleEnabled = value;

            //if (enable)
            //{
            //    m_craftingPuzzleEnabled = true;
            //}
            //if (enable == false)
            //{
            //    m_craftingPuzzleEnabled = false;
            //}
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
            if(m_craftingPuzzleEnabled) 
            { 
                OnEnemyDeathSubsribe(collider);
            }    
        }
        private void OnEnemyExit(GameObject collider)
        {
            if (m_craftingPuzzleEnabled)
            { 
                OnEnemyDeathUnsubscribe(collider);
            }       
        }
    }
}
