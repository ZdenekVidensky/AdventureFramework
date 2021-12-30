namespace TVB.Core.Audio
{
    using System.Collections;
    using TVB.Game.Options;
    using TVB.Game.Utilities;
    using UnityEngine;
    using UnityEngine.Audio;

    public class AudioManager : MonoBehaviour
    {
        // CONSTANTS

        private const float MIN_MUSIC_VOLUME = 0.0001f;

        // CONFIGURATION

        [Header("Configuration")]
        [SerializeField]
        private float       m_CrossDuration = 2f;
        [SerializeField]
        private bool        m_Disabled = false;
        [SerializeField]
        private AudioMixer  m_AudioMixer;

        [Header("Audio Sources")]
        [SerializeField]
        private AudioSource m_MusicAudioSourceA;
        [SerializeField]
        private AudioSource m_MusicAudioSourceB;
        [SerializeField]
        private AudioSource m_AmbientAudioSource;

        // PUBLIC METHODS

        public void Initialize()
        {
            PlayerOptions options = PlayerOptionsUtility.GetPlayerOptions();

            m_AudioMixer.SetFloat(PlayerOptionsUtility.SOUNDS_VOLUME_PARAMETER, Mathf.Log10(options.SoundsVolume) * PlayerOptionsUtility.VOLUME_MULTIPLIER);
            m_AudioMixer.SetFloat(PlayerOptionsUtility.MUSIC_VOLUME_PARAMETER, Mathf.Log10(options.MusicVolume) * PlayerOptionsUtility.VOLUME_MULTIPLIER);
            m_AudioMixer.SetFloat(PlayerOptionsUtility.VOICES_VOLUME_PARAMETER, Mathf.Log10(options.VoicesVolume) * PlayerOptionsUtility.VOLUME_MULTIPLIER);
        }

        public void PlayMusic(AudioClip clip, bool fade = true)
        {
            if (m_Disabled == true)
                return;

            if (m_MusicAudioSourceA.isPlaying == true && m_MusicAudioSourceA.clip == clip)
                return;

            if (m_MusicAudioSourceB.isPlaying == true && m_MusicAudioSourceB.clip == clip)
                return;

            float maxMusicVolume = PlayerOptionsUtility.GetPlayerOptions().MusicVolume;

            if (m_MusicAudioSourceA.isPlaying == false)
            {
                m_MusicAudioSourceA.clip = clip;

                if (fade == true)
                {
                    StartCoroutine(FadeAudio(m_MusicAudioSourceA, true));
                }
                else
                {
                    m_MusicAudioSourceA.volume = maxMusicVolume;
                    m_MusicAudioSourceA.Play();
                }

                if (m_MusicAudioSourceB.isPlaying == true)
                {
                    if (fade == true)
                    {
                        StartCoroutine(FadeAudio(m_MusicAudioSourceB, false));
                    }
                    else
                    {
                        m_MusicAudioSourceB.Stop();
                    }
                }
                return;
            }

            if (m_MusicAudioSourceB.isPlaying == false)
            {
                m_MusicAudioSourceB.clip = clip;

                if (fade == true)
                {
                    StartCoroutine(FadeAudio(m_MusicAudioSourceB, true));
                }
                else
                {
                    m_MusicAudioSourceB.volume = maxMusicVolume;
                    m_MusicAudioSourceB.Play();
                }

                if (m_MusicAudioSourceA.isPlaying == true)
                {
                    if (fade == true)
                    {
                        StartCoroutine(FadeAudio(m_MusicAudioSourceA, false));
                    }
                    else
                    {
                        m_MusicAudioSourceA.Stop();
                    }
                }
                return;
            }

            if (m_MusicAudioSourceA.isPlaying == true && m_MusicAudioSourceB.isPlaying == true)
            {
                m_MusicAudioSourceB.Stop();

                m_MusicAudioSourceB.clip = clip;
                StartCoroutine(FadeAudio(m_MusicAudioSourceB, true));
                StartCoroutine(FadeAudio(m_MusicAudioSourceA, false));
            }
        }

        public void StopMusic()
        {
            StartCoroutine(FadeAudio(m_MusicAudioSourceA, false));
            StartCoroutine(FadeAudio(m_MusicAudioSourceB, false));
        }

        public void PlayAmbient(AudioClip clip)
        {
            m_AmbientAudioSource.clip = clip;
            StartCoroutine(FadeAudio(m_AmbientAudioSource, true));
        }

        public void StopAmbient()
        {
            StartCoroutine(FadeAudio(m_AmbientAudioSource, false));
        }

        // PRIVATE METHODS

        private IEnumerator FadeAudio(AudioSource audioSource, bool fadeIn)
        {
            float maxMusicVolume = PlayerOptionsUtility.GetPlayerOptions().MusicVolume;

            yield return null;

            float diff = Time.unscaledDeltaTime / m_CrossDuration;

            if (fadeIn == true)
            {
                audioSource.Play();
                audioSource.volume = 0f;

                while (audioSource.volume < maxMusicVolume)
                {
                    audioSource.volume += diff;
                    yield return null;
                }
            }
            else
            {
                while (audioSource.volume > MIN_MUSIC_VOLUME)
                {
                    audioSource.volume -= diff;
                    yield return null;
                }

                audioSource.Stop();
            }
        }
    }
}
