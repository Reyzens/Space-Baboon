using System.Collections.Generic;
using UnityEngine;

namespace SpaceBaboon
{
    public class ObjectPool : MonoBehaviour
    {

        [SerializeField] private GameObject m_prefab;
        [SerializeField] private int m_poolSize;

        private List<GameObject> pooledObjects = new List<GameObject>();


        public void CreatePool(GameObject prefab)
        {
            m_prefab = prefab;
            if (m_poolSize <= 0)
            {
                Debug.LogError("Invalid pool size");
                m_poolSize = 10;
            }
            for (int i = 0; i < m_poolSize; i++)
            {
                GameObject obj = Instantiate(m_prefab);
                obj.GetComponent<IPoolable>()?.Deactivate();
                pooledObjects.Add(obj);
                Debug.Log("Awake : " + pooledObjects.Count);

            }

        }

        public GameObject Spawn(Vector2 pos)
        {
            foreach (GameObject obj in pooledObjects)
            {
                var pooledObject = obj.GetComponent<IPoolable>();
                if (pooledObject != null && !pooledObject.IsActive)
                {
                    pooledObject.Activate(pos, this);
                    Debug.Log("activating");

                    return obj;
                }
                else
                {
                    if (pooledObject.IsActive)
                    {
                        Debug.Log(obj.name + " is active");
                    }
                    if (pooledObject == null)
                    {
                        Debug.Log(obj.name + " is not poolable");
                    }
                }
            }

            //If pool is maxed 
            GameObject newObj = Instantiate(m_prefab);
            pooledObjects.Add(newObj);
            newObj.GetComponent<IPoolable>()?.Activate(pos, this);
            Debug.Log("activating new : " + pooledObjects.Count);

            return newObj;
        }

        public void UnSpawn(GameObject obj)
        {
            var pooledObject = obj.GetComponent<IPoolable>();
            if (pooledObject == null)
            {
                Debug.Log(obj.name + "is not poolable");
                return;
            }
            pooledObject.Deactivate();
        }
    }
}
