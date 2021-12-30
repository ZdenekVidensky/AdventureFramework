namespace TVB.Game.GameSignals
{
    using TVB.Core.SignalSystem;
    using TVB.Game.Options;

    public class GameplaySignals
    {
        public Signal                EndGame           = new Signal();
        public Signal                Pause             = new Signal();
        public Signal                NewGame           = new Signal();
        public Signal                ContinueGame      = new Signal();
        public Signal<PlayerOptions> OptionsChanged  = new Signal<PlayerOptions>();              
    }
}
