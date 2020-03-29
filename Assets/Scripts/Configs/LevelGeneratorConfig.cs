using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS0649

namespace TapTap
{
    [CreateAssetMenu]
    public class LevelGeneratorConfig : Config
    {
        [SerializeField]
        private Tile m_TilePrefab;

        [SerializeField]
        private Coin m_CoinPrefab;

        [SerializeField]
        private int m_StartTileSize = 3;

        [SerializeField]
        private int m_TileSize = 1;

        [SerializeField]
        private int m_MaxTilesPerDirection = 5;

        [SerializeField]
        private float m_BufferPosition = 25.0f;

        [SerializeField]
        private float m_DisappearPosition = 5.0f;

        public Tile TilePrefab { get { return m_TilePrefab; } }

        public Coin CoinPrefab { get { return m_CoinPrefab; } }

        public int StartTileSize { get { return m_StartTileSize; } }

        public int TileSize { get { return m_TileSize; } }

        public int MaxTilesPerDirection { get { return m_MaxTilesPerDirection; } }

        public float BufferPosition { get { return m_BufferPosition; } }

        public float DisappearPosition { get { return m_DisappearPosition; } }
    }
}
