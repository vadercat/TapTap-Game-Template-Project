using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TapTap
{
    [DefaultExecutionOrder(ExecutionOrder.MinOrder)]
    public class Main : MonoBehaviour
    {
        private Dictionary<Type, IInstance> m_Instances = new Dictionary<Type, IInstance>();

        private UpdateSystem m_UpdateSystem = new UpdateSystem();

        private ObjectPooler m_ObjectPooler = new ObjectPooler();

        private static Main m_Singleton;

        public static Main Singleton
        {
            get { return m_Singleton; }
        }

        public void RegisterInstance(IInstance instance)
        {
            if (instance == null)
            {
                Debug.LogError("Can't register instance! Instance is null!");
                return;
            }

            Type type = instance.GetType();
            if (!m_Instances.ContainsKey(type))
            {
                m_Instances.Add(type, instance);
            }
        }

        public void ExcludeInstance(IInstance instance)
        {
            if (instance == null)
            {
                Debug.LogError("Can't exclude instance! Instance is null!");
                return;
            }

            Type type = instance.GetType();
            if (Main.Singleton.m_Instances.ContainsKey(type))
            {
                Main.Singleton.m_Instances.Remove(type);
            }
        }

        private void Awake()
        {
            if (m_Singleton != null)
            {
                Debug.LogWarning("Copy of Main was found and automatically destroyed!");

                Destroy(gameObject);

                return;
            }

            m_Singleton = this;

            DontDestroyOnLoad(gameObject);

            // Register instances.

            RegisterInstance(m_UpdateSystem);
            RegisterInstance(m_ObjectPooler);
        }

        private void OnDestroy()
        {
            // Exclude instances.

            ExcludeInstance(m_UpdateSystem);
            ExcludeInstance(m_ObjectPooler);
        }

        private void Start()
        {
            Update();
        }

        private void Update()
        {
            m_UpdateSystem.ListenForScreenResolutionUpdate();

            if (m_UpdateSystem.m_OnUpdate != null)
            {
                m_UpdateSystem.m_OnUpdate.Invoke();
            }

            if (m_UpdateSystem.m_OnLateUpdate != null)
            {
                m_UpdateSystem.m_OnLateUpdate.Invoke();
            }

            if (Input.GetKeyDown(KeyCode.Escape)) // TODO: for testing only
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }
        }

        private void FixedUpdate()
        {
            if (m_UpdateSystem.m_OnFixedUpdate != null)
            {
                m_UpdateSystem.m_OnFixedUpdate.Invoke();
            }
        }

        public static T Get<T>()
        {
            IInstance value;
            m_Singleton.m_Instances.TryGetValue(typeof(T), out value);

            return (T)value;
        }
    }
}
