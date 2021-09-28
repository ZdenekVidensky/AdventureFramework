namespace TVB.Game.GUI
{
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;

    using TVB.Core.Attributes;
    using TVB.Core.GUI;
    using TVB.Game.GameSignals;

    public class GUIIngameMenuView : GUIView
    {        // CONFIGURATION

        [SerializeField]
        private AudioClip m_ButtonSound;
        [SerializeField]
        private AudioClip m_OpenSound;
        [SerializeField]
        private AudioClip m_CloseSound;

        // PRIVATE MEMBERS

        [GetComponentInChildren("ContinueButton", true), SerializeField, HideInInspector]
        private Button m_ContinueButton;
        [GetComponentInChildren("EndGameButton", true), SerializeField, HideInInspector]
        private Button m_EndGameButton;

        public override void OnInitialized()
        {
            base.OnInitialized();

            m_ContinueButton.onClick.AddListener(OnContinueButtonClicked);
            m_EndGameButton.onClick.AddListener(OnEndGameButtonClicked);
        }

        public override void OnDeinitialized()
        {
            m_ContinueButton.onClick.RemoveListener(OnContinueButtonClicked);
            m_EndGameButton.onClick.RemoveListener(OnEndGameButtonClicked);

            base.OnDeinitialized();
        }

        public override void OnOpen()
        {
            base.OnOpen();

            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(m_ContinueButton.gameObject);

            Frontend.PlaySound(m_OpenSound);

            m_ContinueButton.interactable = true;
            m_EndGameButton.interactable  = true;
        }

        public override void OnClosed()
        {
            Frontend.PlaySound(m_CloseSound);

            base.OnClosed();
        }

        // HANDLERS

        private void OnContinueButtonClicked()
        {
            Signals.GameplaySignals.Pause.Emit();
            Frontend.PlaySound(m_ButtonSound);
        }

        private void OnEndGameButtonClicked()
        {
            Signals.GameplaySignals.EndGame.Emit();
            Frontend.PlaySound(m_ButtonSound);
            m_EndGameButton.interactable = false;
        }
    }
}
