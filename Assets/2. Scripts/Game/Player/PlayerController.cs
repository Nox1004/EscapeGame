using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace EscapeGame
{
    [AddComponentMenu("Game/Player/Controller")]
    public class PlayerController : Controller
    {
        List<InputModule> m_InputList;

        private void Awake()
        {
            m_InputList = new List<InputModule>();

            InputModule[] modules = GetComponents<InputModule>();

            foreach (var module in modules)
            {
                if (module.enabled)
                    m_InputList.Add(module);
            }
        }

        private void Update()
        {
            var refPlayer = character as PlayerCharacter;
            if (refPlayer.IsMoving)
            {
                refPlayer.IsMoving = false;
            }

            for (int i = 0; i < m_InputList.Count; i++)
            {
                m_InputList[i].CustomUpdate();
            }
        }
    }
}