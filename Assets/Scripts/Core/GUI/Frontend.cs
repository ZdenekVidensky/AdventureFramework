namespace TVB.Core.GUI
{
    using System.Linq;
    using System.Collections.Generic;
    
    using UnityEngine;

    using TVB.Core.Localization;

    public class Frontend : MonoBehaviour
    {
        // CONFIGURATION

        [SerializeField]
        private AudioSource m_AudioSource;

        // PRIVATE MEMBERS
        
        private GUIView[]     m_AvailableViews = new GUIView[0];
        private List<GUIView> m_ScreensStack   = new List<GUIView>(10);

        // PUBLIC METHODS

        public void Initialize()
        {
            m_ScreensStack.Clear();
            m_AvailableViews = GetComponentsInChildren<GUIView>(true);

            for (int idx = 0, count = m_AvailableViews.Length; idx < count; idx++)
            {
                var view = m_AvailableViews[idx];

                view.Initialize(this);
                view.SetActive(true);
                view.Canvas.enabled = false;
            }

            TextDatabase.OnLanguageChanged += RefreshTranslations;
            
            RefreshTranslations();

            OnInitialized();
        }

        public void PlaySound(AudioClip clip)
        {
            m_AudioSource.PlayOneShot(clip);
        }

        public void Deinitialize()
        {
            for (int idx = 0, count = m_AvailableViews.Length; idx < count; idx++)
            {
                m_AvailableViews[idx].Deinitialize();
            }

            OnDeinitialized();
        }

        public T OpenScreen<T>() where T : GUIView
        {
            return OpenScreen(typeof(T)) as T;
        }

        public GUIView OpenScreen(System.Type screenType)
        {
            var screenToOpen = m_AvailableViews.FirstOrDefault(m => m.GetType() == screenType);

            if (screenToOpen == null)
            {
                return null;
            }

            m_ScreensStack.Insert(0, screenToOpen);
            screenToOpen.Interactable = true;
            screenToOpen.Canvas.enabled = true;
            screenToOpen.SetActive(true);
            screenToOpen.OnOpen();

            return screenToOpen;
        }

        public bool IsViewOpen<T>() where T: GUIView
        {
            return m_ScreensStack.FirstOrDefault(m => m.GetType() == typeof(T)) != null;
        }

        public void CloseScreen<T>() where T : GUIView
        {
            CloseScreen(typeof(T));
        }

        public void CloseScreen(System.Type screenType)
        {
            var viewToClose = m_ScreensStack.FirstOrDefault(m => m.GetType() == screenType);

            if (viewToClose == null)
                return;

            viewToClose.Interactable   = false;
            viewToClose.Canvas.enabled = false;
            viewToClose.OnClosed();
            m_ScreensStack.Remove(viewToClose);
        }

        public virtual void OnInitialized() { }
        public virtual void OnDeinitialized() { }
        public virtual void OnUpdate()
        {
            for (int idx = 0, count = m_AvailableViews.Length; idx < count; idx++)
            {
                m_AvailableViews[idx].OnUpdate();
            }
        }

        // PRIVATE METHODS

        private void RefreshTranslations()
        {
            var localizedTexts = GetComponentsInChildren<GUILocalizedText>(true);

            for (int idx = 0; idx < localizedTexts.Length; idx++)
            {
                localizedTexts[idx].Localize();
            }
        }
    }
}
