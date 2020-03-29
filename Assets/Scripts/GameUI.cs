using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable CS0649

namespace TapTap
{
    public class GameUI : Entity, IEnableEvent, IDisableEvent, ISceneResetEvent
    {
        [SerializeField]
        private GameObject m_ClickRaycastTarget;

        [SerializeField]
        private GameObject m_RestartRaycastTarget;

        [SerializeField]
        private float m_GameOverDelay = 1.0f;

        [SerializeField]
        private Text m_ScoreText;

        [SerializeField]
        private GameObject m_TapText;

        [SerializeField]
        private GameObject m_GameOverText;

        private string m_ScoreTextFormat;

        private IEnumerator m_GameOverCoroutine;

        private GameLogic GameLogic
        {
            get { return Main.Get<GameLogic>(); }
        }

        private void Start()
        {
            Reset();
        }

        public void OnEnableEvent()
        {
            GameLogic.m_OnScoreChanged += OnScore;

            GameLogic.m_OnGameStarted += OnGameStarted;

            GameLogic.m_OnGameOver += OnGameOver;
        }

        public void OnDisableEvent()
        {
            if (GameLogic != null)
            {
                GameLogic.m_OnScoreChanged -= OnScore;

                GameLogic.m_OnGameStarted -= OnGameStarted;

                GameLogic.m_OnGameOver -= OnGameOver;
            }
        }

        public void OnScore()
        {
            if (string.IsNullOrEmpty(m_ScoreTextFormat))
            {
                m_ScoreTextFormat = m_ScoreText.text;
            }

            m_ScoreText.text = string.Format(m_ScoreTextFormat, GameLogic.Score);
        }

        public void OnGameStarted()
        {
            m_TapText.SetActive(false);
        }

        public void OnGameOver()
        {
            m_ClickRaycastTarget.SetActive(false);

            StopGameOverCoroutine();

            m_GameOverCoroutine = GameOverCoroutine();
            StartCoroutine(m_GameOverCoroutine);
        }

        private void StopGameOverCoroutine() // TODO: to extension
        {
            if (m_GameOverCoroutine != null)
            {
                StopCoroutine(m_GameOverCoroutine);
                m_GameOverCoroutine = null;
            }
        }

        private IEnumerator GameOverCoroutine()
        {
            yield return new WaitForSeconds(m_GameOverDelay);

            m_GameOverText.SetActive(true);

            m_RestartRaycastTarget.SetActive(true);
        }

        public void OnSceneResetEvent()
        {
            Reset();
        }

        private void Reset()
        {
            OnScore();

            m_TapText.SetActive(true);

            m_ClickRaycastTarget.SetActive(true);

            StopGameOverCoroutine();

            m_GameOverText.SetActive(false);

            m_RestartRaycastTarget.SetActive(false);
        }
    }
}
