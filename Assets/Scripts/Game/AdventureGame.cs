namespace TVB.Game
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Collections;

    using UnityEngine;
    using UnityEditor;
    using UnityEngine.SceneManagement;

    using TVB.Core;
    using TVB.Core.Localization;
    using TVB.Game.GameSignals;
    using TVB.Game.Graph;
    using TVB.Game.Interactable;

    public class AdventureGame : Game
    {
        // CONFIGURATION

        [SerializeField]
        private ELanguage m_Language = ELanguage.Czech;

        [SerializeField]
        private List<Achievement> AchievementsDatabase = new List<Achievement>(32);

        // PUBLIC MEMBERS

        public Inventory Inventory => m_Inventory;
        public Dictionary<string, bool> Conditions => m_Conditions;
        public List<string> UnlockedAchievements => m_UnlockedAchievements;
        public GraphManager GraphManager => m_GraphManager;
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
            get => m_IsInventoryOpen;
            set => m_IsInventoryOpen = value;
        }

        public bool GamePaused
        {
            get => m_GamePaused;
            set
            {
                m_GamePaused = value;
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

        public static new AdventureGame Instance
        {
            get
            {
                return instance as AdventureGame;
            }
        }

        // PRIVATE MEMBERS

        private Inventory                     m_Inventory            = new Inventory();
        private Dictionary<string, bool>      m_Conditions           = new Dictionary<string, bool>(64);
        private List<string>                  m_UnlockedAchievements = new List<string>(16);
        private bool                          m_GameEnded            = false;
        private bool                          m_GamePaused           = false;
        private GraphManager                  m_GraphManager         = null;
        private Player                        m_Player               = null;
        private bool                          m_IsBusy               = false;
        private bool                          m_IsInventoryOpen      = false;

        // GAME INTERFACE

        protected override void OnInitialized()
        {
            base.OnInitialized();

            TextDatabase.InitializeLanguage(m_Language);
            InitializeGraphManager();

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

            m_GraphManager.Initialize();
            m_Player = FindObjectOfType<Player>();
        }

        // PUBLIC METHODS

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
            Achievement achievement = AchievementsDatabase.FirstOrDefault(m => m.ID == ID);

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

        public IEnumerator LoadSceneAsync(string sceneName)
        {
            IsBusy = true;

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            asyncLoad.allowSceneActivation = false;

            while (asyncLoad.isDone == false)
            {
                if (asyncLoad.progress >= 0.9f)
                {
                    AdventureScene scene = Scene as AdventureScene;
                    yield return scene.FadeOut(0.3f);
                    scene.Deinitialize();
                    asyncLoad.allowSceneActivation = true;
                    break;
                }

                yield return null;
            }
        }

        // PRIVATE METHODS

        private void UpdateInput()
        {
            if (Input.GetMouseButtonDown(0) == true)
            {
                m_GraphManager.OnSkipPerformed();
            }
        }

        private void OnGamePauseChanged()
        {
            if (m_GamePaused == false)
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

