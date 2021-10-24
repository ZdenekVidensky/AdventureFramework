using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using System.Collections;
using System.Collections.Generic;
using TVB.Game;
using UnityEditor;
using UnityEngine;


public class ItemsEditorWindow : OdinMenuEditorWindow
{
    private const string ITEMS_PATH = "Assets/Prefabs/Items/";
    private CreateNewItem m_CreateNewItemInstance;

    [MenuItem("TVB/Items Editor")]
    private static void OpenWindow()
    {
        GetWindow<ItemsEditorWindow>().Show();
    }
    protected override OdinMenuTree BuildMenuTree()
    {
        OdinMenuTree tree = new OdinMenuTree();

        m_CreateNewItemInstance = new CreateNewItem();
        tree.Add("Create new item", m_CreateNewItemInstance);
        tree.AddAllAssetsAtPath("Items", ITEMS_PATH, typeof(InventoryItem));

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
                InventoryItem asset = selected.SelectedValue as InventoryItem;
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
            Item = ScriptableObject.CreateInstance<InventoryItem>();
            Item.ID = "NewItem";
        }

        [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
        public InventoryItem Item;

        [Button("Add new item")]
        private void Create()
        {
            AssetDatabase.CreateAsset(Item, $"{ITEMS_PATH}{Item.ID}.asset");
            AssetDatabase.SaveAssets();
        }
    }
}
