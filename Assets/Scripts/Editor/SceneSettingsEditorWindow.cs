using UnityEditor;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using TVB.Game;


public class SceneSettingsEditorWindow : OdinMenuEditorWindow
{
    private const string ITEMS_PATH = "Assets/Prefabs/SceneSettings/";
    private CreateNewItem m_CreateNewItemInstance;

    [MenuItem("TVB/Scene Settings Editor")]
    private static void OpenWindow()
    {
        GetWindow<SceneSettingsEditorWindow>().Show();
    }
    protected override OdinMenuTree BuildMenuTree()
    {
        OdinMenuTree tree = new OdinMenuTree();

        m_CreateNewItemInstance = new CreateNewItem();
        tree.Add("Create new scene settings", m_CreateNewItemInstance);
        tree.AddAllAssetsAtPath("Scene Settings", ITEMS_PATH, typeof(SceneSettings));

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
                SceneSettings asset = selected.SelectedValue as SceneSettings;
                string path = AssetDatabase.GetAssetPath(asset);
                AssetDatabase.DeleteAsset(path);
                AssetDatabase.SaveAssets();
            }
        }
        SirenixEditorGUI.EndHorizontalToolbar();
    }

    public class CreateNewItem
    {
        public CreateNewItem()
        {
            Item = ScriptableObject.CreateInstance<SceneSettings>();
            Item.SceneName = "NewSceneName";
        }

        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public SceneSettings Item;

        [Button("Add new scene settings")]
        private void Create()
        {
            AssetDatabase.CreateAsset(Item, $"{ITEMS_PATH}{Item.SceneName}.asset");
            AssetDatabase.SaveAssets();
        }
    }
}
