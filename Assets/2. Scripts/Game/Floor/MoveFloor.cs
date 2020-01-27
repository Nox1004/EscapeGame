using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EscapeGame
{
    [RequireComponent(typeof(BoxCollider))]
    public class MoveFloor : MonoBehaviour
    {
        [SerializeField] string m_NextScene = string.Empty;
        [SerializeField] byte stairsIdx = 0;

        private void Reset()
        {
            gameObject.layer = LayerMask.NameToLayer("MoveFloor");
            GetComponent<BoxCollider>().isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            // Physics에서 Player와만 충돌할 수 있게 설정했지만, 직관성을 위해 조건문 이용
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                var floorMgr = FloorMgr.s_Instance;

                // 층이 이동될 때, 이동되기 전 층의 FireDoor가 항상열려있게 한다. 복잡하게 하면 한없이 어려워진다.
                if ( floorMgr.fireDoorList != null && m_NextScene.CompareTo("Floor_1st") != 0)
                {                                   // CompareTo --> 같을 경우에 0의 값, 다를 경우 -1
                    for (int i = 0; i < floorMgr.fireDoorList.fireDoorObjs.Count; i++)
                    {
                        GameManager.s_Instance.fireDoorArray[(floorMgr.currentFloor - 1) * 2 + i] = true;
                    }
                }

                // Scene 전환 코루틴 호출
                StartCoroutine(PlayerUI.s_Instance.changeUI.ChangeStart(m_NextScene, stairsIdx));
            }
        }
    }
}
