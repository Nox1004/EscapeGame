using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using Valve.VR;

// character 다운캐스팅이 가능하다면? 

namespace EscapeGame
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(PlayerController))]
    public class VR : InputModule
    {
        [SerializeField] SteamVR_Action_Boolean MenuClick;
        [SerializeField] SteamVR_Action_Vector2 TouchPosition;
        [SerializeField] SteamVR_Action_Boolean TouchPadClick;

        public override void CustomUpdate()
        {
            base.CustomUpdate();

            // 상태변환을 위해 Application Menu 버튼이 눌렸는지
            InputCheckMenuButton();
        }

        protected override void InputMoveForward()
        {
            Vector2 TouchRightPadValue = TouchPosition.GetAxis(SteamVR_Input_Sources.RightHand);


            if (TouchRightPadValue != Vector2.zero)
            {
                float rightValueY = TouchRightPadValue.y;

                if (Mathf.Abs(rightValueY) > 0.5f)
                {
                    var refPlayer = (PlayerCharacter)m_Controller.character;

                    if (TouchPadClick.GetState(SteamVR_Input_Sources.RightHand))
                    {
                        if (refPlayer.IsBending)    //캐릭터가 숙이고 있는 중이면 이동속도를 상대적으로 느리게
                        {
                            m_Controller.Move(m_Controller.character.transform.forward,
                                        (rightValueY > 0) ? rightValueY / refPlayer.decelerationCoefficient : rightValueY * 0.75f / refPlayer.decelerationCoefficient);
                        }
                        else
                        {
                            m_Controller.Move(m_Controller.character.transform.forward,
                                        (rightValueY > 0) ? rightValueY : rightValueY * 0.75f);
                        }
                    }
                }
            }
        }

        protected override void InputMoveRight()
        {
            Vector2 TouchRightPadValue = TouchPosition.GetAxis(SteamVR_Input_Sources.RightHand);

            if (TouchRightPadValue != Vector2.zero)
            {
                float rightValueX = TouchRightPadValue.x;

                if (Mathf.Abs(rightValueX) > 0.5f)
                {
                    if (TouchPadClick.GetState(SteamVR_Input_Sources.RightHand))
                        m_Controller.Move(m_Controller.character.transform.right, rightValueX * 0.65f);
                }
            }
        }

        protected override void InputTurn()
        {
            Vector2 TouchLeftPadValue = TouchPosition.GetAxis(SteamVR_Input_Sources.LeftHand);

            if (TouchLeftPadValue != Vector2.zero)
            {
                float leftValueX = TouchLeftPadValue.x;

                if (Mathf.Abs(leftValueX) > 0.2f)
                {
                    if (TouchPadClick.GetState(SteamVR_Input_Sources.LeftHand))
                        m_Controller.Turn(leftValueX);
                }
            }
        }

        //Note. ClickB 이벤트를 등록해주어야한다. 연구실에서 작업
        private void InputCheckMenuButton()
        {
            var refPlayer = (PlayerCharacter)m_Controller.character;

            if (!refPlayer.IsChanging && MenuClick.stateDown)
            {
                refPlayer.ChangeState();
            }
        }

        protected override void Awake()
        {
            m_Controller = GetComponent<PlayerController>();
        }
    }
}