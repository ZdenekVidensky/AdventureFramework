using UnityEngine;

namespace TVB.Game.Options
{
    public struct PlayerOptions
    {
        public bool SubtitlesOn;
        public bool TutorialOn;
        public float MusicVolume;
        public float AmbientVolume;
        public float VoiceVolume;
    }

    public static class PlayerOptionsUtility
    {
        public static void StorePlayerOptions(PlayerOptions options)
        {
            PlayerPrefs.SetInt(nameof(PlayerOptions.SubtitlesOn), options.SubtitlesOn ? 1 : 0);
            PlayerPrefs.SetInt(nameof(PlayerOptions.TutorialOn), options.TutorialOn ? 1 : 0);
            PlayerPrefs.SetFloat(nameof(PlayerOptions.MusicVolume), options.MusicVolume);
            PlayerPrefs.SetFloat(nameof(PlayerOptions.AmbientVolume), options.AmbientVolume);
            PlayerPrefs.SetFloat(nameof(PlayerOptions.VoiceVolume), options.VoiceVolume);

            PlayerPrefs.Save();
        }

        public static PlayerOptions GetPlayerOptions()
        {
            int  subtitlesOn = PlayerPrefs.GetInt(nameof(PlayerOptions.SubtitlesOn));
            int  tutorialOn = PlayerPrefs.GetInt(nameof(PlayerOptions.TutorialOn));
            float musicVolume = PlayerPrefs.GetFloat(nameof(PlayerOptions.MusicVolume));
            float ambientVolume = PlayerPrefs.GetFloat(nameof(PlayerOptions.AmbientVolume));
            float voiceVolume = PlayerPrefs.GetFloat(nameof(PlayerOptions.VoiceVolume));

            return new PlayerOptions()
            {
                SubtitlesOn = subtitlesOn == 1 ? true : false,
                TutorialOn = tutorialOn == 1 ? true : false,
                MusicVolume = musicVolume,
                AmbientVolume = ambientVolume,
                VoiceVolume = voiceVolume
            };
        }
    }
}
