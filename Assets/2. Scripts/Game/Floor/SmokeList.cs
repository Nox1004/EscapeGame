using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EscapeGame.Object;

namespace EscapeGame
{
    public class SmokeList : MonoBehaviour
    {
        public List<SmokeObject> smokeobjects = new List<SmokeObject>();

        private void Reset()
        {
            smokeobjects.Clear();

            SmokeObject[] smokes = GetComponentsInChildren<SmokeObject>();

            foreach(var smoke in smokes)
            {
                smokeobjects.Add(smoke);
            }
        }

        [ContextMenu("Particle 값 변경")]
        private void Test()
        {
            Debug.Log("Test");
            foreach(var smokeobj in smokeobjects)
            {
                for(int i = 0; i < smokeobj.smokeObj.particleObjects.Count; i++)
                {
                    smokeobj.smokeObj.particleObjects[i].SetActive(true);
                }

                ParticleSystem[] particles = smokeobj.smokeObj.GetComponentsInChildren<ParticleSystem>();
                Debug.Log(particles.Length);
                foreach(var particle in particles)
                {
                    var collisionModule = particle.collision;

                    if(particle.gameObject.name == "Smoke")
                    {
                        var shapeModule = particle.shape;

                        shapeModule.angle = 5.0f;
                        collisionModule.radiusScale = 0.4f;
                    }
                    else
                    {
                        collisionModule.radiusScale = 0.5f;
                    }

                    collisionModule.enableDynamicColliders = true;

                    int temp = 1024 | 131072;

                    collisionModule.collidesWith = temp;
                }

                for (int i = 0; i < smokeobj.smokeObj.particleObjects.Count; i++)
                {
                    smokeobj.smokeObj.particleObjects[i].SetActive(false);
                }
            }
        }
    }
}