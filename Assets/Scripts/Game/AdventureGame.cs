﻿namespace TVB.Game
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using TVB.Core;
    using TVB.Core.Attributes;
    using TVB.Core.Audio;
    using TVB.Core.Graph;
    using TVB.Core.Interactable;
    using TVB.Core.Localization;
    using TVB.Game.GameSignals;
    using TVB.Game.GUI;
    using TVB.Game.Save;
    using TVB.Game.Scenes;
    using TVB.Game.Utilities;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using Scene = Core.Scene;

    public class AdventureGame : Game
    {
        // CONFIGURATION

        [SerializeField]
        private ELanguage m_Language = ELanguage.Czech;
        [SerializeField]
        private AudioManager m_AudioManager;

        [SerializeField]
        private List<Achievement> m_AchievementsDatabase = new List<Achievement>(32);
        [SerializeField]
        private List<InventoryItem> m_InventoryItemsDatabase;

        [Header("Cheats")]
        [SerializeField]
        private bool m_EnableCheats = true;
        [GetComponent(), SerializeField, HideInInspector]
        private List<InventoryItem> m_StartingItems;

        // PUBLIC MEMBERS

        public Inventory Inventory => m_Inventory;
        public Dictionary<string, bool> Conditions => m_Conditions;
        public List<string> UnlockedAchievements => m_UnlockedAchievements;
        public GraphManager GraphManager => m_GraphManager;
        public AudioManager AudioManager => m_AudioManager;
        public Player Player
        {
            get
            {
                if (m_Player == null)
                {
                    m_Player = FindObjectOfType<Player>(true);
                }

                return m_Player;
            }
        }

        public bool IsBusy
        {
            get
            {
                return m_IsBusy;
            }

            set
            {
                Signals.GUISignals.GameBusyChanged.Emit(value);
                m_IsBusy = value;
            }
        }

        public bool IsInventoryOpen
        {
            get
            {
                return m_IsInventoryOpen;
            }

            set
            {
                m_IsInventoryOpen = value;
                Signals.GUISignals.SetActivePlacesVisible.Emit(false);
                Signals.GUISignals.SetInventoryOpen.Emit(value);
            }
        }

        public bool AreActivePlacesVisible
        {
            get
            {
                return m_AreActivePlacesVisible;
            }

            set
            {
                m_AreActivePlacesVisible = value;
                Signals.GUISignals.SetActivePlacesVisible.Emit(value);
                Signals.GUISignals.SetInventoryOpen.Emit(false);
                //IsGamePaused = value;
            }
        }

        public string SelectedItemID
        {
            get
            {
                return m_SelectedItem;
            }

            set
            {
                m_SelectedItem = value;
            }
        }

        public string HoveredItemID
        {
            get
            {
                return m_HoveredItem;
            }

            set
            {
                m_HoveredItem = value;
            }
        }

        public bool IsGamePaused
        {
            get => m_IsGamePaused;
            set
            {
                m_IsGamePaused = value;
                OnGamePauseChanged();
            }
        }

        public bool GameEnded
        {
            get => m_GameEnded;
            set
            {
                m_GameEnded = value;
                OnGameEndedChanged();
            }
        }

        public string PreviousScene
        {
            get => m_PreviousScene;
        }

        public AdventureScene CurrentScene
        {
            get => Scene as AdventureScene;
        }

        public static new AdventureGame Instance
        {
            get
            {
                return instance as AdventureGame;
            }
        }

        // PRIVATE MEMBERS

        private Inventory m_Inventory = new Inventory();
        private Dictionary<string, bool> m_Conditions = new Dictionary<string, bool>(64);
        private List<string> m_UnlockedAchievements = new List<string>(16);
        private bool m_GameEnded = false;
        private bool m_IsGamePaused = false;
        private GraphManager m_GraphManager = null;
        private Player m_Player = null;
        private bool m_IsBusy = false;
        private bool m_IsInventoryOpen = false;
        private bool m_AreActivePlacesVisible = false;
        private string m_SelectedItem = null;
        private string m_HoveredItem = null;
        private string m_PreviousScene = null;
        private SaveData m_PendingSaveData = null;

        // GAME INTERFACE

        protected override void OnInitialized()
        {
            base.OnInitialized();

            TextDatabase.InitializeLanguage(m_Language);
            InitializeGraphManager();

            if (m_EnableCheats == true)
            {
                Inventory.AddItems(m_StartingItems);
            }

            if (m_AudioManager != null)
            {
                m_AudioManager.Initialize();
            }

            // TODO:
            //Cursor.SetCursor(m_GUISettings.CursorIcon, Vector2.zero, CursorMode.ForceSoftware);
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            UpdateInput();
        }

        protected override void OnSceneLoaded()
        {
            base.OnSceneLoaded();

            m_GraphManager.InitializeOnSceneLoaded();
            m_Player = FindObjectOfType<Player>();

            if (m_PendingSaveData != null)
            {
                Player.transform.position = new Vector3(m_PendingSaveData.PositionX, m_PendingSaveData.PositionY, Player.Position.z);
                Player.SetDirection((EDirection)m_PendingSaveData.Direction);
                m_PendingSaveData = null;
            }
        }

        // PUBLIC METHODS

        public void ProcessInteractiveGraph(InteractiveGraph graph, IInteractable interactableObject = null)
        {
            StartCoroutine(m_GraphManager.ProcessInteractiveGraph(graph, interactableObject));
        }

        public void InitializeGraphManager()
        {
            m_GraphManager = GetComponent<GraphManager>();

            if (m_GraphManager == null)
            {
                Debug.LogError("There is no Graph manager in the scene!");
                return;
            }

            m_GraphManager.Initialize();
        }

        public void SetCondition(string name, bool condition)
        {
            m_Conditions[name] = condition;
        }

        public bool GetCondition(string name)
        {
            m_Conditions.TryGetValue(name, out bool result);
            return result;
        }

        public void UnlockAchievement(string ID)
        {
            Achievement achievement = m_AchievementsDatabase.FirstOrDefault(m => m.ID == ID);

            if (achievement == null)
            {
                Debug.LogError($"Achievement with ID {ID} doesn't exist in database!");
                return;
            }

            if (m_UnlockedAchievements.Contains(ID) == true)
                return;

            m_UnlockedAchievements.Add(ID);

            Signals.GUISignals.UnlockAchievement.Emit(achievement);
        }

        public void TryToUseItem(string selectedItemID)
        {
            if (IsBusy == true)
                return;

            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                IInteractable interactableItem = hit.collider.gameObject.GetComponent<IInteractable>();

                if (interactableItem != null)
                {
                    interactableItem.OnUseItem(selectedItemID);
                }
            }
        }

        public void LoadSceneAsync(string sceneName)
        {
            StartCoroutine(LoadSceneAsync_Coroutine(sceneName));
        }

        public void LoadSavedGame(SaveData saveData)
        {
            StartCoroutine(LoadSceneAsync_Coroutine(saveData.SceneName, saveData));
        }

        public void SaveGame(string saveFileName)
        {
            SaveUtility.SaveGame(Player.Position, Player.Direction, Inventory.Items, Conditions, Scene.SceneName, saveFileName, Scene.SceneNameID);
        }

        private IEnumerator LoadSceneAsync_Coroutine(string sceneName, SaveData saveData = null)
        {
            IsBusy = true;

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            asyncLoad.allowSceneActivation = false;

            while (asyncLoad.isDone == false)
            {
                if (asyncLoad.progress >= 0.9f)
                {
                    Scene scene = Scene;
                    m_PreviousScene = scene.SceneName;
                    yield return scene.FadeOut(0.3f);
                    scene.Deinitialize();

                    if (saveData != null)
                    {
                        ApplySaveData(saveData);
                        m_PendingSaveData = saveData; // Save save data for scene (player)
                    }

                    asyncLoad.allowSceneActivation = true;
                    break;
                }

                yield return null;
            }

            IsGamePaused = false;
        }

        // PRIVATE METHODS

        private void ApplySaveData(SaveData saveData)
        {
            Conditions.Clear();

            for (int idx = 0; idx < saveData.Conditions.Length; idx++)
            {
                ConditionSaveData item = saveData.Conditions[idx];
                Conditions[item.Name] = item.Value;
            }

            Inventory.Items.Clear();
            // TODO: Refresh inventory GUI

            for (int idx = 0; idx < saveData.InventoryItems.Length; idx++)
            {
                string itemID = saveData.InventoryItems[idx];

                InventoryItem item = m_InventoryItemsDatabase.FirstOrDefault(m => m.ID == itemID);

                if (item == null)
                {
                    Debug.LogError($"Item with ID ${itemID} is not in the database!");
                    continue;
                }

                Inventory.AddItem(item);
            }
        }

        private void UpdateInput()
        {
            if (Input.GetMouseButtonUp(0) == true)
            {
                m_GraphManager.OnSkipPerformed();
            }

            if (Input.GetButtonDown(InputNamesUtility.QUICK_SAVE) == true)
            {
                SaveGame(SaveUtility.QUICKSAVE_NAME);

                Debug.LogError("Quicksaved");
            }

            if (Input.GetButtonDown(InputNamesUtility.QUICK_LOAD) == true)
            {
                SaveData saveData = SaveUtility.LoadGame(SaveUtility.QUICKSAVE_NAME);

                if (saveData == null)
                {
                    Debug.LogError("There is no quick save file");
                    return;
                }

                StartCoroutine(LoadSceneAsync_Coroutine(saveData.SceneName, saveData));
            }

            if (IsBusy == false && Player != null)
            {
                if (Input.GetButtonDown(InputNamesUtility.MENU) == true)
                {
                    if (IsInventoryOpen == true)
                    {
                        IsInventoryOpen = false;
                        return;
                    }

                    System.Type closedViewType = Scene.Frontend.CloseTopView(typeof(GUIIngameView));

                    if (closedViewType != null && closedViewType != typeof(GUIIngameMenuView))
                        return;

                    if (IsGamePaused == true)
                    {
                        IsGamePaused = false;
                    }
                    else
                    {
                        Scene.Frontend.OpenView<GUIIngameMenuView>();
                        IsGamePaused = true;
                    }
                }
            }
        }

        private void OnGamePauseChanged()
        {
            if (m_IsGamePaused == false)
            {
                Time.timeScale = 1f;
            }
            else
            {
                Time.timeScale = 0.001f;
            }
        }

        private void OnGameEndedChanged()
        {
            if (m_GameEnded == true)
            {
                // TODO: Something before end
#if UNITY_EDITOR
                EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
            }
        }
    }
}

