using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EscapeGame
{
    /// <summary>
    /// 레벨디자인을 위한 ScriptableObject
    /// </summary>
    [CreateAssetMenu(fileName = "LevelDesign.asset", menuName = "EscapeGame/LevelDesign Configuration", order = 1)]
    public class LevelDesign : ScriptableObject
    {
        [Header("시작지점")]
        public Vector3 StartPosition;
        public Vector3 StartEular;

        [Header("층 에디터 전용")]
        [Tooltip("현재 작업중인 층")]
        public int floor;

        [Tooltip("현재 작업중인 층 시간추가 및 감소")]
        public float addTime;

        [Tooltip("인덱스 검사")]
        public int searchIndex;

        // 층수만큼 갯수 설정
        public List<FlowTable> smokeTable = new List<FlowTable>();
        public List<FlowTable> combustionTable = new List<FlowTable>();

        [ContextMenu("SmokeListSort")]
        private void SmokeListSort()
        {
            Debug.Log("SmokeListSort");

            smokeTable[floor - 1].MergeSort(0, smokeTable[floor - 1].objs.Count - 1);

            //foreach(var smokes in smokeTable)
            //{
            //    smokes.MergeSort(0, smokes.objs.Count - 1);
            //}
        }

        [ContextMenu("CombustionSort")]
        private void CombustionListSort()
        {
        }

        [ContextMenu("Search Index")]
        private void Test()
        {
            smokeTable[floor - 1].IndexSearch(searchIndex);
        }

        [ContextMenu("Time Change")]
        private void TimeChangeTest()
        {
            if(floor-1 == 0)
            {
                smokeTable[floor - 1].TimeChange(addTime, true);
            }
            else
                smokeTable[floor - 1].TimeChange(addTime);
        }
    }
    
    /// <summary>
    /// 레벨디자인을 위한 흐름표
    /// </summary>
    [System.Serializable]
    public class FlowTable
    {
        public List<CallObject> objs = new List<CallObject>();

        public int IndexSearch(int idx)
        {
            int temp = -1;
            int count = 0;
            
            while(count < objs.Count)
            {
                if (objs[count].idx == idx)
                {
                    temp = count;

                    Debug.Log(temp);
                }
                count++;
            }
            return 1; 
        }

        // 시간 변경
        public void TimeChange(float addTime, bool firstFloor = false)
        {
            if (!firstFloor)
            {
                for (int i = 0; i < objs.Count; i++)
                {
                    if (objs[i].direction != Vector3.zero)
                    {
                        objs[i].AddTime(addTime);
                    }
                }
            }
            else
            {
                for (int i = 0; i < objs.Count; i++)
                {
                    if (objs[i].steps != 1)
                    {
                        objs[i].AddTime(addTime);
                    }
                }
            }
        }

        // Sort - MergeSort를 이용., O(N) = NlogN
        public void MergeSort(int start, int end)
        {
            if(start < end)
            {
                int middle = (start + end) / 2;

                // devide(분할)
                MergeSort(start, middle);
                MergeSort(middle + 1, end);

                // conquer (합병)
                Merge(start, end, middle);
            }
        }

        // Merge 병합
        private void Merge(int start, int end, int middle)
        {
            List<CallObject> temp = new List<CallObject>();

            int i = start, j = middle + 1, copy = 0;

            while( i <= middle && j <= end)
            {
                if (objs[i].time <= objs[j].time)
                    temp.Add(objs[i++]);
                else if (objs[i].time >= objs[j].time)
                    temp.Add(objs[j++]);
            }

            while (i <= middle) temp.Add(objs[i++]);
            while (j <= end) temp.Add(objs[j++]);
            
            for(int k = start; k <= end; k++)
            {
                objs[k] = temp[copy++];
            }
        }

        // [참고] https://hsp1116.tistory.com/33 개인적으로 합병정렬을 복습한다.
    }

    /// <summary>
    /// 호출 객체 관련 클래스
    /// 배열의 인덱스, 객체의 단계, 방향을 갖게 설정한다.
    /// </summary>
    [System.Serializable]
    public class CallObject
    {
        // 순서대로 작성안해도 정렬을 할 수 있게 추가해주자. (∵ 추가 및 수정을 원할하게 하기 위함) 
        [Tooltip("작동 시간(※ 시간순서대로 작성해야함)")]
        public float time;

        [Tooltip("오브젝트 인덱스(ex. smokeList idx, CombustionList idx)")]
        public int idx;

        [Tooltip("오브젝트의 단계(ex. 1단계, 2단계)")]
        public byte steps;

        [Tooltip("방향을 필요로 하는 오브젝트는 방향을 설정해준다")]
        public Vector3 direction;

        /// <summary>
        /// FireDoor 관련
        /// </summary>
        public bool DoorIsClosed; // true : 확산되지 못하게 설정 , false : 확산가능상태

        public byte DoorIdx;


        public void AddTime(float addTime)
        {
            time = time + addTime;
        }
    }
}
