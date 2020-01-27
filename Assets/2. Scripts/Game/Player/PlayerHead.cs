using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EscapeGame
{
    /// <summary>
    /// 플레이어 머리부분
    /// </summary>
    public class PlayerHead : MonoBehaviour
    {
        private PlayerCharacter m_PlayerCharacter;

        public void ChangeYLocation()
        {
            m_PlayerCharacter.IsChanging = true;

            StartCoroutine(PositionChanging());
        }

        // Head의 위치를 변경시켜주는 코루틴 함수
        private IEnumerator PositionChanging()
        {
            // 상대적인 목표 위치 (여기로 Head가 이동해야한다)
            Vector3 relativeDstPosition;
            Vector3 srcPosition = new Vector3(0, transform.localPosition.y, 0);

            bool isMoveUp = false;

            // 플레이어 캐릭터의 현재 상태가 굽히고 있어 서있는 상태로 변경해야는지 체크
            if (m_PlayerCharacter.IsBending)
            {
                isMoveUp = true;
                relativeDstPosition = new Vector3(0, this.transform.localPosition.y + m_PlayerCharacter.relativeBend_Y_Value, 0);
            }
            else
            {
                isMoveUp = false;
                relativeDstPosition = new Vector3(0, this.transform.localPosition.y - m_PlayerCharacter.relativeBend_Y_Value, 0);
            }

            while (true)
            {
                if (isMoveUp)
                {
                    // 상대적인y의 값이 대상y의 값보다 높은지 체크한다. 높으면 반복문을 나온다.
                    if (transform.localPosition.y > relativeDstPosition.y)
                        break;

                    transform.localPosition += transform.up * Time.deltaTime;

                    // 일정 위치를 넘어가면 서있는 상태로 변경
                    if (m_PlayerCharacter.IsBending
                        && transform.localPosition.y > relativeDstPosition.y - (relativeDstPosition.y - srcPosition.y) * 0.5f)
                    {
                        m_PlayerCharacter.IsBending = false;
                        
                        
                    }

                }
                else
                {

                    // 상대적인 y의 값이 대상y의 값보다 낮은지 체크한다. 낮으면 반복문을 나온다.
                    if (transform.localPosition.y < relativeDstPosition.y)
                        break;

                    transform.localPosition -= transform.up * Time.deltaTime;

                    // 일정 위치를 넘어가면 굽히는 상태로 변경
                    if (!m_PlayerCharacter.IsBending
                        && transform.localPosition.y < relativeDstPosition.y + (srcPosition.y - relativeDstPosition.y) * 0.5f)
                    {
                        m_PlayerCharacter.IsBending = true;
                    }
                }

                yield return null;
            }

            m_PlayerCharacter.IsChanging = false;

        }

        private void Reset()
        {
            var refHead = GetComponentInChildren<Camera>().gameObject;

            refHead.layer = LayerMask.NameToLayer("PlayerHead");
            refHead.AddComponent<SphereCollider>().isTrigger = true;
        }

        private void Awake()
        {
            m_PlayerCharacter = GetComponentInParent<PlayerCharacter>();
        }
    }
}