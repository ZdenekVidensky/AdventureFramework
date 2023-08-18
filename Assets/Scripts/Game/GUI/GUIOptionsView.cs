namespace TVB.Game.GUI
{
    using TVB.Core.Attributes;
    using TVB.Core.GUI;
    using TVB.Game.GameSignals;
    using TVB.Game.Options;
    using TVB.Game.Utilities;
    using UnityEngine;
    using UnityEngine.Audio;
    using UnityEngine.UI;

    public class GUIOptionsView : GUIView
    {
        // CONFIGURATION

        [SerializeField]
        private AudioClip m_ButtonSound; // TODO: Move to some settings

        [Header("Audio Mixers")]
        [SerializeField]
        private AudioMixer m_AudioMixer;

        // PRIVATE MEMBERS

        //[GetComponentInChildren("ApplyButton", true), SerializeField, HideInInspector]
        //private Button m_ApplyButton;
        [GetComponentInChildren("SubtitlesToggle", true), SerializeField, HideInInspector]
        private Toggle m_SubtitlesToggle;

        [GetComponentInChildren("SoundsSlider", true), SerializeField, HideInInspector]
        private Slider m_SoundsSlider;
        [GetComponentInChildren("MusicSlider", true), SerializeField, HideInInspector]
        private Slider m_MusicSlider;
        [GetComponentInChildren("VoicesSlider", true), SerializeField, HideInInspector]
        private Slider m_VoicesSlider;

        // Options

        private bool m_SubtitlesOn;
        private float m_SoundsVolume;
        private float m_MusicVolume;
        private float m_VoicesVolume;

        public override void OnInitialized()
        {
            base.OnInitialized();

            m_SubtitlesToggle.onValueChanged.AddListener(OnSubtitlesToggleChanged);

            // Sliders

            m_SoundsSlider.onValueChanged.AddListener(OnSoundsSliderChanged);
            m_MusicSlider.onValueChanged.AddListener(OnMusicSliderChanged);
            m_VoicesSlider.onValueChanged.AddListener(OnVoicesSliderChanged);
        }

        public override void OnDeinitialized()
        {
            m_SubtitlesToggle.onValueChanged.RemoveListener(OnSubtitlesToggleChanged);

            // Sliders

            m_SoundsSlider.onValueChanged.RemoveListener(OnSoundsSliderChanged);
            m_MusicSlider.onValueChanged.RemoveListener(OnMusicSliderChanged);
            m_VoicesSlider.onValueChanged.RemoveListener(OnVoicesSliderChanged);

            base.OnDeinitialized();
        }

        public override void OnOpen()
        {
            base.OnOpen();

            PlayerOptions options = PlayerOptionsUtility.GetPlayerOptions();

            m_SubtitlesOn = options.SubtitlesOn;
            m_MusicVolume = options.MusicVolume;
            m_SoundsVolume = options.SoundsVolume;
            m_VoicesVolume = options.VoicesVolume;

            m_SubtitlesToggle.isOn = m_SubtitlesOn;
            m_SoundsSlider.value = m_SoundsVolume;
            m_MusicSlider.value = m_MusicVolume;
            m_VoicesSlider.value = m_VoicesVolume;
        }

        protected override void OnBackButtonClicked()
        {
            PlayerOptions newOptions = new PlayerOptions();
            newOptions.SubtitlesOn = m_SubtitlesOn;
            newOptions.MusicVolume = m_MusicVolume;
            newOptions.SoundsVolume = m_SoundsVolume;
            newOptions.VoicesVolume = m_VoicesVolume;

            PlayerOptionsUtility.SavePlayerOptions(newOptions);

            Signals.GameplaySignals.OptionsChanged.Emit(newOptions);

            base.OnBackButtonClicked();
        }

        // HANDLERS

        private void OnSoundsSliderChanged(float value)
        {
            m_SoundsVolume = value;

            m_AudioMixer.SetFloat(PlayerOptionsUtility.SOUNDS_VOLUME_PARAMETER, Mathf.Log10(m_SoundsVolume) * PlayerOptionsUtility.VOLUME_MULTIPLIER);
        }

        private void OnMusicSliderChanged(float value)
        {
            m_MusicVolume = value;

            m_AudioMixer.SetFloat(PlayerOptionsUtility.MUSIC_VOLUME_PARAMETER, Mathf.Log10(m_MusicVolume) * PlayerOptionsUtility.VOLUME_MULTIPLIER);
        }

        private void OnVoicesSliderChanged(float value)
        {
            m_VoicesVolume = value;

            m_AudioMixer.SetFloat(PlayerOptionsUtility.VOICES_VOLUME_PARAMETER, Mathf.Log10(m_VoicesVolume) * PlayerOptionsUtility.VOLUME_MULTIPLIER);
        }

        private void OnSubtitlesToggleChanged(bool value)
        {
            m_SubtitlesOn = value;
        }
    }
}
