namespace TVB.Core.Audio
{
    using System.Collections;

    using UnityEngine;
    
    // TODO: Max volume from options

    public class AudioManager : MonoBehaviour
    {
        // CONSTANTS

        private const float MAX_MUSIC_VOLUME = 1f; // TODO: Remove

        // CONFIGURATION

        [Header("Configuration")]
        [SerializeField]
        private float       m_CrossDuration = 2f;
        [SerializeField]
        private bool        m_Disabled = false;

        [Header("Audio Sources")]
        [SerializeField]
        private AudioSource m_MusicAudioSourceA;
        [SerializeField]
        private AudioSource m_MusicAudioSourceB;
        [SerializeField]
        private AudioSource m_AmbientAudioSource;

        // PUBLIC METHODS

        public void PlayMusic(AudioClip clip)
        {
            if (m_Disabled == true)
                return;

            if (m_MusicAudioSourceA.isPlaying == true && m_MusicAudioSourceA.clip == clip)
                return;

            if (m_MusicAudioSourceB.isPlaying == true && m_MusicAudioSourceB.clip == clip)
                return;

            if (m_MusicAudioSourceA.isPlaying == false)
            {
                m_MusicAudioSourceA.clip = clip;
                StartCoroutine(FadeAudio(m_MusicAudioSourceA, true));

                if (m_MusicAudioSourceB.isPlaying == true)
                {
                    StartCoroutine(FadeAudio(m_MusicAudioSourceB, false));
                }
                return;
            }

            if (m_MusicAudioSourceB.isPlaying == false)
            {
                m_MusicAudioSourceB.clip = clip;
                StartCoroutine(FadeAudio(m_MusicAudioSourceB, true));

                if (m_MusicAudioSourceA.isPlaying == true)
                {
                    StartCoroutine(FadeAudio(m_MusicAudioSourceA, false));
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
            float diff = Time.unscaledDeltaTime / m_CrossDuration;

            if (fadeIn == true)
            {
                audioSource.Play();
                audioSource.volume = 0f;

                while (audioSource.volume < MAX_MUSIC_VOLUME)
                {
                    audioSource.volume += diff;
                    yield return null;
                }
            }
            else
            {
                while (audioSource.volume > 0f)
                {
                    audioSource.volume -= diff;
                    yield return null;
                }

                audioSource.Stop();
            }
        }
    }
}
