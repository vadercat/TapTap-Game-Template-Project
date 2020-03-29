using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS0649

namespace TapTap
{
    public class Entity : MonoBehaviour
    {
        [ReadOnlyAttribute]
        [SerializeField]
        private string m_EntityName;

        private Transform m_Transform;

        public string EntityName { get { return m_EntityName; } }

        public const string Tag = "Entity";

        public Transform Transform
        {
            get
            {
                if (m_Transform == null)
                {
                    m_Transform = GetComponent<Transform>();
                }

                return m_Transform;
            }
        }

        private void Awake()
        {
            if (string.IsNullOrEmpty(m_EntityName))
            {
                Debug.LogWarning("<Entity Name> is null!", this);
            }

            tag = Tag;

            if (Main.Singleton != null)
            {
                if (this is IInstance)
                {
                    Main.Singleton.RegisterInstance(this as IInstance);
                }
            }

            if (this is IAwakeEvent)
            {
                (this as IAwakeEvent).OnAwakeEvent();
            }
        }

        private void OnDestroy()
        {
            if (Main.Singleton != null)
            {
                if (this is IInstance)
                {
                    Main.Singleton.ExcludeInstance(this as IInstance);
                }

                if (this is IPoolable)
                {
                    Main.Get<ObjectPooler>().TryRemoveFromCollection(m_EntityName, this as IPoolable);
                }
            }

            if (this is IDestroyEvent)
            {
                (this as IDestroyEvent).OnDestroyEvent();
            }
        }

        private void OnEnable()
        {
            if (Main.Singleton != null)
            {
                RegisterEvents();
            }

            if (this is IEnableEvent)
            {
                (this as IEnableEvent).OnEnableEvent();
            }
        }

        private void OnDisable()
        {
            if (Main.Singleton != null)
            {
                ExcludeEvents();
            }

            if (this is IDisableEvent)
            {
                (this as IDisableEvent).OnDisableEvent();
            }
        }

        private void RegisterEvents()
        {
            if (this is IUpdateEvent)
            {
                Main.Get<UpdateSystem>().m_OnUpdate += (this as IUpdateEvent).OnUpdateEvent;
            }

            if (this is ILateUpdateEvent)
            {
                Main.Get<UpdateSystem>().m_OnLateUpdate += (this as ILateUpdateEvent).OnLateUpdateEvent;
            }

            if (this is IFixedUpdateEvent)
            {
                Main.Get<UpdateSystem>().m_OnFixedUpdate += (this as IFixedUpdateEvent).OnFixedUpdateEvent;
            }

            if (this is IScreenResolutionChangedEvent)
            {
                Main.Get<UpdateSystem>().m_OnScreenResolutionChanged += (this as IScreenResolutionChangedEvent).OnScreenResolutionChangedEvent;
            }

            if (this is ISceneResetEvent)
            {
                Main.Get<UpdateSystem>().m_OnSceneReset += (this as ISceneResetEvent).OnSceneResetEvent;
            }

            if (this is ISceneLateResetEvent)
            {
                Main.Get<UpdateSystem>().m_OnSceneLateReset += (this as ISceneLateResetEvent).OnSceneLateResetEvent;
            }
        }

        private void ExcludeEvents()
        {
            if (this is IUpdateEvent)
            {
                Main.Get<UpdateSystem>().m_OnUpdate -= (this as IUpdateEvent).OnUpdateEvent;
            }

            if (this is ILateUpdateEvent)
            {
                Main.Get<UpdateSystem>().m_OnLateUpdate -= (this as ILateUpdateEvent).OnLateUpdateEvent;
            }

            if (this is IFixedUpdateEvent)
            {
                Main.Get<UpdateSystem>().m_OnFixedUpdate -= (this as IFixedUpdateEvent).OnFixedUpdateEvent;
            }

            if (this is IScreenResolutionChangedEvent)
            {
                Main.Get<UpdateSystem>().m_OnScreenResolutionChanged -= (this as IScreenResolutionChangedEvent).OnScreenResolutionChangedEvent;
            }

            if (this is ISceneResetEvent)
            {
                Main.Get<UpdateSystem>().m_OnSceneReset -= (this as ISceneResetEvent).OnSceneResetEvent;
            }

            if (this is ISceneLateResetEvent)
            {
                Main.Get<UpdateSystem>().m_OnSceneLateReset -= (this as ISceneLateResetEvent).OnSceneLateResetEvent;
            }
        }
    }
}
