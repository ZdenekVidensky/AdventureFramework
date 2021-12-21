namespace TVB.Game.Scenes
{
    using UnityEngine;

    using TVB.Core;
    using TVB.Game.GUI;
    using TVB.Game.GameSignals;

    public class MenuScene : Scene
    {
        // SCENE INTERFACE

        public override void OnInitialized()
        {
            base.OnInitialized();

            Frontend.OpenScreen<GUIMainMenuView>();

            StartCoroutine(m_Fader.FadeIn(0.3f, OnFadeInCompleted));

            Signals.GameplaySignals.NewGame.Connect(OnNewGame);
            Signals.GameplaySignals.EndGame.Connect(OnEndGame);

            m_AudioManager.PlayMusic(m_SceneSettings.BackgroundMusic, false);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        public override void OnDeinitialized()
        {
            Signals.GameplaySignals.NewGame.Disconnect(OnNewGame);
            Signals.GameplaySignals.EndGame.Disconnect(OnEndGame);

            base.OnDeinitialized();
        }

        // HANDLERS

        private void OnNewGame()
        {
            AdventureGame.Instance.IsBusy = true;

            m_AudioManager.StopMusic();
            AdventureGame.Instance.LoadSceneAsync("SampleScene");
        }

        private void OnEndGame()
        {
            AdventureGame.Instance.GameEnded = true;
        }
    }
}
