using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TapTap
{
    [RequireComponent(typeof(Graphic))]
    public class GraphicBlinker : Entity, IAwakeEvent, IUpdateEvent
    {
        private Color m_OriginalColor;

        private Color m_ClearColor;

        private Graphic m_Graphic;

        public void OnAwakeEvent()
        {
            m_Graphic = GetComponent<Graphic>();

            m_OriginalColor = m_Graphic.color;
            m_ClearColor = new Color(m_OriginalColor.r, m_OriginalColor.g, m_OriginalColor.b, 0.0f);
        }

        public void OnUpdateEvent()
        {
            float lerp = Mathf.Sin(Time.unscaledTime * Mathf.PI) * 0.5f + 0.5f;

            m_Graphic.color = Color.Lerp(m_ClearColor, m_OriginalColor, lerp);
        }
    }
}
