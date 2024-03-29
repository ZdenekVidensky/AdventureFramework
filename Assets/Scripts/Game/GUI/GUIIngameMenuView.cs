﻿namespace TVB.Game.GUI
{
    using TVB.Core.Attributes;
    using TVB.Core.GUI;
    using TVB.Game.GameSignals;
    using UnityEngine;
    using UnityEngine.UI;

    public class GUIIngameMenuView : GUIView
    {
        // CONFIGURATION

        [SerializeField]
        private AudioClip m_ButtonSound;

        // PRIVATE MEMBERS

        [GetComponentInChildren("ContinueGameButton", true), SerializeField, HideInInspector]
        private Button m_ContinueButton;
        [GetComponentInChildren("SaveGameButton", true), SerializeField, HideInInspector]
        private Button m_SaveGameButton;
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
            m_SaveGameButton.onClick.AddListener(OnSaveGameButtonClicked);
            m_LoadGameButton.onClick.AddListener(OnLoadGameButtonClicked);
            m_EndGameButton.onClick.AddListener(OnEndGameButtonClicked);
            m_OptionsButton.onClick.AddListener(OnOptionsButtonClicked);
        }

        private void OnOptionsButtonClicked()
        {
            Frontend.OpenView<GUIOptionsView>();
        }

        public override void OnDeinitialized()
        {
            m_ContinueButton.onClick.RemoveListener(OnContinueButtonClicked);
            m_SaveGameButton.onClick.RemoveListener(OnSaveGameButtonClicked);
            m_LoadGameButton.onClick.RemoveListener(OnLoadGameButtonClicked);
            m_EndGameButton.onClick.RemoveListener(OnEndGameButtonClicked);
            m_OptionsButton.onClick.RemoveListener(OnOptionsButtonClicked);

            base.OnDeinitialized();
        }

        public override void OnOpen()
        {
            base.OnOpen();

            m_ContinueButton.interactable = true;
            m_SaveGameButton.interactable = true;
            m_EndGameButton.interactable = true;
        }

        // HANDLERS

        private void OnContinueButtonClicked()
        {
            AdventureGame.Instance.IsGamePaused = false;
            Frontend.PlaySound(m_ButtonSound);

            Close();
        }

        private void OnSaveGameButtonClicked()
        {
            Frontend.OpenView<GUISaveGameView>();
        }

        private void OnLoadGameButtonClicked()
        {
            Frontend.OpenView<GUILoadGameView>();
        }

        private void OnEndGameButtonClicked()
        {
            Signals.GameplaySignals.EndGame.Emit();
            Frontend.PlaySound(m_ButtonSound);

            m_EndGameButton.interactable = false;
        }
    }
}
