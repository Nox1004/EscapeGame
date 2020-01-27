using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EscapeGame
{
    public abstract class SystemUI : MonoBehaviour
    {
        protected PlayerUI m_PlayerUI;

        protected abstract void Action(float amount = 0.0f);

        protected virtual void Awake()
        {
            m_PlayerUI = GetComponentInParent<PlayerUI>();
        }

        protected IEnumerator Fade(MaskableGraphic UIComponent, EFade fade, float alpha = 0.0f)
        {
            Color clr = UIComponent.color;

            // Fade In (알파값이 0이 되는 걸로 생각한다)
            if (fade == EFade.In)
            {
                while (clr.a > alpha)
                {
                    clr.a -= Time.deltaTime;
                    UIComponent.color = clr;

                    yield return null;
                }

                UIComponent.color = new Color(clr.r, clr.g, clr.b, 0);
            }
            // Fade Out (알파값이 1이 되는걸로 생각한다)
            else
            {
                while (clr.a < 1.0f)
                {
                    clr.a += Time.deltaTime;
                    UIComponent.color = clr;

                    yield return null;
                }

                UIComponent.color = new Color(clr.r, clr.g, clr.b, 1);
            }

            yield return null;
        }
    }
}