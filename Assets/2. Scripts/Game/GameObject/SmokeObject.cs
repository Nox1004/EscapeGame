using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using EscapeGame.Particle;

/// <summary>
/// 연기 오브젝트
/// </summary>
namespace EscapeGame.Object {
    [AddComponentMenu("Game/Object/Smoke Object")]
    public class SmokeObject : MGameObject
    {
        #region Editor에서 이용하는 변수 및 함수

        public bool isCreated = false;

        public GameObject smokePrefab = null;
        public Smoke smokeObj = null;

        /// <summary>
        /// 연기 오브젝트 생성
        /// </summary>
        public override void CreateObject()
        {
            if (isCreated)
                return;

            isCreated = true;
            GameObject go = Instantiate(smokePrefab, transform);
            go.name = "Smoke";
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;

            smokeObj = go.GetComponent<Smoke>();
            smokeObj.Reset();
        }

        /// <summary>
        /// 연기오브젝트 제거
        /// </summary>
        public override void RemoveObject()
        {
            isCreated = false;

            Smoke temp = smokeObj;
            smokeObj = null;

            DestroyImmediate(temp.gameObject);
        }

        #endregion

        public byte currentStep { get; private set; }

        protected override void Operate(byte steps, Vector3 dir, bool isPrewarm)
        {
            currentStep = steps;

            if(currentStep == 3)
                return;

            if (steps == 1)
                smokeObj.SetLookRotation(dir);

            smokeObj.ParticleObjectActive(steps, isPrewarm);
        }
    }
}