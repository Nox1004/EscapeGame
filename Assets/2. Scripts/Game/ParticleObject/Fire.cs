using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EscapeGame.Particle
{
    public class Fire : MonoBehaviour
    {
        public FireType _FireType;

        public FireType fireType { get; private set; }

        public static float Damage { get; set; }

        // 반경
        public float Range;

        private void Reset()
        {
            ParticleSystem[] particles = GetComponentsInChildren<ParticleSystem>();

            foreach (var particle in particles)
            {
                particle.gameObject.layer = LayerMask.NameToLayer("Fire");
            }
        }

        protected void Awake()
        {
            fireType = _FireType;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(this.transform.position, Range);
        }
    }
}