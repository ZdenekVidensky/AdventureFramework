using UnityEditor;
using UnityEngine;
using System;

using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using TVB.Core.Localization;

public class LocalizationEditorWindow : OdinMenuEditorWindow
{
    private const string ITEMS_PATH = "Assets/Prefabs/Localization/";
    private CreateNewItem m_CreateNewItemInstance;

    public string m_FileName;

    [MenuItem("TVB/Localization Editor")]
    private static void OpenWindow()
    {
        GetWindow<LocalizationEditorWindow>().Show();
    }

    protected override OdinMenuTree BuildMenuTree()
    {
        OdinMenuTree tree = new OdinMenuTree();

        m_CreateNewItemInstance = new CreateNewItem();
        tree.Add("Create new item", m_CreateNewItemInstance);
        tree.AddAllAssetsAtPath("Items", ITEMS_PATH, typeof(LocalizedTexts));

        return tree;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (m_CreateNewItemInstance != null)
        {
            DestroyImmediate(m_CreateNewItemInstance.Item);
        }
    }

    protected override void OnBeginDrawEditors()
    {
        base.OnBeginDrawEditors();

        OdinMenuTreeSelection selected = this.MenuTree.Selection;

        SirenixEditorGUI.BeginHorizontalToolbar();
        {

            if (SirenixEditorGUI.ToolbarButton("Delete current") == true)
            {
                LocalizedTexts asset = selected.SelectedValue as LocalizedTexts;
                string path = AssetDatabase.GetAssetPath(asset);
                AssetDatabase.DeleteAsset(path);
                AssetDatabase.SaveAssets();
            }

            if (SirenixEditorGUI.ToolbarButton("Reload texts") == true)
            {
                TextDatabase.Reload();
            }

            foreach(ELanguage language in Enum.GetValues(typeof(ELanguage)))
            {
                if (SirenixEditorGUI.ToolbarButton($"Switch to {language}") == true)
                {
                    TextDatabase.InitializeLanguage(language);
                }
            }

            SirenixEditorGUI.BeginBox($"Current language: {TextDatabase.CurrentLanguage}");
            SirenixEditorGUI.EndBox();
        }
        SirenixEditorGUI.EndHorizontalToolbar();
    }

    public class CreateNewItem
    {
        public CreateNewItem()
        {
            Item = ScriptableObject.CreateInstance<LocalizedTexts>();
            Item.Name = "NewLocalizationTexts";
        }

        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public LocalizedTexts Item;

        [Button("Add new item")]
        private void Create()
        {
            AssetDatabase.CreateAsset(Item, $"{ITEMS_PATH}{Item.Name}.asset");
            AssetDatabase.SaveAssets();
        }
    }
}
