using UnityEngine;
using UnityEngine.UIElements;

namespace SpaceBaboon.UISystem
{
    public class UIInfoBox
    {
        private UIManager m_manager;
        private VisualElement m_root;

        public void Create(UIManager manager, VisualElement root)
        {
            m_manager = manager;
            m_root = root;





            Enable();
        }

        private void Enable()
        {

        }

        public void Disable()
        {

        }
    }
}
