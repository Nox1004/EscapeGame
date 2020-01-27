﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using Utilities;

// 사용하지 않는다.
namespace EscapeGame.Particle
{
    public class FirePool : MonoBehaviour, IPool
    {
        [Header("오브젝트 풀링")]
        public List<PooledParticleObject> PoolList = new List<PooledParticleObject>();
        
        private void Awake()
        {
            foreach (var Pool in PoolList)
            {
                Pool.Initialize(this.transform);
            }
        }

        /// Pop
        public GameObject PopFromPool(string itemName, Transform parent = null)
        {
            PooledObject Pool = GetPoolItem(itemName);
            if (Pool == null)
                return null;

            return Pool.PopFromPool(parent);
        }

        /// Push 사용하지 않고 있음.
        public void PushToPool(string itemName, GameObject obj)
        {
            PooledObject Pool = GetPoolItem(itemName);

            if (Pool == null)
                return;

            Pool.PushToPool(obj, transform);
        }

        private PooledObject GetPoolItem(string itemName)
        {
            for (int i = 0; i < PoolList.Count; i++)
            {
                if (PoolList[i].poolItemName.Equals(itemName))
                    return PoolList[i];
            }
            return null;
        }
    }
}