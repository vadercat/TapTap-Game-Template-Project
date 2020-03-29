using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TapTap
{
    public abstract class CoinSpawnMethod : MonoBehaviour
    {
        public abstract bool CanSpawnCoin();
    }
}
