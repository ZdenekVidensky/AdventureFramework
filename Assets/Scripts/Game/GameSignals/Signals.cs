namespace TVB.Game.GameSignals
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

        public static GUISignals GUISignals => Instance.m_GUISignals;
        public static GameplaySignals GameplaySignals => Instance.m_GameplaySignals;

        // PRIVATE MEMBERS

        private GUISignals m_GUISignals = new GUISignals();
        private GameplaySignals m_GameplaySignals = new GameplaySignals();
    }
}
