using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TapTap
{
    public class CoinSpawnMethodSequence : CoinSpawnMethod
    {
        [SerializeField]
        private int m_CoinSpawnOrderCount = 5;

        private int m_TileLocalIndex;

        private int m_TilesCount;

        public override bool CanSpawnCoin()
        {
            int blockIndex = (m_TilesCount / m_CoinSpawnOrderCount) % m_CoinSpawnOrderCount;

            m_TilesCount++;
            m_TileLocalIndex = (m_TileLocalIndex + 1) % m_CoinSpawnOrderCount;

            return blockIndex == m_TileLocalIndex;
        }
    }
}
