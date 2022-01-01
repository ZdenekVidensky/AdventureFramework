namespace TVB.Game.GUI
{
    using System.Collections.Generic;
    using UnityEngine;

    using TVB.Core.GUI;
    using TVB.Game.Save;
    using TVB.Game.Utilities;

    public class GUISaveSlotsList : GUIComponent
    {
        private bool m_IsLoadMenu;
        private GUISaveSlot[] SaveSlots;

        public override void OnInitialized()
        {
            base.OnInitialized();

            SaveSlots = GetComponentsInChildren<GUISaveSlot>();
        }

        public void InitializeSaveSlots(bool isLoadMenu)
        {
            m_IsLoadMenu = isLoadMenu;
            List<GUISaveData> saveDataList = SaveUtility.GetSaveData(true);
            int dataCount = saveDataList.Count;

            for (int idx = 0; idx < SaveSlots.Length; idx++)
            {
                GUISaveSlot slot = SaveSlots[idx];
                slot.OnClick.RemoveAllListeners();
            }

            for (int idx = 0; idx < SaveSlots.Length; idx++)
            {
                GUISaveSlot slot = SaveSlots[idx];

                if (idx >= dataCount)
                {
                    slot.SetIsEmpty();
                    slot.SetIsEnabled(m_IsLoadMenu == false);

                    continue;
                }

                GUISaveData data = saveDataList[idx];

                slot.Initialize(data);
                slot.SetIsEnabled(true);

                slot.OnClick.AddListener(OnSaveSlotClick);
            }
        }

        private void OnSaveSlotClick(string saveFileName, GUISaveSlot clickedSlot)
        {
            // Load game

            if (m_IsLoadMenu == true)
            {
                SaveData saveData = SaveUtility.LoadGame(saveFileName);
                AdventureGame.Instance.LoadSavedGame(saveData);

                return;
            }

            string finalFileName = $"{System.Array.IndexOf(SaveSlots, clickedSlot)}{SaveUtility.DEFAULT_NAME}";
            AdventureGame.Instance.SaveGame(finalFileName);
            InitializeSaveSlots(m_IsLoadMenu);
        }
    }
}
