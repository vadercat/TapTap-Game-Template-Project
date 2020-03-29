using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TapTap
{
    public class CoinSpawnMethodRandom : CoinSpawnMethod
    {
        [SerializeField]
        [Range(0.0f, 1.0f)]
        private float m_CoinSpawnChance = 0.5f;

        public override bool CanSpawnCoin()
        {
            return Random.value <= m_CoinSpawnChance;
        }
    }
}
