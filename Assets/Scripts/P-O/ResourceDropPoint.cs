using UnityEngine;

namespace SpaceBaboon
{
    public class ResourceDropPoint : MonoBehaviour
    {
        //Serialized variables
        [SerializeField]
        private bool m_DebugMode = false;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void CollectResource()
        {
            if (m_DebugMode) { Debug.Log("Player activated CollectResource on station"); }
        }

        public void AllocateResource() { }
    }
}
