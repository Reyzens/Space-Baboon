using System.Collections.Generic;
using UnityEngine;

namespace SpaceBaboon.PoolingSystem
{
    [System.Serializable]
    public class GenericObjectPool
    {
        [SerializeField] private GameObject m_container;
        [SerializeField] private int m_poolStartingSize;

        private Dictionary<string, Queue<GameObject>> m_pooledObjects = new Dictionary<string, Queue<GameObject>>();

        public void CreatePool(List<GameObject> prefabList, string poolName)
        {            
            m_container = new GameObject();
            m_container.name = poolName + " pool";

            //Debug.Log("Container called" + m_container);

            foreach (GameObject prefab in prefabList)
            {
                if (m_poolStartingSize <= 0)
                {
                    Debug.LogError("Invalid pool size");
                    m_poolStartingSize = 10;
                }

                Queue<GameObject> newQueue = new Queue<GameObject>();
                for (int i = 0; i < m_poolStartingSize; i++)
                {                    
                    GameObject obj = GameObject.Instantiate(prefab, m_container.transform);

                    obj.GetComponent<IPoolableGeneric>().Deactivate();
                    newQueue.Enqueue(obj);
                }               

                string keyName = prefab.name + "(Clone)";
                m_pooledObjects.Add(keyName, newQueue);
            }
        }

        public GameObject Spawn(GameObject prefabToSpawn, Vector2 pos)
        {
            string keyName = prefabToSpawn.name + "(Clone)";
            if (m_pooledObjects[keyName].Count != 0)
            {
                var obj = m_pooledObjects[keyName].Dequeue();

                var pooledObj = obj.GetComponent<IPoolableGeneric>();
                pooledObj.Activate(pos, this);

                return obj;
            }

            //If pool is empty 
            GameObject newObj = GameObject.Instantiate(prefabToSpawn, m_container.transform);
            newObj.GetComponent<IPoolableGeneric>().Activate(pos, this);
            //Debug.Log("activating new : " + prefabToSpawn.name);

            return newObj;
        }

        public void UnSpawn(GameObject obj)
        {
            var pooledObject = obj.GetComponent<IPoolableGeneric>();
            if (pooledObject == null)
            {
                Debug.LogError(obj.name + "is not poolable");
                return;
            }
            pooledObject.Deactivate();

            //Debug.Log("unspawn : " + obj.name);
            m_pooledObjects[obj.name].Enqueue(obj);
        }

        public void SetPoolStartingSize(int poolSize)
        {
            m_poolStartingSize = poolSize;
        }
    }
}
