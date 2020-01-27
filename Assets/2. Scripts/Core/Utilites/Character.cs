using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EscapeGame.Particle;
using EscapeGame;

namespace Utilities
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CharacterMovement))]
    [RequireComponent(typeof(Health))]
    public abstract class Character : MonoBehaviour
    {
        [SerializeField] GameObject ControllerPrefab = null;

        protected bool m_isActivating = false;
        public bool isDamaged;

        [Tooltip("데미지를 받았을 때, 딜레이 시간")]
        [Header("Delay Time")]
        public float waitTime;

        CharacterMovement m_Movement;
        Controller m_Controller;

        public Health health { get; private set; }

        public Controller controller { get { return m_Controller; } }

        public virtual void AddMovementInput(Vector3 WorldDirection, float ScaleValue)
        {
            if (m_Movement)
                m_Movement.AddInputVector(WorldDirection, ScaleValue);
        }

        public virtual void AddControllerYawInput(float Value)
        {
            if (m_Movement)
                m_Movement.AddYawInput(Value);
        }

        public virtual IEnumerator TriggerDamage(float ratio)
        {
            if (health.Hp <= 0)
                yield return null;

            // 단순 데미지 처리만 한다.
            float _time = 0.0f;

            isDamaged = true;

            if (ratio < 0.2f)
            {
                // 가까우면 고온으로 인해 사망
                health.Damaged(health.MaxHp);

                yield return null;
            }
            else
                health.Damaged(Fire.Damage * ratio);

            var playerUI = PlayerUI.s_Instance;

            // HurtUI 작동
            playerUI.hurtUI.Activate(ratio);

            // ExplanationUI 작동
            playerUI.explanationUI.Enable("FireDanager", true);

            while (_time < waitTime)
            {
                _time += Time.deltaTime;
                yield return null;
            }

            isDamaged = false;

            playerUI.hurtUI.Activate();
        }

        public void SetActivate(bool isActivated) { m_isActivating = isActivated; }

        public abstract void ExhaustPhysicalStrength();
        protected abstract void OnParticleCollision(GameObject other);

        protected virtual void Reset()
        {
            m_Movement = GetComponent<CharacterMovement>();
            m_Movement.Initialize(this);
            m_Movement.MaxWalkSpeed = 1.0f;
            m_Movement.RotateSpeed = 10.0f;
        }

        protected virtual void Awake()
        {
            health = GetComponent<Health>();

            // Controller 할당 및 생성
            Controller refCon = ControllerPrefab.GetComponent<Controller>();

            if(refCon != null)
            {
                GameObject _controller = Instantiate(ControllerPrefab, this.transform);
                _controller.name = "Controller";
                m_Controller = _controller.GetComponent<Controller>();
                m_Controller.Initialize(this);
            }

            // Movement 할당 및 초기화
            m_Movement = GetComponent<CharacterMovement>();
            m_Movement.Initialize(this);
        }
    }
}