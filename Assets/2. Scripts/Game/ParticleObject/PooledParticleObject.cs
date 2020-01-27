using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace EscapeGame.Particle {

    [System.Serializable]
    public class PooledParticleObject : PooledObject
    {
        public override void Initialize(Transform parent = null)
        {
            //var refParticleObject = prefab.GetComponent<ParticleObject>();

            //if (refParticleObject == null) { 
            //    Debug.LogError("프리팹에 ParticleObject 클래스가 할당되어있지 않습니다.");

            //    return;
            //}
            //else
            //    base.Initialize(parent);
        }
    }
}