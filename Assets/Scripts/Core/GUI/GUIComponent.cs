﻿namespace TVB.Core.GUI
{
    using TVB.Core.Attributes;
    using UnityEngine;

    public class GUIComponent : MonoBehaviour
    {
        // PUBLIC MEMBERS

        [GetComponent(true), SerializeField, HideInInspector]
        public RectTransform RectTransform;

        // PROTECTED MEMBERS

        protected Frontend Frontend { get; private set; }

        // PRIVATE MEMBERS

        private bool m_Initialized;

        public void Initialize(Frontend frontend)
        {
            Frontend = frontend;

            OnInitialized();
        }

        public void Deinitialize()
        {
            m_Initialized = false;

            OnDeinitialized();
        }

        public virtual void OnDeinitialized()
        {
            Frontend = null;
            m_Initialized = false;
        }

        public virtual void OnInitialized()
        {
            m_Initialized = true;
        }

        public virtual void OnUpdate()
        {
            if (m_Initialized == false)
                return;
        }
    }
}
