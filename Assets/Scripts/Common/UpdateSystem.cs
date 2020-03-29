using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TapTap
{
    public class UpdateSystem : IInstance
    {
        public UnityAction m_OnUpdate;

        public UnityAction m_OnLateUpdate;

        public UnityAction m_OnFixedUpdate;

        public UnityAction m_OnScreenResolutionChanged;

        public UnityAction m_OnSceneReset;

        public UnityAction m_OnSceneLateReset;

        private int m_LastScreenWidth;

        private int m_LastScreenHeight;

        public void ListenForScreenResolutionUpdate()
        {
            if (Screen.width != m_LastScreenWidth)
            {
                UpdateScreenResolution();
            }
            else if (Screen.height != m_LastScreenHeight)
            {
                UpdateScreenResolution();
            }
        }

        private void UpdateScreenResolution()
        {
            m_LastScreenWidth = Screen.width;
            m_LastScreenHeight = Screen.height;

            if (m_OnScreenResolutionChanged != null)
            {
                m_OnScreenResolutionChanged.Invoke();
            }
        }

        public void ResetScene()
        {
            if (m_OnSceneReset != null)
            {
                m_OnSceneReset.Invoke();
            }

            if (m_OnSceneLateReset != null)
            {
                m_OnSceneLateReset.Invoke();
            }
        }
    }
}
