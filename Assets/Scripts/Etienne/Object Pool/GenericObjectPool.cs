using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceBaboon.PoolingSystem
{
    public class GenericObjectPool : MonoBehaviour
    {
        [SerializeField] private GameObject m_container;
        [SerializeField] private int m_poolSize;

        private Dictionary<string, Queue<GameObject>> m_pooledObjects = new Dictionary<string, Queue<GameObject>>();


        public void CreatePool(List<GameObject> prefabList, string poolName)
        {
            m_container = new GameObject();
            m_container.name = poolName + " pool";

            foreach (GameObject prefab in prefabList)
            {
                if (m_poolSize <= 0)
                {
                    Debug.LogError("Invalid pool size");
                    m_poolSize = 10;
                }

                Queue<GameObject> newQueue = new Queue<GameObject>();
                for (int i = 0; i < m_poolSize; i++)
                {
                    GameObject obj = Instantiate(prefab, m_container.transform);

                    obj.GetComponent<IPoolableNew>().Deactivate();
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

                var pooledObj = obj.GetComponent<IPoolableNew>();
                pooledObj.Activate(pos, this);

                return obj;
            }

            //If pool is empty 
            GameObject newObj = Instantiate(prefabToSpawn, m_container.transform);
            newObj.GetComponent<IPoolableNew>().Activate(pos, this);
            //Debug.Log("activating new : " + prefabToSpawn.name);

            return newObj;
        }

        public void UnSpawn(GameObject obj)
        {
            var pooledObject = obj.GetComponent<IPoolableNew>();
            if (pooledObject == null)
            {
                Debug.LogError(obj.name + "is not poolable");
                return;
            }
            pooledObject.Deactivate();

            //Debug.Log("unspawn : " + obj.name);
            m_pooledObjects[obj.name].Enqueue(obj);
        }

        public void SetPoolSize(int poolSize)
        {
            m_poolSize = poolSize;
        }
    }
}
