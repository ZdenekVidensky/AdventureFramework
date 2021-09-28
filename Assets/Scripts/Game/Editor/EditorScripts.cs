using UnityEditor;
using UnityEngine;

using TVB.Core.Localization;

public class EditorScripts : MonoBehaviour
{
    [MenuItem("TVB/Translation/Reload")]
    public static void ReloadTranslations()
    {
        TextDatabase.Reload();
    }

    [MenuItem("TVB/Translation/Change to Czech")]
    public static void SetCzechTranslation()
    {
        TextDatabase.InitializeLanguage(ELanguage.Czech);
    }

    [MenuItem("TVB/Translation/Change to English")]
    public static void SetEnglishTranslation()
    {
        TextDatabase.InitializeLanguage(ELanguage.English);
    }
}
