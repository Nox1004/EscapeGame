using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using Valve.VR;

[DisallowMultipleComponent]
public class StartEnd_VR : InputModule
{
    [SerializeField] SteamVR_Action_Boolean TriggerClick;
    [SerializeField] SteamVR_Action_Boolean TouchPadClick;
    [SerializeField] SteamVR_Action_Vector2 TouchPosition;

    bool isActivate;
    float m_time;

    protected override void InputMoveForward()
    {
        Vector2 TouchPadValue = TouchPosition.GetAxis(SteamVR_Input_Sources.Any);

        if( isActivate && TouchPadValue != Vector2.zero)
        {
            isActivate = false;

            float ValueY = TouchPadValue.y;

            if(Mathf.Abs(ValueY) > 0.5f)
            {
                if(TouchPadClick.GetState(SteamVR_Input_Sources.Any))
                {
                    if (ValueY > 0)
                        StartEndUI.s_Instance.Change(true);
                    else
                        StartEndUI.s_Instance.Change(false);
                }
            }
        }
    }

    protected override void InputMoveRight() { }

    protected override void InputTurn() { }

    public override void CustomUpdate()
    {
        base.CustomUpdate();

        // Trigger 클릭 추가
        if(TriggerClick.GetState(SteamVR_Input_Sources.Any))
        {
            StartEndUI.s_Instance.Choice();
        }
    }

    private void Update()
    {
        m_time += Time.deltaTime;

        if(m_time >= 0.2f)
        {
            m_time = 0.0f;
            isActivate = true;
        }

        CustomUpdate();
    }
}
