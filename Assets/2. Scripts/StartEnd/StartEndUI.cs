using System.Collections;
using System.Collections.Generic;
using Utilities;
using EscapeGame;
using UnityEngine;
using UnityEngine.UI;

public class StartEndUI : Singleton<StartEndUI>
{
    public Color normalColor;

    public Color selectColor;

    [SerializeField] int cntidx = 0;

    [SerializeField] Button[] Buttons;

    // 현재 인덱스를 증가 및 감소, 버튼의 이미지 색상 변경
    public void Change(bool isIncreased)
    {
        Buttons[cntidx].image.color = normalColor;

        if (isIncreased)
        {
            cntidx++;

            // Buttons 길이보다 클 경우에 0으로 수정
            if (cntidx >= Buttons.Length)
                cntidx = 0;
        }
        else
        {
            cntidx--;

            if (cntidx < 0)
                cntidx = Buttons.Length - 1;
        }

        Buttons[cntidx].image.color = selectColor;
    }

    // 선택
    public void Choice()
    {
        if (cntidx == 0)
        {
            RetryButtonClick();
        }
        else
        {
            ExitButtonClick();
        }
    }

    public void RetryButtonClick()
    {
        GameManager.s_Instance.GameStart();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Floor_4th");
    }

    public void ExitButtonClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();  
#endif
    }

    protected override void Awake()
    {
        base.Awake();

        Buttons = GetComponentsInChildren<Button>();
        
        foreach(var button in Buttons)
        {
            button.image.color = normalColor;
        }

        Buttons[cntidx].image.color = selectColor;
    }
}
