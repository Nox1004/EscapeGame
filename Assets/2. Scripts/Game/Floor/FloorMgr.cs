using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace EscapeGame
{
    /// <summary>
    /// 층 정보를 갖는 싱글톤 객체
    /// </summary>
    public class FloorMgr : Singleton<FloorMgr>
    {
        public int currentFloor;

        [Header("층 이동 위치")] public GameObject[] positions;

        public SmokeList smokeList = null;
        public CombustionList combustionList = null;
        public FireDoorList fireDoorList = null;

        List<CallObject> smokeCallObjQueue;
        List<CallObject> combustionCallObjQueue;

        protected override void Awake()
        {
            if (s_instance == null)
            {
                s_instance = this;
            }
            else
            {
                if (s_Instance != this && s_Instance.currentFloor != currentFloor)
                {
                    s_instance = this;
                }
            }

#if UNITY_EDITOR
            if (smokeList == null || combustionList == null)
            {
                Debug.Log("할당을 제대로 해주세요");
                UnityEditor.EditorApplication.isPlaying = false;
            }
#endif
            FloorInitialize();
        }

        private void Start()
        {
            Initialize();

            StartCoroutine(SmokeQueueCoroutine());
            StartCoroutine(CombustionQueueCoroutine());
        }

        private void FloorInitialize()
        {
            smokeCallObjQueue = new List<CallObject>();
            combustionCallObjQueue = new List<CallObject>();

            var gameMgr = GameManager.s_Instance;
            int countNum = gameMgr.smokeObjectIdx[currentFloor -1]; 

            // smokeCheck
            while(true)
            {
                if (gameMgr.cntLevel.smokeTable[currentFloor - 1].objs.Count == countNum)
                    break;

                if (gameMgr.playTimer.elapsedTime > gameMgr.cntLevel.smokeTable[currentFloor - 1].objs[countNum].time)
                    countNum++;
                else
                    break;
            }
            gameMgr.smokeObjectIdx[currentFloor - 1] = countNum;

            countNum = gameMgr.combustionIdx[currentFloor - 1];

            // combustionCheck +추가해야하는 부분
            while(true)
            {
                if (gameMgr.cntLevel.combustionTable[currentFloor - 1].objs.Count == countNum)
                    break;

                if (gameMgr.playTimer.elapsedTime > gameMgr.cntLevel.combustionTable[currentFloor - 1].objs[countNum].time)
                    countNum++;
                else
                    break;
            }
        }

        private void Initialize()
        {
            var smokeValue = GameManager.s_Instance.smokeObjectIdx[currentFloor - 1];
            var combValue = GameManager.s_Instance.combustionIdx[currentFloor - 1];

            for(int i = 0; i < smokeValue; i++)
            {
                ActivateSmokeObject(i, true);
            }

            for(int i = 0; i < combValue; i++)
            {
                ActivateCombsution(i, true);
            }

            if (fireDoorList != null)
            {
                for (int i = 0; i < fireDoorList.fireDoorObjs.Count; i++)
                {
                    // fireDoorArray값을 참조해서 True이면 문을 연 상태로 시작한다.
                    if (GameManager.s_Instance.fireDoorArray[(currentFloor - 1) * 2 + i])
                    {
                        fireDoorList.fireDoorObjs[i].DoorOpen();
                    }
                }
            }
        }

        /// <summary>
        /// Smoke object 활성화하는 함수
        /// </summary>
        /// <param name="idx">obj index of FlowTable</param>
        /// <param name="isPrewarm">Particle Prewarm Field</param>
        public void ActivateSmokeObject(int idx, bool isPrewarm = false)
        {
            var gameMgr = GameManager.s_Instance;
            var callObj = gameMgr.cntLevel.smokeTable[currentFloor-1].objs[idx];

            if (callObj.DoorIsClosed)
            {
                if(!GameManager.s_Instance.fireDoorArray[ (currentFloor - 1) * 2 + callObj.DoorIdx] )
                {
                    smokeCallObjQueue.Add(callObj);

                    return;
                }
            }

            smokeList.smokeobjects[callObj.idx].Activate(callObj.steps, callObj.direction, isPrewarm ? true : false);
        }

        public void ActivateCombsution(int idx, bool isPrewarm = false)
        {
            var gameMgr = GameManager.s_Instance;
            var callObj = gameMgr.cntLevel.combustionTable[currentFloor - 1].objs[idx];

            if (callObj.DoorIsClosed)
            {
                if (!GameManager.s_Instance.fireDoorArray[(currentFloor - 1) * 2 + callObj.DoorIdx])
                {
                    combustionCallObjQueue.Add(callObj);

                    return;
                }
            }

            combustionList.combustionObjects[callObj.idx].Activate(callObj.steps, Vector3.zero, isPrewarm ? true : false);
        }


        private IEnumerator SmokeQueueCoroutine()
        {
            float delayTime = 0.0f;

            while(true)
            {
                if(smokeCallObjQueue.Count != 0)
                {
                    if(smokeCallObjQueue[0].DoorIsClosed == GameManager.s_Instance.fireDoorArray[ (currentFloor - 1)* 2 + smokeCallObjQueue[0].DoorIdx ])
                    {
                        var callObj = smokeCallObjQueue[0];

                        if (smokeCallObjQueue.Count <= 1)
                        {
                            smokeList.smokeobjects[callObj.idx].Activate(callObj.steps, callObj.direction, false);

                            yield return null;
                        }
                        else
                        {
                            delayTime = smokeCallObjQueue[1].time - callObj.time;

                            smokeList.smokeobjects[callObj.idx].Activate(callObj.steps, callObj.direction, false);

                            yield return new WaitForSeconds(delayTime);
                        }

                        smokeCallObjQueue.RemoveAt(0);
                    }
                }

                yield return null;
            }
        }

        private IEnumerator CombustionQueueCoroutine()
        {
            float delayTime = 0.0f;
            bool isActivate = false;

            bool firstAcitvate = false;
            float activeTime = 0.0f;

            var gameMgr = GameManager.s_Instance;

            while(true)
            {
                if (combustionCallObjQueue.Count != 0)
                {
                    var callObj = combustionCallObjQueue[0];

                    if (!isActivate && gameMgr.fireDoorArray[(currentFloor-1)*2 +callObj.DoorIdx] == callObj.DoorIsClosed)
                    {
                        // 작동시작
                        isActivate = true;

                        // 작동하게 된 시간
                        activeTime = gameMgr.playTimer.elapsedTime;

                        // 작동시간이 Queue의 첫번째 원소의 시간보다 짧은 경우
                        if (activeTime <= callObj.time)
                            firstAcitvate = true;   // firstActivate를 true로 변경시킨다.
                    }
                    
                    if(isActivate)
                    {
                        if(firstAcitvate)
                        {
                            delayTime = callObj.time - activeTime;

                            yield return new WaitForSeconds(delayTime);

                            firstAcitvate = false;
                        }
                        else
                        {
                            if (combustionCallObjQueue.Count <= 1)
                            {
                                combustionList.combustionObjects[callObj.idx].Activate(callObj.steps, callObj.direction, false);

                                yield return null;
                            }
                            else
                            {
                                delayTime = combustionCallObjQueue[1].time - callObj.time;

                                combustionList.combustionObjects[callObj.idx].Activate(callObj.steps, callObj.direction, false);

                                yield return new WaitForSeconds(delayTime);
                            }
                        }
                    }
                }

                yield return null;
            }
        }
    }
}
