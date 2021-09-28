﻿namespace TVB.Game.GUI
{
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;

    using TVB.Core.Attributes;
    using TVB.Core.GUI;
    using TVB.Game.GameSignals;

    public class GUIMainMenuView : GUIView
    {
        // CONFIGURATION

        [SerializeField]
        private AudioClip m_ButtonSound;

        // PRIVATE MEMBERS

        [GetComponentInChildren("NewGameButton", true), SerializeField, HideInInspector]
        private Button  m_NewGameButton;
        [GetComponentInChildren("EndGameButton", true), SerializeField, HideInInspector]
        private Button  m_EndGameButton;

        public override void OnInitialized()
        {
            base.OnInitialized();

            m_NewGameButton.onClick.AddListener(OnNewGameButtonClicked);
            m_EndGameButton.onClick.AddListener(OnEndGameButtonClicked);
        }

        public override void OnDeinitialized()
        {
            m_NewGameButton.onClick.RemoveListener(OnNewGameButtonClicked);
            m_EndGameButton.onClick.RemoveListener(OnEndGameButtonClicked);

            base.OnDeinitialized();
        }

        public override void OnOpen()
        {
            base.OnOpen();

            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(m_NewGameButton.gameObject);

            m_NewGameButton.interactable = true;
            m_EndGameButton.interactable = true;
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
