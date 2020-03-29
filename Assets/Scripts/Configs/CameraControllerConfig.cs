using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS0649

namespace TapTap
{
    [CreateAssetMenu]
    public class CameraControllerConfig : Config
    {
        [SerializeField]
        private float m_HorizontalSpace = 10.0f;

        [SerializeField]
        private float m_CameraOffset = -100.0f;

        public float HorizontalSpace { get { return m_HorizontalSpace; } }

        public float CameraOffset { get { return m_CameraOffset; } }
    }
}
