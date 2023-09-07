using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Playniax.UI.SimpleGameUI
{
    public class CanvasGroupFader : MonoBehaviour
    {
        public float fadeSpeed = 5;
        CanvasGroup canvasGroup;

        void Awake()
        {
            if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();

            if (canvasGroup) _targetAlpha = canvasGroup.alpha;
        }

        void Start()
        {
            if (SimpleGameUI.instance && canvasGroup) canvasGroup.alpha = 0;
        }

        void Update()
        {
            if (SimpleGameUI.instance && canvasGroup)
            {
                if (SimpleGameUI.instance.isBusy)
                {
                    canvasGroup.alpha -= fadeSpeed * Time.unscaledDeltaTime;
                    if (canvasGroup.alpha < 0) canvasGroup.alpha = 0;
                }
                else
                {
                    canvasGroup.alpha += fadeSpeed * Time.unscaledDeltaTime;
                    if (canvasGroup.alpha > _targetAlpha) canvasGroup.alpha = _targetAlpha;
                }
            }
        }

        float _targetAlpha;
    }
}