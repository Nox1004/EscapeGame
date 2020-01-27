using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace EscapeGame.Object {
    /// <summary>
    /// FireDoor
    /// </summary>
    [AddComponentMenu("Game/Object/Fire Door")]
    [RequireComponent(typeof(BoxCollider))]
    public class FireDoor : MGameObject
    {
        public int idx;

        [SerializeField] GameObject rightDoor;
        [SerializeField] GameObject leftDoor;

        bool isOpened = false;

        public void DoorOpen()
        {
            Operate(0, Vector3.zero, false);
        }

        protected override void Operate(byte steps, Vector3 dir, bool isPrewarm)
        {
            isOpened = true;

            // fireDoorArray 값을 바꿔준다.
            GameManager.s_Instance.fireDoorArray[(FloorMgr.s_Instance.currentFloor-1)*2 + idx] = true;

            Vector3 cntEuler = rightDoor.transform.rotation.eulerAngles;
            rightDoor.transform.rotation = Quaternion.Euler(cntEuler.x, cntEuler.y - 85, cntEuler.z);

            cntEuler = leftDoor.transform.rotation.eulerAngles;
            leftDoor.transform.rotation = Quaternion.Euler(cntEuler.x, cntEuler.y + 90, cntEuler.z);
        }

        private void Reset()
        {
            Transform[] transforms = GetComponentsInChildren<Transform>();
            GetComponent<BoxCollider>().isTrigger = true;

            foreach (var obj in transforms)
            {
                if (obj.gameObject.CompareTag("FireDoor_Left"))
                {
                    leftDoor = obj.gameObject;
                }
                else if (obj.gameObject.CompareTag("FireDoor_Right"))
                {
                    rightDoor = obj.gameObject;
                }
            }

        }

        private void OnTriggerEnter(Collider other)
        {
            if (!isOpened)
            {
                DoorOpen();
            }
        }
    }
}