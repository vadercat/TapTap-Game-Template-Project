using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TapTap
{
    public class Config : ScriptableObject, IInstance
    {
        public UnityAction m_OnConfigChanged;

#if UNITY_EDITOR
        private void OnValidate()
        {
            OnConfigChanged();
        }
#endif

        public void OnConfigChanged()
        {
            if (m_OnConfigChanged != null)
            {
                m_OnConfigChanged.Invoke();
            }
        }
    }
}
