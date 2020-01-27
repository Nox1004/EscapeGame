using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EscapeGame.Object;

namespace EscapeGame
{
    public class FireDoorList : MonoBehaviour
    {
        public List<FireDoor> fireDoorObjs = new List<FireDoor>();

        private void Reset()
        {
            fireDoorObjs.Clear();

            FireDoor[] doors = GetComponentsInChildren<FireDoor>();

            for(int i = 0; i < doors.Length; i++)
            {
                fireDoorObjs.Add(doors[i]);
                doors[i].idx = i;
            }
        }
    }
}