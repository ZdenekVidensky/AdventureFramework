namespace TVB.Game.GUI
{
    using System.Collections.Generic;

    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.Events;
    using UnityEngine.EventSystems;

    using TVB.Core.GUI;
    using TVB.Core.Attributes;
    using TVB.Core.Localization;
    using TVB.Game.GameSignals;
    
    using GUIText = TMPro.TextMeshProUGUI;

    public class GUIIngameView : GUIView
    {
        [System.Serializable]
        public class SelectDecision : UnityEvent<int> {}

        //PRIVATE MEMBERS

        [GetComponentInChildren("Subtitles", true), SerializeField, HideInInspector]
        private GUIText       m_Subtitles;
        [GetComponentInChildren("Inventory", true), SerializeField, HideInInspector]
        private GUIInventory  m_Inventory;
        [GetComponentInChildren("ItemDescription", true), SerializeField, HideInInspector]
        private GUIText       m_ItemDescription;

        private RectTransform m_ItemDescriptionRectTransform;

        // Decisions
        [GetComponentInChildren("Decisions", true), SerializeField, HideInInspector]
        private RectTransform m_Decisions;
        [GetComponentInChildren("Decision1", true), SerializeField, HideInInspector]
        private Button        m_Decision1Button;
        [GetComponentInChildren("Decision2", true), SerializeField, HideInInspector]
        private Button        m_Decision2Button;
        [GetComponentInChildren("Decision3", true), SerializeField, HideInInspector]
        private Button        m_Decision3Button;
        [GetComponentInChildren("Decision4", true), SerializeField, HideInInspector]
        private Button        m_Decision4Button;
        [GetComponentInChildren("Decision1Text", true), SerializeField, HideInInspector]
        private GUIText       m_Decision1Text;
        [GetComponentInChildren("Decision2Text", true), SerializeField, HideInInspector]
        private GUIText       m_Decision2Text;
        [GetComponentInChildren("Decision3Text", true), SerializeField, HideInInspector]
        private GUIText       m_Decision3Text;
        [GetComponentInChildren("Decision4Text", true), SerializeField, HideInInspector]
        private GUIText       m_Decision4Text;

        // PUBLIC MEMBERS

        [HideInInspector]
        public SelectDecision SelectDecisionEvent = new SelectDecision();

        // GUIVIEW INTERFACE

        public override void OnInitialized()
        {
            base.OnInitialized();

            m_Decision1Button.onClick.AddListener(OnDecision1ButtonClick);
            m_Decision2Button.onClick.AddListener(OnDecision2ButtonClick);
            m_Decision3Button.onClick.AddListener(OnDecision3ButtonClick);
            m_Decision4Button.onClick.AddListener(OnDecision4ButtonClick);

            m_ItemDescription.SetActive(false);

            Signals.GUISignals.SetItemDescription.Connect(SetItemDescription);
            Signals.GUISignals.ShowItemDescription.Connect(ShowItemDescription);
            Signals.GUISignals.GameBusyChanged.Connect(OnGameIsBusyChanged);

            m_ItemDescriptionRectTransform = m_ItemDescription.rectTransform;
        }

        public override void OnDeinitialized()
        {

            m_Decision1Button.onClick.RemoveListener(OnDecision1ButtonClick);
            m_Decision2Button.onClick.RemoveListener(OnDecision2ButtonClick);
            m_Decision3Button.onClick.RemoveListener(OnDecision3ButtonClick);
            m_Decision4Button.onClick.RemoveListener(OnDecision4ButtonClick);

            Signals.GUISignals.GameBusyChanged.Disconnect(OnGameIsBusyChanged);
            Signals.GUISignals.SetItemDescription.DisconnectAll();
            Signals.GUISignals.ShowItemDescription.DisconnectAll();

            base.OnDeinitialized();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (m_ItemDescription.IsActive() == false)
                return;

            if (AdventureGame.Instance.IsBusy == true)
                return;

            m_ItemDescriptionRectTransform.position = Input.mousePosition;
        }

        public override void OnOpen()
        {
            base.OnOpen();

            SetSubtitlesVisibility(false);
            DisplayDecisions(false);
        }

        // PUBLIC METHODS

        public void EndedInteraction()
        {
            SetSubtitlesVisibility(false);
            DisplayDecisions(false);
        }

        public void SetSubtitlesVisibility(bool state)
        {
            m_Subtitles.SetActive(state);
        }

        public void SetSubtitlesText(string text)
        {
            m_Subtitles.text = text;
        }

        public void SetInventoryData(List<InventoryItem> items)
        {
            m_Inventory.SetData(items);
        }

        public void SetDecisions(List<int> textIDs)
        {
            int lenght = textIDs.Count;

            if (lenght >= 1)
            {
                m_Decision1Text.text = TextDatabase.Localize[textIDs[0]];
            }

            if (lenght >= 2)
            {
                m_Decision2Text.text = TextDatabase.Localize[textIDs[1]];
            }

            if (lenght >= 3)
            {
                m_Decision3Text.text = TextDatabase.Localize[textIDs[2]];
            }

            if (lenght >= 4)
            {
                m_Decision4Text.text = TextDatabase.Localize[textIDs[3]];
            }

            m_Decision1Button.SetActive(lenght >= 1);
            m_Decision2Button.SetActive(lenght >= 2);
            m_Decision3Button.SetActive(lenght >= 3);
            m_Decision4Button.SetActive(lenght >= 4);
        }

        public void DisplayDecisions(bool state)
        {
            if (state == true)
            {
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(m_Decision1Button.gameObject);
                m_Decision1Button.OnSelect(null);
            }

            m_Decisions.SetActive(state);
        }

        // HANDLERS

        private void OnDecision1ButtonClick()
        {
            SelectDecisionEvent.Invoke(0);
        }

        private void OnDecision2ButtonClick()
        {
            SelectDecisionEvent.Invoke(1);
        }

        private void OnDecision3ButtonClick()
        {
            SelectDecisionEvent.Invoke(2);
        }
        private void OnDecision4ButtonClick()
        {
            SelectDecisionEvent.Invoke(3);
        }

        // GUI SIGNALS

        private void SetItemDescription(string description)
        {
            m_ItemDescription.text = description;
        }

        private void ShowItemDescription(bool active)
        {
            m_ItemDescription.SetActive(active);
        }

        private void OnGameIsBusyChanged(bool busy)
        {
            if (busy == true && m_ItemDescription.IsActive() == true)
            {
                m_ItemDescription.SetActive(false);
            }
        }
    }
}
