namespace TVB.Game.GUI
{
    using System.Collections.Generic;
    using UnityEngine;

    using TVB.Core.GUI;
    using TVB.Game.Save;
    using TVB.Game.Utilities;
    using System.Linq;

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
            List<GUISaveData> saveDataList = SaveUtility.GetSaveData(isLoadMenu == true);
            int dataCount = saveDataList.Count;

            for (int idx = 0; idx < SaveSlots.Length; idx++)
            {
                GUISaveSlot slot = SaveSlots[idx];
                slot.OnClick.RemoveAllListeners();

                slot.SetIsEmpty();
                slot.SetIsEnabled(m_IsLoadMenu == false);

                slot.OnClick.AddListener(OnSaveSlotClick);
            }

            bool containsAutosave = false;
            bool containsQuicksave = false;

            for(int idx = 0, count = saveDataList.Count; idx < count; idx++)
            {
                GUISaveData data = saveDataList[idx];
                int index = data.Index;

                if (isLoadMenu == false)
                {
                    index -= 2;
                }

                if (index >= SaveSlots.Length)
                    break;

                GUISaveSlot slot = SaveSlots[index];
                slot.Initialize(data);
                slot.SetIsEnabled(true);

                containsAutosave  |= data.IsAutosave;
                containsQuicksave |= data.IsQuicksave;
            }

            // Hide autosave and quicksave slots if there is no any

            if (isLoadMenu == true && containsAutosave == false)
            {
                SaveSlots[0].SetActive(false);
            }

            if (isLoadMenu == true && containsQuicksave == false)
            {
                SaveSlots[1].SetActive(false);
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

            string finalFileName = $"{System.Array.IndexOf(SaveSlots, clickedSlot) + 2}{SaveUtility.DEFAULT_NAME}";
            AdventureGame.Instance.SaveGame(finalFileName);
            InitializeSaveSlots(m_IsLoadMenu);
        }
    }
}
