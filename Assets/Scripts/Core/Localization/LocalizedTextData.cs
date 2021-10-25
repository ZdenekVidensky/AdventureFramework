using Sirenix.OdinInspector;
using UnityEngine;

namespace TVB.Core.Localization
{ 
    [System.Serializable]
    public class LocalizedTextData 
    {
        public LocalizedTextData()
        {
            TextID = 0;

            Translates = new LocalizedTextDataItem[]
            {
                new LocalizedTextDataItem { Language = ELanguage.Czech },
                new LocalizedTextDataItem { Language = ELanguage.English }
            };
        }

        public LocalizedTextDataItem GetTranslatedData(ELanguage language)
        {
            for (int idx = 0, count = Translates.Length; idx < count; idx++)
            {
                LocalizedTextDataItem item = Translates[idx];

                if (item.Language == language)
                    return item;
            }

            return default;
        }

        public int TextID;

        [HorizontalGroup("Split", 0.3f)]
        [Button("Copy ID to clipboard")]
        private void CopyTextIDToClipboard()
        {
            GUIUtility.systemCopyBuffer = TextID.ToString();
        }

        public LocalizedTextDataItem[] Translates;
    }

    [System.Serializable]
    public struct LocalizedTextDataItem
    {
        public ELanguage Language;
        public string Text;
        public AudioClip VoiceoverAudioClip; 
    }
}
