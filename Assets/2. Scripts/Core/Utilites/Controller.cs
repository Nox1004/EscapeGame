using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    [DisallowMultipleComponent]
    public abstract class Controller : MonoBehaviour
    {
        public Character character {
            get;
            private set;
        }

        public void Initialize(Character character)
        {
            this.character = character;
        }
        
        public void Move(Vector3 WorldAccel, float ScaleValue)
        {
            if(character)
                character.AddMovementInput(WorldAccel, ScaleValue);
        }

        public void Turn(float ScaleValue)
        {
            if (character)
                character.AddControllerYawInput(ScaleValue);
        }
    }
}