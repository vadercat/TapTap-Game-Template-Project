using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TapTap
{
    [DefaultExecutionOrder(ExecutionOrder.GameLogic)]
    public class GameLogic : Entity, IInstance, ISceneResetEvent
    {
        private bool m_IsGameStarted = false;

        private bool m_IsGameOver = false;

        private int m_Score;

        // Events.

        public UnityAction m_OnMouseClick;

        public UnityAction m_OnGameStarted;

        public UnityAction m_OnGameOver;

        public UnityAction m_OnScoreChanged;

        // Properties.

        public bool IsGameStarted { get { return m_IsGameStarted; } }

        public bool IsGameOver { get { return m_IsGameOver; } }

        public int Score { get { return m_Score; } }

        public void SetGameOver()
        {
            if (m_IsGameOver)
            {
                return;
            }

            m_IsGameOver = true;

            if (m_OnGameOver != null)
            {
                m_OnGameOver.Invoke();
            }
        }

        public void DoMouseClick()
        {
            if (!m_IsGameStarted)
            {
                m_IsGameStarted = true;

                if (m_OnGameStarted != null)
                {
                    m_OnGameStarted.Invoke();
                }

                return;
            }

            if (m_IsGameOver)
            {
                return;
            }

            if (m_OnMouseClick != null)
            {
                m_OnMouseClick.Invoke();
            }
        }

        public void DoReset()
        {
            Main.Get<UpdateSystem>().ResetScene();
        }

        public void OnCollectCoin()
        {
            m_Score++;

            if (m_OnScoreChanged != null)
            {
                m_OnScoreChanged.Invoke();
            }
        }

        public void OnSceneResetEvent()
        {
            m_IsGameStarted = false;

            m_IsGameOver = false;

            m_Score = 0;

            if (m_OnScoreChanged != null)
            {
                m_OnScoreChanged.Invoke();
            }
        }
    }
}
