namespace TVB.Game.GUI
{
    using System.Collections.Generic;
    using UnityEngine;

    using TVB.Core.GUI;
    using TVB.Game.Save;

    public class GUISaveSlotsList : GUIComponent
    {
        [SerializeField]
        private bool m_IsLoadMenu = true;

        private GUISaveSlot[] SaveSlots;

        public override void OnInitialized()
        {
            base.OnInitialized();

            SaveSlots = GetComponentsInChildren<GUISaveSlot>();
        }

        public void InitializeSaveSlots()
        {
            List<GUISaveData> saveDataList = SaveSystem.GetSaveData(true);
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

                    if (m_IsLoadMenu == true)
                    {
                        slot.SetIsEnabled(false);
                    }

                    continue;
                }

                GUISaveData data = saveDataList[idx];

                slot.Initialize(data);
                slot.SetIsEnabled(true);

                slot.OnClick.AddListener(OnSaveSlotClick);
            }
        }

        private void OnSaveSlotClick(string saveFileName)
        {
            if (m_IsLoadMenu == true)
            {
                SaveData saveData = SaveSystem.LoadGame(saveFileName);
                AdventureGame.Instance.LoadSavedGame(saveData);

                return;
            }
        }
    }
}
