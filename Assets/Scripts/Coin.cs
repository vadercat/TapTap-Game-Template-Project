using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TapTap
{
    public class Coin : Entity, ICollectable, IEnableEvent, IDisableEvent, IDisappear, IPoolable
    {
        public void OnEnableEvent()
        {
            Main.Get<LevelGenerator>().RegisterEntity(this);
        }

        public void OnDisableEvent()
        {
            if (Main.Get<LevelGenerator>() != null)
            {
                Main.Get<LevelGenerator>().ExcludeEntity(this);
            }
        }

        public void OnPoolPopEvent()
        {
            gameObject.SetActive(true);
        }

        public void OnPoolPushEvent()
        {
            gameObject.SetActive(false);
        }

        public void Collect()
        {
            Main.Get<GameLogic>().OnCollectCoin();

            ObjectPooler.Push(EntityName, this);
        }

        public void Disappear()
        {
            ObjectPooler.Push(EntityName, this);
        }
    }
}
