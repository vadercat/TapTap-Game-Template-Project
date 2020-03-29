using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS0649

namespace TapTap
{
    [CreateAssetMenu]
    public class TileConfig : Config
    {
        [SerializeField]
        private float m_TileFallingTimeLength = 1.0f;

        [SerializeField]
        private float m_TileMinPosition = -15.0f;

        [SerializeField]
        private AnimationCurve m_TilePositionCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);

        public float TileFallingTimeLength { get { return m_TileFallingTimeLength; } }

        public float TileMinPosition { get { return m_TileMinPosition; } }

        public AnimationCurve TilePositionCurve { get { return m_TilePositionCurve; } }
    }
}
