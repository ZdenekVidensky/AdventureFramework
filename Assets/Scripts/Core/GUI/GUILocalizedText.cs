﻿namespace TVB.Core.GUI
{
    using UnityEngine;

    using TVB.Core.Attributes;
    using TVB.Core.Localization;

    using GUIText = TMPro.TextMeshProUGUI;

    public class GUILocalizedText : MonoBehaviour
    {
        public int TextID;

        [GetComponent, SerializeField, HideInInspector]
        private GUIText m_Text;

        public string Translation
        {
            get
            {
                return TextDatabase.Localize[TextID];
            }
        }

        public void Localize()
        {
            string translatedText = TextDatabase.Localize[TextID];

            if (m_Text != null)
            {
                m_Text.text = translatedText;
            }
        }
    }
}