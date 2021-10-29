namespace TVB.Game.Scenes
{
  
    using UnityEngine;

    using TVB.Core;
    using TVB.Game.GUI;
    using TVB.Game.GameSignals;

    public class AdventureScene : Scene
    {
        // CONFIGURATION

        [SerializeField]
        private SceneSettings m_SceneSettings;


        // SCENE INTERFACE

        public override void OnInitialized()
        {
            base.OnInitialized();

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
            Signals.GUISignals.UnlockAchievement.Disconnect(OnAchievementUnlocked);

            base.OnDeinitialized();
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

        private void OnAchievementUnlocked(Achievement achievement)
        {
            GUIAchievementUnlockedView achievementView = Frontend.OpenScreen<GUIAchievementUnlockedView>();
            achievementView.SetData(achievement);
        }
    }
}
