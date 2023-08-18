namespace TVB.Core
{
    using UnityEngine;

    public class SceneObject : MonoBehaviour
    {
        private bool m_Initialized;

        public void Initialize()
        {
            OnInitialized();
        }

        public void Deinitialize()
        {
            OnDeinitialized();
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

        public virtual void OnDeinitialized() { }
    }
}
