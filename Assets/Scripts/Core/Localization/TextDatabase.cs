namespace TVB.Core.Localization
{
    using System.Collections.Generic;
    using System.IO;
    using UnityEditor;
    using UnityEngine;

    public class TextDatabase
    {
        // STATIC MEMBERS

        private static TextDatabase m_Instance;
        public static System.Action OnLanguageChanged;
        private const string ITEMS_PATH = "Assets/Prefabs/Localization/";

        public static string CurrentLanguage
        {
            get
            {
                return GetInstance().m_CurrentLanguage.ToString();
            }
        }

        public static TextDatabase Localize
        {
            get { return GetInstance(); }
        }

        public static void InitializeLanguage(ELanguage language)
        {
            GetInstance().Init(language);
        }

#if UNITY_EDITOR
        // C-TOR
        public TextDatabase()
        {
            Init(ELanguage.Czech);
        }
#endif

        // PUBLIC MEMBERS

        public string this[int textID] { get { return GetInstance().Get(textID); } }


        // PRIVATE MEMBERS

        private Dictionary<int, string>    m_Translations = new Dictionary<int, string>(255);
        private Dictionary<int, AudioClip> m_TranslatedAudioClips = new Dictionary<int, AudioClip>(255);
        private ELanguage                  m_CurrentLanguage;

        // PUBLIC METHODS

        public void ReloadTranslations()
        {
            Init(m_CurrentLanguage);
        }

        public void Init(ELanguage language)
        {
            m_Translations.Clear();
            m_CurrentLanguage = language;

            string[] files = Directory.GetFiles(ITEMS_PATH, "*.asset");

            for (int idx = 0; idx < files.Length; idx++)
            {
                LocalizedTexts asset = AssetDatabase.LoadAssetAtPath<LocalizedTexts>(files[idx]);

                if (asset == null)
                    continue;

                for (int jdx = 0, count = asset.Items.Count; jdx < count; jdx++)
                {
                    LocalizedTextData localizedData = asset.Items[jdx];
                    LocalizedTextDataItem localizedDataItem = localizedData.GetTranslatedData(language);

                    m_Translations.Add(localizedData.TextID, localizedDataItem.Text);
                    
                    if (localizedDataItem.VoiceoverAudioClip != null)
                    {
                        m_TranslatedAudioClips.Add(localizedData.TextID, localizedDataItem.VoiceoverAudioClip);
                    }
                }
            }
        }

        public static void ChangeLanguage(ELanguage language)
        {
            InitializeLanguage(language);
            OnLanguageChanged.SafeInvoke();
        }

        public static void Reload()
        {
            GetInstance().ReloadTranslations();
        }

        // PRIVATE METHODS

        private static TextDatabase GetInstance()
        {
            if (m_Instance == null)
            {
                m_Instance = new TextDatabase();
            }

            return m_Instance;
        }

        private string Get(int key)
        {
            m_Translations.TryGetValue(key, out var result);

            return result;
        }
    }

    public enum ELanguage
    {
        Czech,
        English
    }
}
