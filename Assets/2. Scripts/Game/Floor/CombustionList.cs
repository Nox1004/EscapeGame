using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EscapeGame.Object;

namespace EscapeGame
{
    public class CombustionList : MonoBehaviour
    {
        public List<Combustion> combustionObjects = new List<Combustion>();

        private void Reset()
        {
            combustionObjects.Clear();

            Combustion[] combustions = GetComponentsInChildren<Combustion>();

            foreach(var combustion in combustions)
            {
                combustionObjects.Add(combustion);
            }
        }
    }
}