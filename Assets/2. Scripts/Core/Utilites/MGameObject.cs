using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public abstract class MGameObject : MonoBehaviour
    {
        private byte cntSteps;
        
        public void Activate(byte steps, Vector3 dir, bool isPrewarm = false)
        {
            cntSteps = steps;

            Operate(cntSteps, dir, isPrewarm);    
        }

        public virtual void CreateObject() { }
        public virtual void RemoveObject() { }

        protected abstract void Operate(byte steps, Vector3 dir, bool isPrewarm);
    }
}