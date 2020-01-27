using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EscapeGame
{
    /// <summary>
    /// 플레이어 현재 상태를 나타내는 클래스 ( 서있는상태, 숙인 상태 )
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class StateUI : SystemUI
    {
        public Sprite standingSprite = null;
        public Sprite bendingSprite = null;
        
        // 현재 상태
        private Image m_image;

        public void ChangeState(bool IsBending)
        {
            if(IsBending)
            {
                // 굽히는 이미지로 변경
                m_image.sprite = bendingSprite;
            }
            else
            {
                // 서있는 이미지로 변경
                m_image.sprite = standingSprite;
            }
        }

        protected override void Action(float amount = 0)
        {
            m_image = GetComponent<Image>();
        }

        protected override void Awake()
        {
            base.Awake();

            Action();
        }
    }
}