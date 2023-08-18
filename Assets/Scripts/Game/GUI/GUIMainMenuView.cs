namespace TVB.Game.GUI
{
    using TVB.Core.Attributes;
    using TVB.Core.GUI;
    using TVB.Game.GameSignals;
    using TVB.Game.Utilities;
    using UnityEngine;
    using UnityEngine.UI;

    public class GUIMainMenuView : GUIView
    {
        // CONFIGURATION

        [SerializeField]
        private AudioClip m_ButtonSound;

        // PRIVATE MEMBERS

        [GetComponentInChildren("ContinueGameButton", true), SerializeField, HideInInspector]
        private Button m_ContinueButton;
        [GetComponentInChildren("NewGameButton", true), SerializeField, HideInInspector]
        private Button m_NewGameButton;
        [GetComponentInChildren("LoadGameButton", true), SerializeField, HideInInspector]
        private Button m_LoadGameButton;
        [GetComponentInChildren("EndGameButton", true), SerializeField, HideInInspector]
        private Button m_EndGameButton;
        [GetComponentInChildren("OptionsButton", true), SerializeField, HideInInspector]
        private Button m_OptionsButton;

        public override void OnInitialized()
        {
            base.OnInitialized();

            m_ContinueButton.onClick.AddListener(OnContinueButtonClicked);
            m_NewGameButton.onClick.AddListener(OnNewGameButtonClicked);
            m_EndGameButton.onClick.AddListener(OnEndGameButtonClicked);
            m_OptionsButton.onClick.AddListener(OnOptionsButtonClicked);
            m_LoadGameButton.onClick.AddListener(OnLoadGameButtonClicked);
        }

        private void OnOptionsButtonClicked()
        {
            Frontend.OpenView<GUIOptionsView>();
        }

        public override void OnDeinitialized()
        {
            m_ContinueButton.onClick.RemoveListener(OnContinueButtonClicked);
            m_NewGameButton.onClick.RemoveListener(OnNewGameButtonClicked);
            m_EndGameButton.onClick.RemoveListener(OnEndGameButtonClicked);
            m_OptionsButton.onClick.RemoveListener(OnOptionsButtonClicked);
            m_LoadGameButton.onClick.RemoveListener(OnLoadGameButtonClicked);

            base.OnDeinitialized();
        }

        private void OnContinueButtonClicked()
        {
            Signals.GameplaySignals.ContinueGame.Emit();
            Frontend.PlaySound(m_ButtonSound);
        }

        public override void OnOpen()
        {
            base.OnOpen();

            bool saveFilesExists = SaveUtility.SaveFilesExists();
            m_ContinueButton.SetActive(saveFilesExists);

            m_ContinueButton.interactable = true;
            m_NewGameButton.interactable = true;
            m_EndGameButton.interactable = true;
        }

        // HANDLERS

        private void OnLoadGameButtonClicked()
        {
            Frontend.PlaySound(m_ButtonSound);
            Frontend.OpenView<GUILoadGameView>();
        }

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
