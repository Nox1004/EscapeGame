using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EscapeGame.Particle
{
    public class Smoke : MonoBehaviour
    {
        public static float Damage { get; set; }

        // 파티클 오브젝트를 담는 리스트
        public List<GameObject> particleObjects = null;

        /// <summary>
        /// 파티클 오브젝트 활성화
        /// </summary>
        /// <param name="idx">순서</param>
        public void ParticleObjectActive(byte idx, bool isPrewarm)
        {
            if (particleObjects.Count < idx)
                return;

            if(isPrewarm) {
                var MainModule = particleObjects[idx - 1].GetComponent<ParticleSystem>().main;
                MainModule.prewarm = true;
            }

            if(!particleObjects[idx-1].activeSelf)
                particleObjects[idx-1].SetActive(true);
        }

        /// <summary>
        /// Rotation값을 설정하는 함수
        /// </summary>
        /// <param name="dir">방향</param>
        public void SetLookRotation(Vector3 dir)
        {
            if(dir != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(dir);
        }

        public void Reset()
        {
            particleObjects.Clear();

            ParticleSystem[] particles = GetComponentsInChildren<ParticleSystem>();

            // 파티클 오브젝트를 비활성화 시켜둔다.
            foreach(var particle in particles)
            {
                particle.gameObject.SetActive(false);
                particleObjects.Add(particle.gameObject);
            }
        }

        [ContextMenu("Smoke Test")]
        private void Test()
        {
            //SetLookRotation(new Vector3(1, 0, 0));

            ParticleObjectActive(1, true);
        }
    }
}
