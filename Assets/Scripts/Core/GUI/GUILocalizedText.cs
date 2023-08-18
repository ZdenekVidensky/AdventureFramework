namespace TVB.Core.GUI
{
    using Sirenix.OdinInspector;
    using TVB.Core.Attributes;
    using TVB.Core.Localization;
    using UnityEngine;
    using GUIText = TMPro.TextMeshProUGUI;

    public class GUILocalizedText : MonoBehaviour
    {
        public int TextID;

        [GetComponent, SerializeField, HideInInspector]
        private GUIText m_Text;

        [MultiLineProperty(5), DisableInEditorMode, DisableInPlayMode, ShowInInspector]
        public string Text => TextDatabase.Localize[TextID];

        public string Translation
        {
            get
            {
                return TextDatabase.Localize[TextID];
            }
        }

        public void SetTextElement(GUIText textElement)
        {
            m_Text = textElement;
        }

        public void Localize()
        {
            string translatedText = Translation;

            if (m_Text != null)
            {
                m_Text.text = translatedText;
            }
        }
    }
}
