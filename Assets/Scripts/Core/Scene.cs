namespace TVB.Core
{
    using System.Collections;

    using UnityEngine;

    using TVB.Core.GUI;
    using TVB.Core.Attributes;
    using TVB.Core.Audio;
    using TVB.Game;
    using TVB.Game.GUI;
    using TVB.Game.DebugTools;

    public class Scene : MonoBehaviour
    {
        // CONFIGURATION

        [SerializeField]
        protected SceneSettings           m_SceneSettings;

        public Frontend Frontend { get => m_Frontend; }

        protected bool                    m_Initialized;
        private Frontend                  m_Frontend;
        private SceneObject[]             m_SceneObjects = new SceneObject[0];

        public string SceneName => m_SceneSettings.SceneName ?? "";

        // PROTECTED MEMBERS

        [GetComponent(true), SerializeField, HideInInspector]
        protected SceneCheatManager m_CheatManager;
        protected GUIFader m_Fader;

        // PUBLIC METHODS

        public void Initialize()
        {
            m_Frontend = GetComponentInChildren<Frontend>();

            if (m_Frontend != null)
            {
                m_Frontend.Initialize();
            }

            m_SceneObjects = GetComponentsInChildren<SceneObject>();

            for (int idx = 0, count = m_SceneObjects.Length; idx < count; idx++)
            {
                m_SceneObjects[idx].Initialize();
            }

            if (m_CheatManager.DisplayDevelopmentView == true)
            {
                GUIDevelopmentView devView = Frontend.OpenScreen<GUIDevelopmentView>();
                devView.DisplayFPS(m_CheatManager.DisplayFPS);
            }

            m_Fader = FindObjectOfType<GUIFader>();
            m_Fader.SetActive(true);

            OnInitialized();
        }

        public void Deinitialize()
        {
            for (int idx = 0, count = m_SceneObjects.Length; idx < count; idx++)
            {
                m_SceneObjects[idx].Deinitialize();
            }

            if (m_Frontend != null)
            {
                m_Frontend.Deinitialize();
            }

            m_Initialized = false;

            OnDeinitialized();
        }

        public virtual void OnInitialized()
        {
            m_Initialized = true;     
        }

        public virtual void OnUpdate()
        {
            if (m_Initialized == false)
                return;

            for (int idx = 0, count = m_SceneObjects.Length; idx < count; idx++)
            {
                m_SceneObjects[idx].OnUpdate();
            }
        }

        public virtual void OnLateUpdate()
        {
            if (m_Initialized == false)
                return;

            if (m_Frontend != null)
            {
                m_Frontend.OnUpdate();
            }
        }

        public virtual void OnDeinitialized() { }

        public IEnumerator FadeOut(float duration)
        {
            yield return m_Fader.FadeOut(duration);
        }

        // PRIVATE MEMBERS
        protected void OnFadeInCompleted()
        {
            AdventureGame.Instance.IsBusy = false;
        }
    }
}
