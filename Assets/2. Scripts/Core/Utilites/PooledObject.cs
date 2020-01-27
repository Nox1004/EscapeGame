using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public interface IPool
    {
        GameObject PopFromPool(string itemName, Transform parent);

        void PushToPool(string itemName, GameObject obj);
    }
}

namespace Utilities
{
    [System.Serializable]
    public class PooledObject 
    {
        public string poolItemName = string.Empty;

        public GameObject prefab = null;

        public int poolCount = 0;

        [SerializeField] // 오브젝트 풀을 관리해주는 리스트
        private List<GameObject> poolList = new List<GameObject>();

        /// <summary>
        /// 초기화
        /// </summary>
        public virtual void Initialize(Transform parent = null)
        {
            for (int i = 0; i < poolCount; i++)
                poolList.Add(CreateItem(parent));
        }

        /// <summary>
        /// 풀에 다시 넣어주는 함수
        /// </summary>
        public void PushToPool(GameObject item, Transform parent = null)
        {
            item.transform.SetParent(parent);
            item.SetActive(false);
            poolList.Add(item);
        }

        /// <summary>
        /// 풀에서 빼서 사용하는 함수
        /// </summary>
        public GameObject PopFromPool(Transform parent = null)
        {
            if (poolList.Count == 0)
                poolList.Add(CreateItem(parent));

            GameObject item = poolList[0];
            poolList.RemoveAt(0);

            item.SetActive(true);
            item.transform.SetParent(parent);

            return item;
        }

        /// <summary>
        /// 아이템 생성(Initialize에서 호출)
        /// </summary>
        private GameObject CreateItem(Transform parent = null)
        {
            GameObject item = Object.Instantiate(prefab);
            item.name = poolItemName;
            item.transform.SetParent(parent);
            item.SetActive(false);

            return item;
        }
    }
}