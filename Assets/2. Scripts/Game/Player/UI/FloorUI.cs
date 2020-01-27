using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EscapeGame
{
    public class FloorUI : SystemUI
    {
        private Text m_Text;

        public IEnumerator ShowCurrentFloor(float amount)
        {
            var clr = m_Text.color; clr.a = amount;
            m_Text.color = clr;

            m_Text.text = FloorMgr.s_Instance.currentFloor.ToString() + "층";

            yield return new WaitForSeconds(2.0f);

            yield return StartCoroutine(Fade(m_Text, EFade.In));
        }

        protected override void Action(float amount = 0)
        { 
            StartCoroutine(ShowCurrentFloor(amount));
        }

        protected override void Awake()
        {
            base.Awake();

            m_Text = GetComponent<Text>();

            // 알파값 0으로 초기화
            Color clr = m_Text.color;

            if (clr.a != 0)
            {
                clr.a = 0.0f;
                m_Text.color = clr;
            }
        }
    }
}
