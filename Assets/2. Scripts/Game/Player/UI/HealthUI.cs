using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EscapeGame
{
    public class HealthUI : SystemUI
    {
        [SerializeField] [Range(0,1)] float size = 0.1f;

        [SerializeField] [Range(0, 1)] float m_coefficient = 0.5f;

        private void Start()
        {
            Action();
        }

        protected override void Action(float amount = 0)
        {
            StartCoroutine(Activate());
        }

        private IEnumerator Activate()
        {
            bool isExpand = true;
            float ratio = 1.0f;

            while(!m_PlayerUI.Character.isDied)
            {
                if(isExpand)
                {
                    transform.localScale = new Vector3(transform.localScale.x + Time.deltaTime * m_coefficient,
                                                       transform.localScale.y + Time.deltaTime * m_coefficient);

                    if(transform.localScale.x > ratio + size)
                    {
                        // ratio 갱신
                        ratio = m_PlayerUI.Character.health.Hp / m_PlayerUI.Character.health.MaxHp;

                        isExpand = false;
                    }
                }
                else
                {
                    transform.localScale = new Vector3(transform.localScale.x - Time.deltaTime * m_coefficient,
                                                       transform.localScale.y - Time.deltaTime * m_coefficient);

                    if(transform.localScale.y < ratio + size * 0.5f)
                    {
                        // ratio 갱신
                        ratio = m_PlayerUI.Character.health.Hp / m_PlayerUI.Character.health.MaxHp;

                        isExpand = true;

                        if(ratio == 0.0f)
                        {
                            // Die
                            m_PlayerUI.Character.ExhaustPhysicalStrength();

                            yield return new WaitForSeconds(1.0f);

                            GameManager.s_Instance.GameEnd();
                        }
                    }

                }

                yield return null;
            }
        }
    }
}