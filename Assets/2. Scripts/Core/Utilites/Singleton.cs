using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        static protected T s_instance;

        static public T s_Instance
        {
            get
            {
                if (s_instance == null)
                {
                    s_instance = FindObjectOfType<T>();

#if UNITY_EDITOR
                    if (s_instance == null)
                        Debug.Log("Singleton Error");
#endif
                }

                return s_instance;
            }
        }

        protected virtual void Awake()
        {
            if(s_instance == null)
            {
                s_instance = (T)this;
            }
            else
            {
                if (s_instance != this)
                {
#if UNITY_EDITOR
                    Debug.Log("Singleton 중복");
#endif
                    Destroy(this.gameObject);
                }
            }
        }
    }
}