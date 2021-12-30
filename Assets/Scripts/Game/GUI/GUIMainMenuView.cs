namespace TVB.Game.GUI
{
    using UnityEngine;
    using UnityEngine.UI;

    using TVB.Core.Attributes;
    using TVB.Core.GUI;
    using TVB.Game.GameSignals;

    public class GUIMainMenuView : GUIView
    {
        // CONFIGURATION

        [SerializeField]
        private AudioClip m_ButtonSound;

        // PRIVATE MEMBERS

        [GetComponentInChildren("ContinueGameButton", true), SerializeField, HideInInspector]
        private Button m_ContinueButton;
        [GetComponentInChildren("NewGameButton", true), SerializeField, HideInInspector]
        private Button  m_NewGameButton;
        [GetComponentInChildren("EndGameButton", true), SerializeField, HideInInspector]
        private Button  m_EndGameButton;
        [GetComponentInChildren("OptionsButton", true), SerializeField, HideInInspector]
        private Button  m_OptionsButton;

        public override void OnInitialized()
        {
            base.OnInitialized();

            m_NewGameButton.onClick.AddListener(OnNewGameButtonClicked);
            m_EndGameButton.onClick.AddListener(OnEndGameButtonClicked);
            m_OptionsButton.onClick.AddListener(OnOptionsButtonClicked);
        }

        private void OnOptionsButtonClicked()
        {
            Frontend.OpenView<GUIOptionsView>();
        }

        public override void OnDeinitialized()
        {
            m_NewGameButton.onClick.RemoveListener(OnNewGameButtonClicked);
            m_EndGameButton.onClick.RemoveListener(OnEndGameButtonClicked);
            m_OptionsButton.onClick.RemoveListener(OnOptionsButtonClicked);

            base.OnDeinitialized();
        }

        public override void OnOpen()
        {
            base.OnOpen();

            m_ContinueButton.interactable = true;
            m_NewGameButton.interactable  = true;
            m_EndGameButton.interactable  = true;
        }

        // HANDLERS

        private void OnNewGameButtonClicked()
        {
            Signals.GameplaySignals.NewGame.Emit();
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
