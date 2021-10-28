namespace TVB.Game
{
    using System.Collections;
   
    using UnityEngine;

    using TVB.Core;
    using TVB.Core.Audio;
    using TVB.Core.Attributes;
    using TVB.Game.GUI;
    using TVB.Game.GameSignals;
    using TVB.Game.DebugTools;

    public class AdventureScene : Scene
    {
        // CONFIGURATION

        [SerializeField]
        private SceneSettings m_SceneSettings;

        // PRIVATE MEMBERS

        [GetComponent(true), SerializeField, HideInInspector]
        private SceneCheatManager        m_CheatManager;
        [GetComponent(true), SerializeField, HideInInspector]
        private AudioManager             m_AudioManager;
        private GUIFader                 m_Fader;


        // SCENE INTERFACE

        public override void OnInitialized()
        {
            base.OnInitialized();

            if (m_CheatManager.DisplayDevelopmentView == true)
            {
                GUIDevelopmentView devView = Frontend.OpenScreen<GUIDevelopmentView>();
                devView.DisplayFPS(m_CheatManager.DisplayFPS);
            }

            m_Fader = FindObjectOfType<GUIFader>();
            m_Fader.SetActive(true);

            Signals.GameplaySignals.EndGame.Connect(OnEndGame);
            Signals.GameplaySignals.NewGame.Connect(OnNewGame);
            Signals.GUISignals.UnlockAchievement.Connect(OnAchievementUnlocked);

            InitializePlayer();

            Frontend.OpenScreen<GUIIngameView>();
            StartCoroutine(m_Fader.FadeIn(0.3f, OnFadeInCompleted));


            if (m_SceneSettings.BackgroundMusic != null)
            {
                AdventureGame.Instance.AudioManager.PlayMusic(m_SceneSettings.BackgroundMusic);
            }
            else
            {
                AdventureGame.Instance.AudioManager.StopMusic();
            }

            if (m_SceneSettings.AmbientMusic != null)
            {
                AdventureGame.Instance.AudioManager.PlayAmbient(m_SceneSettings.AmbientMusic);
            }
            else
            {
                AdventureGame.Instance.AudioManager.StopAmbient();
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        public override void OnDeinitialized()
        {
            Signals.GameplaySignals.EndGame.Disconnect(OnEndGame);
            Signals.GameplaySignals.NewGame.Disconnect(OnNewGame);
            Signals.GUISignals.UnlockAchievement.Disconnect(OnAchievementUnlocked);

            //AdventureGame.Instance.AudioManager.StopAmbient();
            //AdventureGame.Instance.AudioManager.StopMusic();

            base.OnDeinitialized();
        }

        public IEnumerator FadeOut(float duration)
        {
            yield return m_Fader.FadeOut(duration);
        }

        // PRIVATE METHODS

        private void InitializePlayer()
        {
            Player player = FindObjectOfType<Player>();

            if (player == null)
            {
                Debug.LogError("There is no player in the scene!");
                return;
            }

            player.SetSceneSettings(m_SceneSettings);
        }

        private IEnumerator OnNewGame_Coroutine()
        {
            Frontend.CloseScreen<GUIMainMenuView>();
            
            yield return new WaitForSeconds(3f);

            Frontend.OpenScreen<GUIIngameView>();
        }

        private IEnumerator EndGame_Coroutine()
        {
            yield return m_Fader.FadeOut(1f);

            AdventureGame.Instance.GameEnded = true;
        }


        // HANDLERS

        private void OnEndGame()
        {
            StartCoroutine(EndGame_Coroutine());
        }

        private void OnNewGame()
        {
            StartCoroutine(OnNewGame_Coroutine());
        }

        private void OnFadeInCompleted()
        {
            AdventureGame.Instance.IsBusy = false;
        }

        private void OnAchievementUnlocked(Achievement achievement)
        {
            GUIAchievementUnlockedView achievementView = Frontend.OpenScreen<GUIAchievementUnlockedView>();
            achievementView.SetData(achievement);
        }
    }
}
