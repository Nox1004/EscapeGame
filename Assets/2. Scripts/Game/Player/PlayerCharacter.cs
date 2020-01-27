using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using EscapeGame.Particle;

namespace EscapeGame
{
    [AddComponentMenu("Game/Player/Player Character")]
    public class PlayerCharacter : Character
    {
        #region Fields

        [Range(1.1f, 2.0f)]
        [Header("감속 계수")]
        public float decelerationCoefficient;

        [Tooltip("상태변환 시 사용할 Y값")] [Range(0, 1)]
        public float relativeBend_Y_Value;

        public bool isColliding;                                    // 파티클과 충돌하고 있는가?

        private float m_time;                                       // 연기에 벗어난 시간

        private bool m_isBending;

        private PlayerHead m_Head;

        static public PlayerCharacter s_Instance { get; private set; }                // 싱글톤 구현

        public bool IsMoving { get; set; }

        public bool IsBending { // 굽히는 상태인가?
            get { return m_isBending; }
            set {
                m_isBending = value;
                PlayerUI.s_Instance.stateUI.ChangeState(m_isBending);
            }
        }                         

        public bool IsChanging { get; set; }                        // 상태가 변하고 있는 중입니까?

        public bool isDied { get; private set; }

        #endregion


        // 상태를 변경해주는 메서드
        public void ChangeState()
        {
            if (!IsChanging)
            {
                m_Head.ChangeYLocation();
            }
        }

        protected override void OnParticleCollision(GameObject other)
        {
            // 연기에 벗어난 시간을 0으로 초기화
            m_time = 0.0f;

            // Smoke 오브젝트의 단계를 얻어온다.
            var refcurrentStep = other.GetComponentInParent<Object.SmokeObject>().currentStep;

            var playerUI = PlayerUI.s_Instance;
            var refinhleSmokeUI = playerUI.inhaleSmokeUI;

            // 연기관련UI의 currentLevel이 연기오브젝트의 단계랑 다를 경우
            if (refinhleSmokeUI.currentLevel != refcurrentStep)
            {
                byte temp = refinhleSmokeUI.currentLevel;
                
                // 값을 갱신해준다.
                refinhleSmokeUI.currentLevel = refcurrentStep;

                if(temp > refcurrentStep)
                {
                    playerUI.fogUI.Activate(EFade.Out, refcurrentStep);
                }
                else
                {
                    playerUI.fogUI.Activate(EFade.In, refcurrentStep);
                }
            }

            // 설명UI 작동
            if (!IsBending && refcurrentStep == 2)
                playerUI.explanationUI.Enable("HighRisk");
            else if (refcurrentStep == 3)
                playerUI.explanationUI.Enable("VeryHighRisk");

            if (!isColliding)
                isColliding = true;
        }

        // 플레이어가 죽었을 때 호출
        public override void ExhaustPhysicalStrength()
        {
            isDied = true;

            PlayerUI.s_Instance.changeUI.Activate();
        }

        private IEnumerator CheckOutOfSmokeParticle()
        {
            while(!isDied)
            {
                if(isColliding)
                {
                    m_time += Time.deltaTime;

                    // 일정시간이 지나면 InhaleSmokeUI를 비활성화시켜준다.
                    if(m_time > 0.75f) {
                        m_time = 0.0f;
                        isColliding = false;

                        PlayerUI.s_Instance.inhaleSmokeUI.currentLevel = 0;
                        PlayerUI.s_Instance.fogUI.Activate(EFade.Out);
                    }
                }
                yield return null;
            }
        }

        protected override void Awake()
        {
            base.Awake();

            if (s_Instance == null)
                s_Instance = this;
            else
            {
                if (s_Instance != this)
                    Destroy(this.gameObject); // 파괴
            }

            m_Head = GetComponentInChildren<PlayerHead>();

            int layerIdx = LayerMask.NameToLayer("Player");

            if (layerIdx == -1)
                Debug.LogError("Player 레이어 설정을 해주세요");
            else
                gameObject.layer = layerIdx;

            StartCoroutine(CheckOutOfSmokeParticle());

            DontDestroyOnLoad(this.gameObject);
        }

        private void Start()
        {
            // 위치설정
            var cntLevel = GameManager.s_Instance.cntLevel;
            this.transform.position = cntLevel.StartPosition;
            this.transform.rotation = Quaternion.Euler(0, cntLevel.StartEular.y, 0);

            PlayerUI.s_Instance.explanationUI.Enable("GameStart",false, 4.0f);
        }

        private void OnDrawGizmosSelected()
        {
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlaying)
                Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y - relativeBend_Y_Value, transform.position.z), 0.1f);
#endif
        }
    }
}