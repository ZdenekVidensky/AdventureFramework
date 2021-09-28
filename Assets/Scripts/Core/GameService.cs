namespace TVB.Core
{
    public class GameService
    {
        protected bool m_Initialized;

        // PUBLIC METHODS

        public void Initialize()
        {
            OnInitializing();
            OnInitialized();
        }

        public virtual void OnUpdate()
        {
            if (m_Initialized == false)
                return;
        }

        // PROTECTED METHODS

        protected virtual void OnInitializing() { }
        protected virtual void OnInitialized()
        {
            m_Initialized = true;
        }
    }
}
