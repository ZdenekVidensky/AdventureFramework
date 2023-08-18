namespace TVB.Game.GUI
{
    using System.Text;
    using TVB.Core.Attributes;
    using TVB.Core.GUI;
    using TVB.Core.Localization;
    using TVB.Game.Save;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;
    using GUIText = TMPro.TextMeshProUGUI;

    public class GUISaveSlot : GUIComponent
    {
        // CONSTANTS

        private const int QUICKSAVE_TEXTID = 200015;
        private const int EMPTY_TEXTID = 200016;
        private const int AUTOSAVE_TEXTID = 200017;

        // PRIVATE MEMBERS

        [SerializeField, HideInInspector, GetComponentInChildren("Text", true)]
        private GUIText m_Text;
        [SerializeField, HideInInspector, GetComponent(true)]
        private Button m_Button;
        private string m_SaveFileName;

        [System.Serializable]
        public class GUISaveSlotClickEvent : UnityEvent<string, GUISaveSlot> { }

        // PUBLIC MEMBERS

        public GUISaveSlotClickEvent OnClick = new GUISaveSlotClickEvent();

        // PUBLIC METHODS

        public void Initialize(GUISaveData data)
        {
            string sceneName = TextDatabase.Localize[data.SceneNameID];
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("{0} - {1}", data.Date, sceneName);

            if (data.IsQuicksave == true)
            {
                sb.AppendFormat(" ({0})", TextDatabase.Localize[QUICKSAVE_TEXTID]);
            }
            else if (data.IsAutosave == true)
            {
                sb.AppendFormat(" ({0})", TextDatabase.Localize[AUTOSAVE_TEXTID]);
            }

            m_Text.text = sb.ToString();

            m_SaveFileName = data.SaveFileName;
        }

        public void SetIsEmpty()
        {
            m_Text.text = TextDatabase.Localize[EMPTY_TEXTID];
        }

        public void SetIsEnabled(bool isEnabled)
        {
            m_Button.enabled = isEnabled;
        }

        // OVERRIDES

        public override void OnInitialized()
        {
            base.OnInitialized();

            m_Button.onClick.AddListener(OnButtonClick);
        }

        public override void OnDeinitialized()
        {
            m_Button.onClick.RemoveListener(OnButtonClick);

            base.OnDeinitialized();
        }

        // PRIVATE METHODS

        private void OnButtonClick()
        {
            OnClick.Invoke(m_SaveFileName, this);
        }
    }
}
