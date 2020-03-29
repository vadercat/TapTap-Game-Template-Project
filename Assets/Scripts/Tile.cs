using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TapTap
{
    public class Tile : Entity, IEnableEvent, IDisableEvent, IDisappear, IPoolable, IUpdateEvent
    {
        private bool m_Falling;

        private float m_VerticalLerp = 1.0f;

        public void OnEnableEvent()
        {
            Main.Get<LevelGenerator>().RegisterEntity(this);
        }

        public void OnDisableEvent()
        {
            if (Main.Get<LevelGenerator>() != null)
            {
                Main.Get<LevelGenerator>().ExcludeEntity(this);
            }
        }

        public void Disappear()
        {
            if (m_Falling)
            {
                return;
            }

            m_Falling = true;
        }

        public void OnUpdateEvent()
        {
            if (m_Falling)
            {
                if (m_VerticalLerp > 0.0f)
                {
                    m_VerticalLerp = Mathf.Clamp01(m_VerticalLerp - Time.deltaTime / Main.Get<TileConfig>().TileFallingTimeLength);
                    VisualUpdate();

                    if (m_VerticalLerp == 0.0f)
                    {
                        ObjectPooler.Push(EntityName, this);
                    }
                }
            }
            else
            {
                if (m_VerticalLerp < 1.0f)
                {
                    m_VerticalLerp = Mathf.Clamp01(m_VerticalLerp + Time.deltaTime / Main.Get<TileConfig>().TileFallingTimeLength);
                    VisualUpdate();
                }
            }
        }

        private void VisualUpdate()
        {
            Vector3 position = Transform.position;
            position.y = Mathf.Lerp(Main.Get<TileConfig>().TileMinPosition, 0.0f, Main.Get<TileConfig>().TilePositionCurve.Evaluate(m_VerticalLerp));
            Transform.position = position;
        }

        private void Reset()
        {
            m_Falling = false;
            m_VerticalLerp = 1.0f;

            VisualUpdate();
        }

        public void OnPoolPopEvent()
        {
            gameObject.SetActive(true);

            Reset();
        }

        public void OnPoolPushEvent()
        {
            gameObject.SetActive(false);

            Reset();
        }
    }
}
