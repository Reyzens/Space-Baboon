using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceBaboon.TutorialSystem
{
    public enum ETutorialType
    {
        UnlockCraftingStation,

    }

    public class TutorialPopUpWindow : MonoBehaviour
    {
        private Camera m_camera;
        private Image m_window;
        private RectTransform m_windowRT;
        private TextMeshProUGUI m_text;

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
        }

        public void Display(ETutorialType type, Vector3 position)
        {
            var screenPosition = m_camera.WorldToScreenPoint(position);
            m_windowRT.position = screenPosition;

            m_window.gameObject.SetActive(true);
        }
    }
}
