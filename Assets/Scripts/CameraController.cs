using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS0649

namespace TapTap
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : Entity, IAwakeEvent, ILateUpdateEvent, ISceneResetEvent, IScreenResolutionChangedEvent
    {
        [SerializeField]
        private Transform m_Target;

        private Camera m_Camera;

        private Quaternion m_MatrixRotation;

        private Matrix4x4 m_Matrix;

        public void OnAwakeEvent()
        {
            m_Camera = GetComponent<Camera>();

            m_MatrixRotation = Quaternion.Euler(new Vector3(0.0f, 45.0f, 0.0f));
            m_Matrix = Matrix4x4.TRS(Vector3.zero, m_MatrixRotation, Vector3.one);

            Transform.rotation = Quaternion.Euler(new Vector3(35.26439f, 45.0f, 0.0f));
            UpdateOrthographicSize();
        }

        public void OnLateUpdateEvent()
        {
            if (Main.Get<GameLogic>().IsGameOver)
            {
                return;
            }

            if (m_Target != null)
            {
                Vector3 cameraTargetPosition = m_Matrix.inverse.MultiplyPoint3x4(m_Target.position);
                cameraTargetPosition = m_MatrixRotation * new Vector3(0.0f, cameraTargetPosition.y, cameraTargetPosition.z);

                Transform.position = Transform.rotation * new Vector3(0.0f, 0.0f, Main.Get<CameraControllerConfig>().CameraOffset) + cameraTargetPosition;
            }
        }

        public void OnSceneResetEvent()
        {
            UpdateOrthographicSize(); // HACK: Scene Reset
        }

        public void OnScreenResolutionChangedEvent()
        {
            UpdateOrthographicSize();
        }

        private void UpdateOrthographicSize()
        {
            m_Camera.orthographicSize = Main.Get<CameraControllerConfig>().HorizontalSpace / 2.0f / m_Camera.aspect;
        }

#if UNITY_EDITOR
        private static readonly Vector2[] m_ViewportPoints = new Vector2[]
        {
            new Vector2(0.0f, 0.0f),
            new Vector2(0.0f, 1.0f),
            new Vector2(1.0f, 0.0f),
            new Vector2(1.0f, 1.0f),

            new Vector2(0.0f, 0.0f),
            new Vector2(1.0f, 0.0f),
            new Vector2(0.0f, 1.0f),
            new Vector2(1.0f, 1.0f)
        };

        private void OnDrawGizmos()
        {
            if (m_Camera == null)
            {
                m_Camera = GetComponent<Camera>();
            }

            Plane plane = new Plane(Vector3.up, Vector3.zero);

            Vector3 forward = Transform.forward;
            Gizmos.color = Color.red;

            for (int i = 0; i < m_ViewportPoints.Length; i += 2)
            {
                Ray rayStart = new Ray(m_Camera.ViewportToWorldPoint(m_ViewportPoints[i]), forward);
                Ray rayFinish = new Ray(m_Camera.ViewportToWorldPoint(m_ViewportPoints[i + 1]), forward);

                Vector3? lineStart = null;
                Vector3? lineFinish = null;

                float distanceStart;
                if (plane.Raycast(rayStart, out distanceStart))
                {
                    lineStart = rayStart.GetPoint(distanceStart);
                }

                float distanceFinish;
                if (plane.Raycast(rayFinish, out distanceFinish))
                {
                    lineFinish = rayFinish.GetPoint(distanceFinish);
                }

                if (lineStart != null && lineFinish != null)
                {
                    Gizmos.DrawLine(lineStart.Value, lineFinish.Value);
                }
            }
        }
#endif
    }
}
