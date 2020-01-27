using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EscapeGame;

namespace Utilities
{
    /// <summary>
    /// 입력모듈을 다양하게 하기 위해 추상클래스로 구현
    /// </summary>
    public abstract class InputModule : MonoBehaviour
    {
        protected Controller m_Controller;

        /// <summary>
        /// 이동 및 회전 입력부분을 추상메서드로 정의
        /// </summary>
        protected abstract void InputMoveForward();
        protected abstract void InputMoveRight();
        protected abstract void InputTurn();

        // 각각의 모듈 FixedUpdate에서 호출하는 방식으로 해도 무방하다.
        // PlayerController에서 호출하는 방법으로 변경
        public virtual void CustomUpdate()
        {
            InputMoveForward();
            InputMoveRight();
            InputTurn();
        }

        protected void InputOfMoving(bool isPressing = true)
        {
            var refPlayer = (PlayerCharacter)m_Controller.character;

            if (isPressing) {
                if (!refPlayer.IsMoving)
                    refPlayer.IsMoving = true;
            }
        }

        protected virtual void Awake()
        {
            // 컨트롤러 컴포넌트를 찾아본 후
            var refController = GetComponent<Controller>();
            
            // 있다면 할당해준다
            if (refController)
                m_Controller = refController;
        }
    }
}
