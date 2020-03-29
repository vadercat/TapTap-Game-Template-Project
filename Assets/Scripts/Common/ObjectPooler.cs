using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TapTap
{
    public class ObjectPooler : IInstance
    {
        private Dictionary<string, List<IPoolable>> m_Pool = new Dictionary<string, List<IPoolable>>();

        public static T Pop<T>(string entityName) // Shortcut
        {
            return Main.Get<ObjectPooler>().PopFromPool<T>(entityName);
        }

        public static bool Push(string entityName, IPoolable poolableObject) // Shortcut
        {
            return Main.Get<ObjectPooler>().PushToPool(entityName, poolableObject);
        }

        public bool TryRemoveFromCollection(string entityName, IPoolable poolableObject)
        {
            if (poolableObject == null || string.IsNullOrEmpty(entityName))
            {
                return false;
            }

            if (!m_Pool.ContainsKey(entityName))
            {
                return false;
            }

            int index = m_Pool[entityName].IndexOf(poolableObject);

            if (index == -1)
            {
                return false;
            }

            m_Pool[entityName].RemoveAt(index);

            return true;
        }

        public T PopFromPool<T>(string entityName)
        {
            if (string.IsNullOrEmpty(entityName))
            {
                return default(T);
            }

            if (!m_Pool.ContainsKey(entityName))
            {
                return default(T);
            }

            List<IPoolable> list = m_Pool[entityName];

            if (list.Count < 1)
            {
                return default(T);
            }

            int lastIndex = list.Count - 1;
            IPoolable poolableObject = list[lastIndex];
            list.RemoveAt(lastIndex);

            poolableObject.OnPoolPopEvent();

            return (T)poolableObject;
        }

        public bool PushToPool(string entityName, IPoolable poolableObject)
        {
            if (poolableObject == null)
            {
                Debug.LogError("Can't push object to pool! Object is null!");

                return false;
            }

            if (string.IsNullOrEmpty(entityName))
            {
                Debug.LogError("Can't push object to pool! <Entity Name> is null or empty!", poolableObject as Object);

                return false;
            }

            if (!m_Pool.ContainsKey(entityName))
            {
                m_Pool.Add(entityName, new List<IPoolable>());
            }

            if (m_Pool[entityName].Contains(poolableObject))
            {
                return false;
            }

            m_Pool[entityName].Add(poolableObject);

            poolableObject.OnPoolPushEvent();

            return true;
        }
    }
}
