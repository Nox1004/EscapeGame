using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using EscapeGame.Object;
using EscapeGame.Particle;

/// <summary>
/// 게임의 관리자 객체
/// </summary>
namespace EscapeGame
{
    public class GameManager : PersistantSingleton<GameManager>
    {
        /// <summary>
        /// 전역 변수 할당을 위한 값
        /// </summary>
        [Header("데미지 설정")]
        [SerializeField] float FireDamage = 0; // 15.0f 
        [SerializeField] float SmokeDamage = 0; // 0.5f

        [SerializeField] PlayTimer m_PlayTimer = null;

        [Header("Level Design")]
        [SerializeField] List<LevelDesign> lvDesign = null;

        public LevelDesign cntLevel { get; private set; }
        public PlayTimer playTimer { get { return m_PlayTimer; } }

        public int[] combustionIdx { get; private set; }
        public int[] smokeObjectIdx { get; private set; }

        // 방화문의 오픈 정보를 위한 배열
        public bool[] fireDoorArray { get; private set; }

        public bool isPlaying { get; private set; }

        /// <summary>
        /// 게임시작시 호출
        /// </summary>
        public void GameStart()
        {
            if (lvDesign.Count != 0)
            {
                cntLevel = lvDesign[Random.Range(0, lvDesign.Count)];
            }
            isPlaying = true;

            m_PlayTimer.Initialize();

            for (int i = 0; i < 4; i++)
            {
                combustionIdx[i] = 0;
                smokeObjectIdx[i] = 0;
            }

            for ( int i = 0; i < fireDoorArray.Length; i++)
            {
                fireDoorArray[i] = false;
            }
        }

        // 게임종료시 호출
        public void GameEnd()
        {
            Destroy(PlayerCharacter.s_Instance.gameObject); // Character 파괴
            Destroy(PlayerUI.s_Instance.gameObject);        // PlayerUI 파괴

            isPlaying = false;
        }

        protected override void Awake()
        {
            base.Awake();

            if (Smoke.Damage != SmokeDamage) Smoke.Damage = SmokeDamage;
            if (Fire.Damage != FireDamage) Fire.Damage = FireDamage;

#if UNITY_EDITOR
            if (m_PlayTimer == null)
            {
                UnityEditor.EditorApplication.isPlaying = false;
            }
#endif

            combustionIdx = new int[4];
            smokeObjectIdx = new int[4];
            fireDoorArray = new bool[8];        // 각층에 방화문 2개 씩 있다고 가정

#if UNITY_EDITOR
            GameStart(); // test
#endif
        }

        private void Update()
        {
            var refFloorMgr = FloorMgr.s_Instance;
            
            if (refFloorMgr != null)
            {
                var cntFloor = refFloorMgr.currentFloor - 1;

                if (cntLevel.smokeTable[cntFloor].objs.Count != smokeObjectIdx[cntFloor])
                {
                    if (m_PlayTimer.elapsedTime > cntLevel.smokeTable[cntFloor].objs[smokeObjectIdx[cntFloor]].time)
                    {
                        while( smokeObjectIdx[cntFloor] != cntLevel.smokeTable[cntFloor].objs.Count )
                        {
                            var callObj = cntLevel.smokeTable[cntFloor].objs[smokeObjectIdx[cntFloor]];
                            
                            if(!callObj.DoorIsClosed)
                                break;
                            else
                            {
                                if (!fireDoorArray[cntFloor * 2 + callObj.DoorIdx])
                                {
                                    refFloorMgr.ActivateSmokeObject(smokeObjectIdx[cntFloor]);
                                    smokeObjectIdx[cntFloor]++;
                                }
                                else
                                    break;
                            }
                        }

                        if (smokeObjectIdx[cntFloor] != cntLevel.smokeTable[cntFloor].objs.Count)
                        {
                            refFloorMgr.ActivateSmokeObject(smokeObjectIdx[cntFloor]);
                            
                            // 인덱스 증가
                            smokeObjectIdx[cntFloor]++;
                        }
                    }
                }

                if (cntLevel.combustionTable[cntFloor].objs.Count != combustionIdx[cntFloor])
                {
                    if (m_PlayTimer.elapsedTime > cntLevel.combustionTable[cntFloor].objs[combustionIdx[cntFloor]].time)
                    {
                        while (combustionIdx[cntFloor] != cntLevel.combustionTable[cntFloor].objs.Count)
                        {
                            var callObj = cntLevel.combustionTable[cntFloor].objs[combustionIdx[cntFloor]];

                            if (!callObj.DoorIsClosed)
                                break;
                            else
                            {
                                if (!fireDoorArray[cntFloor * 2 + callObj.DoorIdx])
                                {
                                    refFloorMgr.ActivateCombsution(combustionIdx[cntFloor]);
                                    combustionIdx[cntFloor]++;
                                }
                                else
                                    break;
                            }
                        }

                        if (combustionIdx[cntFloor] != cntLevel.combustionTable[cntFloor].objs.Count)
                        {
                            refFloorMgr.ActivateCombsution(combustionIdx[cntFloor]);

                            // 인덱스 증가
                            combustionIdx[cntFloor]++;
                        }
                    }
                }
            }
        }
    }
}