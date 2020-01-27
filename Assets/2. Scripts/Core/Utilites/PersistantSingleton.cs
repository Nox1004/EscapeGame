using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public class PersistantSingleton<T> : Singleton<T> where T: Singleton<T>
    {
        protected override void Awake()
        {
            base.Awake();

            DontDestroyOnLoad(this.gameObject);       
        }
    }
}