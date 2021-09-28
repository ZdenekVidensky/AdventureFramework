namespace TVB.Core.Localization
{
    using System.Collections.Generic;
    using System.Xml;

    using UnityEngine;

    public class TextDatabase
    {
        // STATIC MEMBERS

        private static TextDatabase m_Instance;
        public static System.Action OnLanguageChanged;

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

        private Dictionary<int, string> m_Translations = new Dictionary<int, string>(255);
        private ELanguage               m_CurrentLanguage;

        // PUBLIC METHODS

        public void ReloadTranslations()
        {
            Init(m_CurrentLanguage);
        }

        public void Init(ELanguage language)
        {
            m_Translations.Clear();
            m_CurrentLanguage = language;

            var textObjects = Resources.LoadAll($"Texts/{language}");

            for (int idx = 0, count = textObjects.Length; idx < count; idx++)
            {
                var translations = ExtractTranslations(textObjects[idx] as TextAsset);

                foreach (var translation in translations)
                {
                    AddTranslation(translation.key, translation.value);
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

        private List<(int key, string value)> ExtractTranslations(TextAsset textAsset)
        {
            var result = new List<(int key, string value)>();

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(textAsset.text);

            XmlNodeList nodesList = xml.SelectNodes("/entries/text");

            foreach (XmlNode node in nodesList)
            {
                result.Add((int.Parse(node.Attributes["id"].Value), node.InnerText));
            }

            return result;
        }

        private void AddTranslation(int key, string value)
        {
            m_Translations.TryGetValue(key, out var storedValue);

            if (storedValue != null)
            {
                Debug.LogError($"Translation with key {key} already exists!");
                return;
            }

            m_Translations.Add(key, value);
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
