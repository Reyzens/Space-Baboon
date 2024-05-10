using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace SpaceBaboon
{
    [ExecuteAlways]
    public class ShaderController : MonoBehaviour
    {
        private Material m_playerSpriteRendererMaterial;
        [SerializeField]
        private Color m_color;

        // Start is called before the first frame update

        void Start()
        {
            m_playerSpriteRendererMaterial = GetComponent<SpriteRenderer>().material;
            SetOutlineColor(m_color);
        }

        // Update is called once per frame
        void Update()
        {
            SetOutlineColor(m_color);
        }

        private void SetOutlineColor(Color newcolor)
        {
            m_playerSpriteRendererMaterial.SetColor("_Color", newcolor);
        }
        
    }
}
