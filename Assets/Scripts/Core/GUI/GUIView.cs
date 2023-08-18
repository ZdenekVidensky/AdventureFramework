namespace TVB.Core.GUI
{
    using TVB.Core.Attributes;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(Canvas))]
    public class GUIView : MonoBehaviour
    {
        // PUBLIC MEMBERS

        public bool Interactable
        {
            get { return m_Interactable; }
            set
            {
                m_Frame.blocksRaycasts = value;
                m_Frame.interactable = value;
            }
        }

        public bool IsOpen
        {
            get { return m_Frame.interactable == true && enabled == true; }
        }

        public Canvas Canvas
        {
            get { return m_Canvas; }
        }

        // PROTECTED MEMBERS

        protected Frontend Frontend;

        // PRIVATE MEMBERS

        private GUIComponent[] m_Components;
        private bool m_Initialized;
        private bool m_Interactable;

        [GetComponentInChildren("BackButton"), SerializeField, HideInInspector]
        private Button m_BackButton;

        [GetComponentInChildren("Frame", true), SerializeField, HideInInspector]
        private CanvasGroup m_Frame;
        [GetComponent(true), SerializeField, HideInInspector]
        private Canvas m_Canvas;

        public void Initialize(Frontend frontend)
        {
            Frontend = frontend;
            m_Components = GetComponentsInChildren<GUIComponent>(true);

            for (int idx = 0, count = m_Components.Length; idx < count; idx++)
            {
                m_Components[idx].Initialize(frontend);
            }

            OnInitialized();
        }

        public void Deinitialize()
        {
            for (int idx = 0, count = m_Components.Length; idx < count; idx++)
            {
                m_Components[idx].Deinitialize();
            }


            Frontend = null;
            m_Initialized = false;

            OnDeinitialized();
        }

        public virtual void OnInitialized()
        {
            m_Initialized = true;

            if (m_BackButton != null)
            {
                m_BackButton.onClick.AddListener(OnBackButtonClicked);
            }
        }

        public virtual void OnDeinitialized()
        {
            if (m_BackButton != null)
            {
                m_BackButton.onClick.RemoveListener(OnBackButtonClicked);
            }
        }

        public virtual void OnUpdate()
        {
            if (m_Initialized == false)
                return;

            if (IsOpen == false)
                return;

            for (int idx = 0, count = m_Components.Length; idx < count; idx++)
            {
                m_Components[idx].OnUpdate();
            }
        }

        public virtual void OnOpen() { }
        public virtual void OnClosed() { }

        // PROTECTED METHODS

        protected virtual void Close()
        {
            Frontend.CloseView(this.GetType());
        }

        protected virtual void OnBackButtonClicked()
        {
            Close();
        }
    }
}
