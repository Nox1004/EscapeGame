using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace EscapeGame
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(PlayerController))]
    public class Keyboard : InputModule
    {
        protected override void InputMoveForward()
        {
            float value = Input.GetAxis("Vertical");
            if (value != 0.0f)
            {
                var refPlayer = (PlayerCharacter)m_Controller.character;

                m_Controller.Move(m_Controller.character.transform.forward,
                    (refPlayer.IsBending == true) ? value / refPlayer.decelerationCoefficient : value);

                InputOfMoving();
            }
        }

        protected override void InputMoveRight()
        {
            float value = Input.GetAxis("Horizontal");
            if (value != 0.0f)
            {
                var refPlayer = (PlayerCharacter)m_Controller.character;

                m_Controller.Move(m_Controller.character.transform.right,
                    (refPlayer.IsBending == true) ? value / refPlayer.decelerationCoefficient : value);

                InputOfMoving();
            }
        }

        protected override void InputTurn()
        {
            float value = Input.GetAxis("Mouse X");
            if (value != 0.0f)
                m_Controller.Turn(value);
        }

        public override void CustomUpdate()
        {
            base.CustomUpdate();

            //Note. 테스트
            var refPlayer = m_Controller.character as PlayerCharacter;

            if (!refPlayer.IsChanging && Input.GetKeyDown(KeyCode.T))
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