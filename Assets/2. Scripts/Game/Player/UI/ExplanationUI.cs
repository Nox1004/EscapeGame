using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

namespace EscapeGame
{
    /// <summary>
    /// 설명 UI 관리
    /// </summary>
    [RequireComponent(typeof(Text))]
    public class ExplanationUI : MonoBehaviour
    {
        private float m_visbleTime;
        private float m_timer = 0.0f;

        private XmlDocument xmlDoc = null;
        private Text m_Text = null;
        // private Image m_Image = null;                        // gif를 넣기 위함
        private bool isActivated;                               // 활성화 상태인가?
        private string ActivateKey = string.Empty;              // 활성화 된 Key의 값을 캐싱용도

        private bool _isPriority = false;

        private Dictionary<string, string> ExplanationTable;

        // Explanation Table의 접근
        public void Enable(string TKey, bool IsPriority = false, float visibleTime = 1.0f)
        {
            if (!IsPriority && ActivateKey == TKey)
            {
                m_timer = 0.0f;
                return;
            }

            if (!_isPriority)
            {
                isActivated = true;

                m_timer = 0.0f;
                m_visbleTime = visibleTime;
                ActivateKey = TKey;

                var TValue = ExplanationTable[TKey];

                if (TValue != null)
                {
                    m_Text.text = TValue;
                    m_Text.enabled = true;
                }
                else
                    Debug.LogWarning("Dictionary 문제 발생");
            }

            if (IsPriority)
                _isPriority = true;
        }

        private void Disable()
        {
            m_timer = 0.0f;
            ActivateKey = string.Empty;

            _isPriority = m_Text.enabled = isActivated = false;

            // 이미지가 활성화 상태라면 비활성화 시켜준다..
        }

        private void Awake()
        {
            m_Text = GetComponent<Text>();
            m_Text.enabled = false;
            
            // 이미지 비활성

            TextAsset textAsset = Resources.Load("Explanation") as TextAsset;
            xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(textAsset.text);

            XmlNodeList nodes = xmlDoc.SelectNodes("/Collection/Explanation");

            // Dictionary 인스턴스 생성
            ExplanationTable = new Dictionary<string, string>();

            foreach(XmlNode node in nodes)
            {
                ExplanationTable.Add(node.Attributes["title"].Value
                                    , node.Attributes["Contents"].Value);
            }
        }

        private void Update()
        {
            if(isActivated)
            {
                m_timer += Time.deltaTime;

                if(m_timer >= m_visbleTime)
                {
                    Disable();
                }
            }
        }
    }
}