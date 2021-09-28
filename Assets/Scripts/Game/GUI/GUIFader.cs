namespace TVB.Game.GUI
{
    using System.Collections;

    using UnityEngine;

    using TVB.Core.Attributes;
    using TVB.Core.GUI;

    // TODO: Refactor on Update
    public class GUIFader : GUIComponent
    {
        [GetComponent(true), SerializeField, HideInInspector]
        private CanvasGroup m_CanvasGroup;

        public override void OnInitialized()
        {
            base.OnInitialized();

            m_CanvasGroup.alpha = 0f;
        }

        public IEnumerator FadeIn(float duration, System.Action onFadeInCompleted = null)
        {
            StopAllCoroutines();
            m_CanvasGroup.alpha = 1f;

            yield return new WaitForSecondsRealtime(0.5f);

            while (m_CanvasGroup.alpha > 0f)
            {
                m_CanvasGroup.alpha -= Time.unscaledDeltaTime / duration;
                yield return null;
            }

            if (onFadeInCompleted != null)
            {
                onFadeInCompleted.Invoke();
            }
        }

        public IEnumerator FadeOut(float duration)
        {
            StopAllCoroutines();
            m_CanvasGroup.alpha = 0f;

            while (m_CanvasGroup.alpha < 1f)
            {
                m_CanvasGroup.alpha += Time.unscaledDeltaTime / duration;
                yield return null;
            }

            yield return new WaitForSecondsRealtime(0.5f);
        }
    }
}
