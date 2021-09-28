﻿namespace TVB.Core
{
    using UnityEngine;

    using TVB.Core.GUI;
    
    public class Scene : MonoBehaviour
    {
        public Frontend Frontend { get => m_Frontend; }

        protected bool        m_Initialized;
        private Frontend      m_Frontend;
        private SceneObject[] m_SceneObjects = new SceneObject[0];
        
        [SerializeField]
        private string m_DebugName;

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
    }
}
