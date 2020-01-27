using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EscapeGame.Object
{
    [CustomEditor(typeof(Combustion))]
    public class CombustionEditor : Editor
    {
        Combustion Target;

        private void OnEnable()
        {
            Target = (Combustion)target;
        }

        public override void OnInspectorGUI()
        {
            // 생성되지 않은 상태라면 생성버튼을 활성화해준다.
            if (!Target.isCreated)
            {
                //Target.EFireType = (FireType)EditorGUILayout.EnumPopup("불 타입", Target.EFireType);
                Target.firePrefab = (GameObject)EditorGUILayout.ObjectField("불 프리팹",Target.firePrefab, typeof(GameObject), false);

                if (Target.firePrefab != null
                    && GUILayout.Button("불 오브젝트 생성"))
                {
                    Target.CreateObject();
                }
            }
            else
            {
                if (GUILayout.Button("불 오브젝트 제거"))
                {
                    Target.RemoveObject();
                }
            }

        }
    }
}