using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS0649

namespace TapTap
{
    [DefaultExecutionOrder(ExecutionOrder.LevelGenerator)]
    public class LevelGenerator : Entity, IInstance, IAwakeEvent, IFixedUpdateEvent, ISceneLateResetEvent
    {
        [SerializeField]
        private Transform m_Target;

        private Matrix4x4 m_Matrix;

        private Vector2 m_LeftRightBounds;

        private Vector3 m_LastTilePosition;

        private bool m_IsStartTileCreated;

        private bool m_IsForwardDirectionNow;

        private CoinSpawnMethod m_CoinSpawnMethod;

        private List<Entity> m_Entities = new List<Entity>();

        private List<Entity> m_TempPushList = new List<Entity>();

        private int TileSize
        {
            get { return Main.Get<LevelGeneratorConfig>().TileSize; }
        }

        private int StartTileSize
        {
            get { return Main.Get<LevelGeneratorConfig>().StartTileSize; }
        }

        public void RegisterEntity(Entity entity)
        {
            if (entity == null)
            {
                Debug.LogError("Can't register entity! Entity is null!");
                return;
            }

            if (!m_Entities.Contains(entity) && entity is IPoolable)
            {
                m_Entities.Add(entity);
            }
            else
            {
                Debug.LogError(string.Format("Entity {0} is not poolable!", entity), entity);
            }
        }

        public void ExcludeEntity(Entity entity)
        {
            int index = m_Entities.IndexOf(entity);
            if (index != -1)
            {
                m_Entities.RemoveAt(index);
            }
        }

        public void OnAwakeEvent()
        {
            m_CoinSpawnMethod = GetComponent<CoinSpawnMethod>();

            m_Matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(new Vector3(0.0f, 45.0f, 0.0f)), Vector3.one);

            float boundSize = Main.Get<CameraControllerConfig>().HorizontalSpace / 2.0f;
            m_LeftRightBounds = new Vector2(-boundSize, boundSize);

            Generate();
        }

        private void CleanScene()
        {
            for (int i = 0; i < m_Entities.Count; i++)
            {
                m_TempPushList.Add(m_Entities[i]);
            }

            PushEntitiesToPool();
        }

        private void Generate()
        {
            SpawnTile();
            m_IsStartTileCreated = true;

            m_LastTilePosition = new Vector3(StartTileSize / 2.0f + TileSize / 2.0f, 0.0f, 0.0f);
            SpawnTile();

            TryCreateTileLines();
        }

        private void Reset()
        {
            m_LastTilePosition = Vector3.zero;
            m_IsStartTileCreated = false;
            m_IsForwardDirectionNow = false;
        }

        public void OnFixedUpdateEvent()
        {
            if (Main.Get<GameLogic>().IsGameOver)
            {
                return;
            }

            TryCreateTileLines();

            // Destroy objects behind.

            float targetZ = m_Matrix.inverse.MultiplyPoint3x4(m_Target.position).z;
            float bufferZ = targetZ - Main.Get<LevelGeneratorConfig>().BufferPosition;
            float disappearZ = targetZ - Main.Get<LevelGeneratorConfig>().DisappearPosition;

            for (int i = 0; i < m_Entities.Count; i++)
            {
                float posZ = m_Matrix.inverse.MultiplyPoint3x4(m_Entities[i].Transform.position).z;

                if (m_Entities[i] is IDisappear && posZ <= disappearZ)
                {
                    (m_Entities[i] as IDisappear).Disappear();
                }

                if (posZ <= bufferZ)
                {
                    m_TempPushList.Add(m_Entities[i]);
                }
            }

            PushEntitiesToPool();
        }

        private void PushEntitiesToPool()
        {
            for (int i = 0; i < m_TempPushList.Count; i++)
            {
                ObjectPooler.Push(m_TempPushList[i].EntityName, m_TempPushList[i] as IPoolable);
            }

            m_TempPushList.Clear();
        }

        private void TryCreateTileLines()
        {
            float targetZ = m_Matrix.inverse.MultiplyPoint3x4(m_Target.position).z + Main.Get<LevelGeneratorConfig>().BufferPosition;

            while (m_Matrix.inverse.MultiplyPoint3x4(m_LastTilePosition).z <= targetZ)
            {
                CreateTileLine();
            }
        }

        private void CreateTileLine()
        {
            int count = CalculateCountForNextLine();

            Vector3 tempPos = m_LastTilePosition;
            for (int i = 1; i < count + 1; i++)
            {
                m_LastTilePosition = m_IsForwardDirectionNow ? tempPos + Vector3.forward * i * TileSize : tempPos + Vector3.right * i * TileSize;
                SpawnTile();
            }

            m_IsForwardDirectionNow = !m_IsForwardDirectionNow;
        }

        private int CalculateCountForNextLine()
        {
            int maxCount = 0;

            for (int i = 1; i < Main.Get<LevelGeneratorConfig>().MaxTilesPerDirection + 1; i++)
            {
                Vector3 checkPosition = m_LastTilePosition + (m_IsForwardDirectionNow ? Vector3.forward : Vector3.right) * i * TileSize;

                if (IsOutOfBound(checkPosition))
                {
                    break;
                }
                else
                {
                    maxCount = i;
                }
            }

            if (maxCount < 1)
            {
                return 0;
            }

            return Random.Range(1, maxCount + 1);
        }

        private GameObject SpawnEntity(Entity entityPrefab)
        {
            Entity entity = ObjectPooler.Pop<Entity>(entityPrefab.EntityName);

            if (entity != null)
            {
                return entity.gameObject;
            }
            else
            {
                GameObject entityObject = Instantiate(entityPrefab.gameObject);
#if UNITY_EDITOR
                entityObject.name = System.Guid.NewGuid().ToString();
#endif
                return entityObject;
            }
        }

        private void SpawnTile()
        {
            m_LastTilePosition.y = 0.0f;

            GameObject tileObject = SpawnEntity(Main.Get<LevelGeneratorConfig>().TilePrefab);

            Transform tileTransform = tileObject.GetComponent<Transform>();
            tileTransform.position = m_LastTilePosition;
            tileTransform.rotation = Quaternion.identity;
            tileTransform.localScale = m_IsStartTileCreated ? new Vector3(TileSize, 1.0f, TileSize) : new Vector3(StartTileSize, 1.0f, StartTileSize);

            if (m_IsStartTileCreated)
            {
                SpawnCoin();
            }
        }

        private void SpawnCoin()
        {
            if (m_CoinSpawnMethod != null && m_CoinSpawnMethod.CanSpawnCoin())
            {
                GameObject coinObject = SpawnEntity(Main.Get<LevelGeneratorConfig>().CoinPrefab);

                Transform coinTransform = coinObject.GetComponent<Transform>();
                coinTransform.position = m_LastTilePosition;
                coinTransform.rotation = Quaternion.identity;
            }
        }

        private bool IsOutOfBound(Vector3 position)
        {
            if (m_Matrix.inverse.MultiplyPoint3x4(position + new Vector3(-TileSize / 2.0f, 0.0f, TileSize / 2.0f)).x < m_LeftRightBounds.x)
            {
                return true;
            }

            if (m_Matrix.inverse.MultiplyPoint3x4(position + new Vector3(TileSize / 2.0f, 0.0f, -TileSize / 2.0f)).x > m_LeftRightBounds.y)
            {
                return true;
            }

            return false;
        }

        public void OnSceneLateResetEvent()
        {
            Reset();

            CleanScene();

            Generate();
        }
    }
}
