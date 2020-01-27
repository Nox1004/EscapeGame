using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

namespace EscapeGame
{
    /// <summary>
    /// GlobalFog 및 연기 위험도를 나타내는 클래스
    /// </summary>
    public class FogUI : SystemUI
    {
        [SerializeField] private Text m_ContextsText = null;
        [SerializeField] private Text m_RiskText = null;

        private GlobalFog m_globalFog;
        
        private byte FogLevel;
        private EFade cntFade;                  // In --> Fog densely   Out --> Fog Not densely

        [SerializeField] float m_coefficent = 1.0f;    // 계수
        float defaultValue = 0.05f;

        public bool isActivated { get; private set; }

        public void Activate(EFade fade, byte step = 0)
        {
            cntFade = fade;
            FogLevel = step;

            if (FogLevel == 0 && fade == EFade.Out) {
                TextEnabled(false);
                return;
            }

            if (!m_ContextsText.enabled)
                TextEnabled(true);
            else
                isActivated = true;

            // 1 --> 낮음 color -- 진한 노랑
            // 2 --> 높음 color -- 주황
            // 3 --> 매우 높음 color -- 붉은색
            if(FogLevel == 1)
            {
                m_RiskText.text = "낮음";
                m_RiskText.color = new Color(227, 239, 0);
            }
            else
            {
                m_RiskText.text = (FogLevel == 2) ? "높음" : "매우 높음";
                m_RiskText.color = (FogLevel == 2) ? new Color(255, 127, 0) : Color.red;
            }
        }

        protected override void Awake()
        {
            base.Awake();

#if UNITY_EDITOR
            if(m_ContextsText == null || m_RiskText == null)
            {
                UnityEditor.EditorApplication.isPlaying = false;
                Debug.Log("FogUI 컴포넌트 할당을 제대로 해주세요.");
            }
#endif
            m_ContextsText.enabled = false;
            m_RiskText.enabled = false;
        }

        private void Start()
        {
            Action();
        }

        protected override void Action(float amount = 0)
        {
            m_globalFog = m_PlayerUI.canvas.worldCamera.GetComponent<GlobalFog>();
            m_globalFog.heightDensity = defaultValue;
        }

        private void Update()
        {
            if (isActivated)
            {
                float amount = (FogLevel - 1) * 0.5f + defaultValue;

                if (cntFade == EFade.In)
                {
                    if (m_globalFog.heightDensity < amount)
                    {
                        m_globalFog.heightDensity += Time.deltaTime * m_coefficent;

                        if(m_globalFog.heightDensity > amount)
                        {
                            m_globalFog.heightDensity = amount;
                            isActivated = false;
                        }
                    }
                }
                else
                {
                    if (FogLevel == 0)
                        amount = defaultValue;

                    if(m_globalFog.heightDensity > amount)
                    {
                        m_globalFog.heightDensity -= Time.deltaTime * m_coefficent;

                        if(m_globalFog.heightDensity <= amount)
                        {
                            m_globalFog.heightDensity = amount;
                            isActivated = false;
                        }
                    }
                }
            }
        }

        private void TextEnabled(bool isUsing)
        {
            m_ContextsText.enabled = isUsing;
            m_RiskText.enabled = isUsing;

            isActivated = true;
        }

        [ContextMenu("On01")]
        private void On01()
        {
            Activate(EFade.In, 2);
        }

        [ContextMenu("On02")]
        private void On02()
        {
            Activate(EFade.In, 3);
        }

        [ContextMenu("Off")]
        private void Off()
        {
            Activate(EFade.Out);
        }
    }
}