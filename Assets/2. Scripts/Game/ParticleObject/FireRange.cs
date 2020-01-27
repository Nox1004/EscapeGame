using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace EscapeGame.Particle
{
    [RequireComponent(typeof(SphereCollider))]
    public class FireRange : MonoBehaviour
    {
        Fire m_Fire;
        SphereCollider m_SphereCollider;

        [SerializeField] List<Character> mCharacters;

        private void Reset()
        {
            GetComponent<SphereCollider>().isTrigger = true;
        }

        private void Awake()
        {
            m_Fire = GetComponentInParent<Fire>();
            m_SphereCollider = GetComponent<SphereCollider>();

            m_SphereCollider.radius = m_Fire.Range;
        }

        private void Update()
        { 
            ShootRaycastHit();
        }

        private void ShootRaycastHit()
        {
            RaycastHit hit;
            if (mCharacters.Count != 0)
            {
                for(int i = 0; i < mCharacters.Count; i++)
                {
                    // 해당 게임오브젝트를 가리키는 방향벡터
                    var dir = (mCharacters[i].gameObject.transform.position 
                                - this.transform.position).normalized;

                    // 방향벡터를 향해 레이케스트를 쏜다.
                    Physics.Raycast(this.transform.position, dir, 
                                    out hit, m_SphereCollider.radius);

                    // hit 정보의 collider가 있으면서, MGameObject 컴포넌트가 동적리스트배열 인덱스와 같다면 데미지받는처리를 시도한다.
                    if(hit.collider != null && 
                       hit.collider.GetComponentInParent<Character>() == mCharacters[i])
                    {
                        if (!mCharacters[i].isDamaged)
                        {
                            float ratio = hit.distance / m_Fire.Range;

                            StartCoroutine(mCharacters[i].TriggerDamage(ratio));
                        }
                    }
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            var refGameObject = other.GetComponentInParent<Character>();

            if (refGameObject != null)
                mCharacters.Add(refGameObject);
            
        }

        private void OnTriggerExit(Collider other)
        {
            var refGameObject = other.GetComponentInParent<Character>();

            if (refGameObject != null)
                mCharacters.Remove(refGameObject);
        }
    }
}