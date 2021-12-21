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
        private List<GUIView> m_ViewsStack   = new List<GUIView>(10);

        // PUBLIC METHODS

        public void Initialize()
        {
            m_ViewsStack.Clear();
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
            if (clip == null)
                return;

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

        public T OpenView<T>() where T : GUIView
        {
            return OpenView(typeof(T)) as T;
        }

        public GUIView OpenView(System.Type viewType)
        {
            GUIView viewToOpen = m_AvailableViews.FirstOrDefault(m => m.GetType() == viewType);

            if (viewToOpen == null)
            {
                return null;
            }

            m_ViewsStack.Insert(0, viewToOpen);
            viewToOpen.Interactable = true;
            viewToOpen.Canvas.enabled = true;
            viewToOpen.SetActive(true);
            viewToOpen.OnOpen();

            return viewToOpen;
        }

        public bool IsViewOpen<T>() where T: GUIView
        {
            return m_ViewsStack.FirstOrDefault(m => m.GetType() == typeof(T)) != null;
        }

        public void CloseView<T>() where T : GUIView
        {
            CloseView(typeof(T));
        }

        public void CloseView(System.Type viewType)
        {
            GUIView viewToClose = m_ViewsStack.FirstOrDefault(m => m.GetType() == viewType);

            if (viewToClose == null)
                return;

            viewToClose.Interactable   = false;
            viewToClose.Canvas.enabled = false;
            viewToClose.OnClosed();
            m_ViewsStack.Remove(viewToClose);
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
