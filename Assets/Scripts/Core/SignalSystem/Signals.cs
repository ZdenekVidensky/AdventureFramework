namespace TVB.Core.SignalSystem
{
    public class Signals
    {
        // STATIC MEMBERS

        private static Signals m_Instance;

        public static Signals Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new Signals();
                }

                return m_Instance;
            }
        }
    }
}
