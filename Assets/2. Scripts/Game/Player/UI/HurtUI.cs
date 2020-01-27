using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EscapeGame
{
    [RequireComponent(typeof(Image))]
    public class HurtUI : SystemUI
    {
        Image m_Image;

        // 외부에서 접근할 수 있게 해주는 함수
        public void Activate(float amount = 0.0f)
        {
            Action(amount);
        }

        protected override void Action(float amount)
        {
            m_Image.color = new Color(1, 1, 1, Mathf.InverseLerp(1, 0, amount));
        }

        protected override void Awake()
        {
            base.Awake();

            m_Image = GetComponent<Image>();

            // Image Color 알파값을 0으로 한다.
            Action(1);
        }
    }
}