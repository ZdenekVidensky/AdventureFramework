namespace TVB.Game.GameSignals
{
    using TVB.Core.SignalSystem;
    
    public class GUISignals
    {
        public Signal<string>      SetItemDescription    = new Signal<string>();
        public Signal<bool>        ShowItemDescription   = new Signal<bool>();
        public Signal<Achievement> UnlockAchievement     = new Signal<Achievement>();
        public Signal<bool>        GameBusyChanged       = new Signal<bool>();
    }
}
