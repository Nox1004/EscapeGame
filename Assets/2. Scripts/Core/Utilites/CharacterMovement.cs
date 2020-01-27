using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody))]
    public class CharacterMovement : MonoBehaviour
    {
        Character m_Character;
        Rigidbody m_Rigidbody;

        public float MaxWalkSpeed;
        public float RotateSpeed;

        public virtual void Initialize(Character character)
        {
            m_Character = character;

            m_Rigidbody = GetComponent<Rigidbody>();
            m_Rigidbody.mass = 10.0f;
            m_Rigidbody.useGravity = true;
        }

        public virtual void AddInputVector(Vector3 WorldDirection, float ScaleValue)
        {
            if(m_Character)
            {
                CharacterMove(WorldDirection * ScaleValue);
            }
        }

        public virtual void AddYawInput(float Value)
        {
            if(m_Character)
            {
                CharacterTurn(Value);
            }
        }

        private void CharacterMove(Vector3 WorldDirection)
        {
            float moveDistance = WorldDirection.magnitude * MaxWalkSpeed;

            Vector3 movement = moveDistance * WorldDirection * Time.deltaTime;
            
            m_Rigidbody.position += movement;
            transform.position = m_Rigidbody.position;
        }

        private void CharacterTurn(float Value)
        {
            Quaternion Rotation = Quaternion.Euler(new Vector3(0, Value * RotateSpeed, 0) * Time.deltaTime);
            m_Rigidbody.MoveRotation(m_Rigidbody.rotation * Rotation);
        }
    }
}