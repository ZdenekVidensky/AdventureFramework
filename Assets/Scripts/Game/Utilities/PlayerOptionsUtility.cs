using TVB.Game.Options;
using UnityEngine;

namespace TVB.Game.Utilities
{
    public static class PlayerOptionsUtility
    {
        public const string SOUNDS_VOLUME_PARAMETER = "SoundsVolume";
        public const string MUSIC_VOLUME_PARAMETER = "MusicVolume";
        public const string VOICES_VOLUME_PARAMETER = "VoicesVolume";

        public const float VOLUME_MULTIPLIER = 30f;

        public static void SavePlayerOptions(PlayerOptions options)
        {
            PlayerPrefs.SetInt(nameof(PlayerOptions.SubtitlesOn), options.SubtitlesOn ? 1 : 0);
            PlayerPrefs.SetInt(nameof(PlayerOptions.TutorialOn), options.TutorialOn ? 1 : 0);
            PlayerPrefs.SetFloat(nameof(PlayerOptions.MusicVolume), options.MusicVolume);
            PlayerPrefs.SetFloat(nameof(PlayerOptions.SoundsVolume), options.SoundsVolume);
            PlayerPrefs.SetFloat(nameof(PlayerOptions.VoicesVolume), options.VoicesVolume);

            PlayerPrefs.Save();
        }

        public static PlayerOptions GetPlayerOptions()
        {
            int subtitlesOn = PlayerPrefs.GetInt(nameof(PlayerOptions.SubtitlesOn), 1);
            int tutorialOn = PlayerPrefs.GetInt(nameof(PlayerOptions.TutorialOn), 1);
            float musicVolume = PlayerPrefs.GetFloat(nameof(PlayerOptions.MusicVolume), 1f);
            float ambientVolume = PlayerPrefs.GetFloat(nameof(PlayerOptions.SoundsVolume), 1f);
            float voiceVolume = PlayerPrefs.GetFloat(nameof(PlayerOptions.VoicesVolume), 1f);

            return new PlayerOptions()
            {
                SubtitlesOn = subtitlesOn == 1 ? true : false,
                TutorialOn = tutorialOn == 1 ? true : false,
                MusicVolume = musicVolume,
                SoundsVolume = ambientVolume,
                VoicesVolume = voiceVolume
            };
        }
    }
}
