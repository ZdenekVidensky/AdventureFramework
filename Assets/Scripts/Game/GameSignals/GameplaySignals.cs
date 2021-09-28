namespace TVB.Game.GameSignals
{
    using TVB.Core.SignalSystem;

    public class GameplaySignals
    {
        public Signal              EndGame           = new Signal();
        public Signal              Pause             = new Signal();
        public Signal              NewGame           = new Signal();
    }
}
