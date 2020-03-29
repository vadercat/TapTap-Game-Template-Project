using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS0649

namespace TapTap
{
    public class Ball : Entity, IEnableEvent, IDisableEvent, IUpdateEvent, ISceneResetEvent
    {
        [SerializeField]
        private bool m_Bot;

        [Header("Render")]

        [SerializeField]
        private Transform m_Renderer;

        private int m_MoveDirectionIndex;

        private Vector3 m_FallingVelocity;

        private Collider[] m_Colliders = new Collider[10];

        private float BallSpeed
        {
            get { return Main.Get<BallConfig>().BallSpeed; }
        }

        private float BallRadius
        {
            get { return Main.Get<BallConfig>().BallRadius; }
        }

        private readonly Vector3[] m_MoveDirections =
        {
            Vector3.right,
            Vector3.forward
        };

        public void OnEnableEvent()
        {
            UpdateRenderer();

            Main.Get<GameLogic>().m_OnMouseClick += ChangeMoveDirection;
        }

        public void OnDisableEvent()
        {
            if (Main.Get<GameLogic>() != null)
            {
                Main.Get<GameLogic>().m_OnMouseClick -= ChangeMoveDirection;
            }
        }

        private void ChangeMoveDirection()
        {
            m_MoveDirectionIndex = (m_MoveDirectionIndex + 1) % m_MoveDirections.Length;
        }

        public void OnUpdateEvent()
        {
            if (!Main.Get<GameLogic>().IsGameStarted)
            {
                UpdateRenderer(); // HACK: Scene Reset

                return;
            }

            if (Main.Get<GameLogic>().IsGameOver)
            {
                if (Transform.position.y < -1000.0f) // TODO: Min height position
                {
                    return;
                }

                m_FallingVelocity += Physics.gravity * Time.deltaTime;
                Transform.position += m_FallingVelocity * Time.deltaTime;
            }
            else
            {
                CollisionCheck();

                if (!IsOnGround(GetBallCenterPosition()))
                {
                    Main.Get<GameLogic>().SetGameOver();
                }
            }

            Transform.position += m_MoveDirections[m_MoveDirectionIndex] * BallSpeed * Time.deltaTime;

            UpdateRenderer();

            if (m_Bot)
            {
                BotLogic();
            }
        }

        private void BotLogic()
        {
            if (!IsOnGround(GetBallCenterPosition() + m_MoveDirections[m_MoveDirectionIndex] * Main.Get<LevelGeneratorConfig>().TileSize * 0.5f))
            {
                ChangeMoveDirection();
            }
        }

        private void UpdateRenderer()
        {
            m_Renderer.localPosition = new Vector3(0.0f, BallRadius, 0.0f);
            m_Renderer.localScale = Vector3.one * BallRadius * 2.0f;
        }

        private Vector3 GetBallCenterPosition()
        {
            return Transform.position + Vector3.up * BallRadius;
        }

        private bool IsOnGround(Vector3 origin)
        {
            Ray ray = new Ray(origin, Vector3.down);

            return Physics.Raycast(ray, Main.Get<BallConfig>().GroundLayer);
        }

        private void CollisionCheck()
        {
            int count = Physics.OverlapSphereNonAlloc(GetBallCenterPosition(), BallRadius, m_Colliders);

            for (int i = 0; i < count; i++)
            {
                if (m_Colliders[i].CompareTag(Entity.Tag))
                {
                    ICollectable collectable = m_Colliders[i].GetComponent<ICollectable>();

                    if (collectable != null)
                    {
                        collectable.Collect();
                    }
                }
            }
        }

        public void OnSceneResetEvent()
        {
            Transform.position = Vector3.zero;

            m_MoveDirectionIndex = 0;

            m_FallingVelocity = Vector3.zero;
        }
    }
}
