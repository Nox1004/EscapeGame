using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Note. 0704
/// 오브젝트에 가려도 그리게 하는 방법을 고민해보기.
/// 아마... 셰이더 공부가 필요해보임

namespace EscapeGame
{
    public class PlayerForward : MonoBehaviour
    {
        private SpriteRenderer m_Sprite;
        private PlayerCharacter m_Character;

        bool isActivated;           // 코루틴 동작을 위한 변수

        private IEnumerator Coroutine()
        {
            Color clr = m_Sprite.color;

            bool isFadeIn = false;          // true : 페이드인  / false : 페이드아웃

            while (true)
            {
                if (isActivated)
                {
                    if (isFadeIn)
                    {
                        clr.a += Time.deltaTime;
                        m_Sprite.color = clr;

                        if (clr.a > 0.5f)
                            isFadeIn = false;
                    }
                    else
                    {
                        clr.a -= Time.deltaTime;
                        m_Sprite.color = clr;

                        if (clr.a < 0.0f)
                            isFadeIn = true;
                    }
                }
                else
                {
                    if (clr.a != 0.0f)
                    {
                        clr.a = 0.0f;
                        m_Sprite.color = clr;
                    }
                }

                yield return null;
            }
        }

        private void Awake()
        {
            m_Sprite = GetComponent<SpriteRenderer>();
            m_Character = GetComponentInParent<PlayerCharacter>();

            StartCoroutine(Coroutine());
        }

        private void LateUpdate()
        {
            isActivated = m_Character.IsMoving ? false : true;
        }
    }
}