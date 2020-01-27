using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EscapeGame
{
    /// <summary>
    /// 연기에 의해 발생하는 UI
    /// </summary>
    public class InhaleSmokeUI : SystemUI
    {
        public byte currentLevel { get; set; }

        private Slider m_slider;

        [SerializeField] float m_coefficient = 1.0f;

        protected override void Awake()
        {
            base.Awake();

            m_slider = GetComponentInChildren<Slider>();
            m_slider.gameObject.SetActive(false);

            Action();
        }
            
        protected override void Action(float amount = 0)
        {
            StartCoroutine(Activate());
        }

        private IEnumerator Activate()
        {
            while(!m_PlayerUI.Character.isDied)
            {
                // 연기레벨이 1이상일 경우
                if (currentLevel > 1)
                {
                    if (!m_slider.gameObject.activeSelf)
                        m_slider.gameObject.SetActive(true);

                    if (!m_PlayerUI.Character.IsBending)
                    {
                        m_slider.value += Time.deltaTime
                                            * Mathf.Pow(currentLevel, currentLevel)
                                            * m_coefficient * 0.5f;
                    }
                    else
                    {
                        m_slider.value += Time.deltaTime
                                            * Mathf.Pow(currentLevel, currentLevel)
                                            * m_coefficient * 0.15f;
                    }

                    if (m_slider.value >= m_slider.maxValue)
                    {
                        // 슬라이더의 값을 최소값으로 초기화
                        m_slider.value = m_slider.minValue;

                        // 플레이어에게 연기 데미지를 입힌다.
                        m_PlayerUI.Character.health.Damaged(Particle.Smoke.Damage);
                    }
                }
                // 연기레벨이 1미만일 경우에
                else
                {
                    // 슬라이더가 활성화되어 있으며
                    if(m_slider.gameObject.activeSelf)
                    {
                        // 슬라이더의 값이 최소값이랑 다를 경우에
                        if(m_slider.value != m_slider.minValue)
                        {
                            m_slider.value -= Time.deltaTime * m_coefficient;
                        }
                        // 같을 경우에
                        else
                        {
                            // 비활성화 시켜준다.
                            m_slider.gameObject.SetActive(false);
                        }
                    }
                }

                yield return null;
            }

            m_slider.gameObject.SetActive(false);
        }
    }
}