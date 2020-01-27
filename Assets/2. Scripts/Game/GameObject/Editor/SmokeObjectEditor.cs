using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using EscapeGame.Particle;

namespace EscapeGame.Object
{
    [CustomEditor(typeof(SmokeObject))]
    public class SmokeObjectEditor : Editor
    {
        SmokeObject Target = null;

        private void OnEnable()
        {
            Target = target as SmokeObject;
        }

        public override void OnInspectorGUI()
        {
            if(!Target.isCreated)
            {
                Target.smokePrefab = (GameObject)EditorGUILayout.ObjectField("연기 프리팹", Target.smokePrefab, typeof(GameObject), false);

                if (Target.smokePrefab != null
                    && Target.smokePrefab.GetComponent<Smoke>() != null
                    && GUILayout.Button("생성하기"))
                {
                    Target.CreateObject();
                }
            }
            else
            {
                if (GUILayout.Button("제거하기"))
                {
                    Target.RemoveObject();
                }
            }
        }
    }
}