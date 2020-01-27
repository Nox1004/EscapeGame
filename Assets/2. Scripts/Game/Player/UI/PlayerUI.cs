using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace EscapeGame
{
    public class PlayerUI : Singleton<PlayerUI>
    {
        public PlayerCharacter Character { get; private set; }

        public Canvas canvas { get; private set; }

        public HurtUI hurtUI { get; private set; }
        public InhaleSmokeUI inhaleSmokeUI { get; private set; }
        public FloorUI floorUI { get; private set; }
        public FogUI fogUI { get; private set; }
        public StateUI stateUI { get; private set; }
        public ExplanationUI explanationUI { get; private set; }
        public ChangeSceneUI changeUI { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            canvas = GetComponent<Canvas>();
            Character = canvas.worldCamera.GetComponentInParent<PlayerCharacter>();

            hurtUI = GetComponentInChildren<HurtUI>();
            inhaleSmokeUI = GetComponentInChildren<InhaleSmokeUI>();
            floorUI = GetComponentInChildren<FloorUI>();
            fogUI = GetComponentInChildren<FogUI>();
            stateUI = GetComponentInChildren<StateUI>();
            changeUI = GetComponentInChildren<ChangeSceneUI>();
            explanationUI = GetComponentInChildren<ExplanationUI>();

            DontDestroyOnLoad(this.gameObject);
        }
    }
}