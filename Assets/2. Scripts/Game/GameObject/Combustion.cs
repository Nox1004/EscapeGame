using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using EscapeGame.Particle;

/// <summary>
/// 연소물 오브젝트
/// </summary>
namespace EscapeGame.Object
{
    [AddComponentMenu("Game/Object/Combustion")]
    public class Combustion : MGameObject
    {
        #region Editor에서 이용하는 변수 및 함수
        public bool isCreated = false;

        public GameObject firePrefab = null;
        public GameObject FireObject = null;

        /// <summary>
        /// 불오브젝트 생성
        /// </summary>
        public override void CreateObject()
        {
            if (isCreated)
                return;

            GetComponentInChildren<Collider>().gameObject.layer = LayerMask.NameToLayer("Combustion");

            isCreated = true;
            GameObject go = Instantiate(firePrefab, this.transform);
            go.name = "Fire";
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            go.SetActive(false);

            FireObject = go;
        }

        /// <summary>
        /// 불오브젝트 제거
        /// </summary>
        public override void RemoveObject()
        {
            isCreated = false;

            GameObject go = FireObject;
            FireObject = null;

            DestroyImmediate(go);
        }

        #endregion

        bool m_isBurning = false;

        protected override void Operate(byte steps, Vector3 dir, bool isPrewarm)
        {
            // 1단계가 될 경우
            if (steps == 1)
            {
                if (m_isBurning)
                    return;

                m_isBurning = true;

                FireObject.SetActive(true);
            }
        }

        private void Reset()
        {
            GetComponentInChildren<Collider>().gameObject.layer = LayerMask.NameToLayer("Object");

            if (GetComponentInChildren<Collider>().gameObject == null)
            {
                Debug.Log(gameObject);
            }
        }

        [ContextMenu("Test")]
        private void Test()
        {
            Operate(1,Vector3.zero,false);
        }
    }
}