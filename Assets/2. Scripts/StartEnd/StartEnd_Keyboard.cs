using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

[DisallowMultipleComponent]
public class StartEnd_Keyboard : InputModule
{
    bool isActivate;
    float m_time;

    protected override void InputMoveForward()
    {
        float value = Input.GetAxisRaw("Vertical");
        var ref_endUI = StartEndUI.s_Instance;

        if(isActivate && value != 0.0f)
        {
            isActivate = false;

            if (value > 0)
                ref_endUI.Change(true);
            else
                ref_endUI.Change(false);
        }
    }

    protected override void InputMoveRight() { }

    protected override void InputTurn() { }

    public override void CustomUpdate()
    {
        base.CustomUpdate();

        if(Input.GetKeyDown(KeyCode.Return))
            StartEndUI.s_Instance.Choice();
    }

    private void Update()
    {
        m_time += Time.deltaTime;
        if (m_time >= 0.2f)
        {
            m_time = 0.0f;
            isActivate = true;
        }

        CustomUpdate();
    }
}
