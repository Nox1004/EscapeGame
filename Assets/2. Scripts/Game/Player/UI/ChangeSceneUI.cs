using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace EscapeGame {
    public class ChangeSceneUI : SystemUI
    {
        private Image m_Image;

        // 외부에서 접근할 수 있게 해주는 함수
        public void Activate(float amount = 0.0f)
        {
            Action(amount);
        }

        // 층 간 씬전환을 위한 코루틴
        public IEnumerator ChangeStart(string nextScene, byte stairsIdx)
        {
            Scene preScene = SceneManager.GetActiveScene();

            // 컨트롤러 작동 정지
            m_PlayerUI.Character.controller.enabled = false;

            // Fade Out
            yield return StartCoroutine(Fade(m_Image, EFade.Out));

            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive);
            asyncOperation.allowSceneActivation = false;

            // 위치 및 회전 설정
            m_PlayerUI.Character.transform.position = FloorMgr.s_Instance.positions[stairsIdx].transform.position;
            m_PlayerUI.Character.transform.rotation = FloorMgr.s_Instance.positions[stairsIdx].transform.rotation;

            while (asyncOperation.progress < 0.9f)
            {
                yield return null;
            }

            asyncOperation.allowSceneActivation = true;

            yield return new WaitForSeconds(0.15f);

            SceneManager.SetActiveScene(SceneManager.GetSceneByName(nextScene));

            // 층수를 보여주는 UI작동
            StartCoroutine(m_PlayerUI.floorUI.ShowCurrentFloor(1.0f));

            // Fade In
            yield return StartCoroutine(Fade(m_Image, EFade.In));

            m_PlayerUI.Character.controller.enabled = true;
            m_PlayerUI.inhaleSmokeUI.currentLevel = 0;
            m_PlayerUI.fogUI.Activate(EFade.Out);

            SceneManager.UnloadSceneAsync(preScene);
        }

        protected override void Action(float amount = 0)
        {
            StartCoroutine(End(amount == 1 ? true : false));
        }

        private IEnumerator End(bool isCleared)
        {
            Color clr = m_Image.color;

            // 컨트롤러 조작을 정지시킨다.
            m_PlayerUI.Character.controller.enabled = false;
            m_PlayerUI.hurtUI.enabled = false;
            m_PlayerUI.inhaleSmokeUI.enabled = false;

            while (clr.a < 1.0f)
            {
                clr.a += Time.deltaTime;
                m_Image.color = clr;

                yield return null;
            }

            SceneManager.LoadScene(isCleared ? "End_Success" : "End_Failure");

            yield return null;
        }

        protected override void Awake()
        {
            base.Awake();

            Color clr = Color.black;
            clr.a = 0.0f;

            m_Image = GetComponent<Image>();
            m_Image.color = clr;
        }
    }
}