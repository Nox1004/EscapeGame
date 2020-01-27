using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace EscapeGame
{
    public class PlayTimer : PersistantSingleton<PlayTimer>
    {
        /// <summary>
        /// 경과시간
        /// </summary>
        public float elapsedTime { get; private set; }

        [SerializeField] [Range(0.1f, 10)] float m_coefficent = 1.0f;

        /// <summary>
        /// 초기화 함수 (시작 및 다시 시작할 경우에 호출)
        /// </summary>
        public void Initialize()
        {
            if (elapsedTime != 0.0f)
                elapsedTime = 0.0f;
        }

        private void Update()
        {
            if(GameManager.s_Instance.isPlaying)
                elapsedTime += Time.deltaTime * m_coefficent;
        }
    }
}
