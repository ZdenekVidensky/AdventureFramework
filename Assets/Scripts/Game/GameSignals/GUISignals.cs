namespace TVB.Game.GameSignals
{
    using TVB.Core.SignalSystem;
    
    public class GUISignals
    {
        public Signal<string>      SetActionText         = new Signal<string>();
        public Signal<bool>        ShowActionText        = new Signal<bool>();
        public Signal<Achievement> UnlockAchievement     = new Signal<Achievement>();
    }
}
