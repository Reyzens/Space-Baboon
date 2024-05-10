using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceBaboon.TutorialSystem
{
    public enum ETutorialType
    {
        UnlockCraftingStation,
        SomeEnum,
        BossSpawn,
    }

    public class TutorialPopUpWindow : MonoBehaviour
    {
        private Camera m_camera;
        private Image m_window;
        private RectTransform m_windowRT;
        private TextMeshProUGUI m_text;

        [SerializeField] private List<PopUpText> m_popUpTexts = new List<PopUpText>();
        private Dictionary<ETutorialType, string> m_popUpsDictionary = new Dictionary<ETutorialType, string>();

        private Vector3 m_defaultHidingPos = new Vector3(1200,0,0); //Just a random position outside of screen

        private void Awake()
        {
            m_camera = Camera.main;
            m_window = GetComponentInChildren<Image>();
            m_windowRT = m_window.GetComponent<RectTransform>();
            m_text = GetComponentInChildren<TextMeshProUGUI>();

            m_window.gameObject.SetActive(false);
        }

        private void Start()
        {
            GameManager.Instance.SetTutorialWindow(this);

            foreach (var item in m_popUpTexts)
            {
                m_popUpsDictionary.Add(item.type, item.text);

            }
        }

        public void Display(ETutorialType type, Vector3 position)
        {
            var screenPosition = m_camera.WorldToScreenPoint(position);
            m_windowRT.position = screenPosition;

            if (m_text == null)
            {
                Debug.Log("null ref");
            }
            m_text.text = m_popUpsDictionary[type];



            m_window.gameObject.SetActive(true);
        }

        public void Hide()
        {
            m_window.gameObject.SetActive(false);
            m_windowRT.position = m_defaultHidingPos;

            //GameManager.Instance.PauseGame(false);
        }
    }

    [System.Serializable]
    public struct PopUpText
    {
        public ETutorialType type;
        [TextArea(3, 10)] public string text;
    }
}
