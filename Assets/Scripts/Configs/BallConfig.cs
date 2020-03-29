using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS0649

namespace TapTap
{
    [CreateAssetMenu]
    public class BallConfig : Config
    {
        [SerializeField]
        private float m_BallSpeed = 5.0f;

        [SerializeField]
        private float m_BallRadius = 0.25f;

        [SerializeField]
        private LayerMask m_GroundLayer;

        public float BallSpeed { get { return m_BallSpeed; } }

        public float BallRadius { get { return m_BallRadius; } }

        public LayerMask GroundLayer { get { return m_GroundLayer; } }
    }
}
