namespace TVB.Core
{
    using System.Collections.Generic;

    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class Game : MonoBehaviour
    {
        // SINGLETON IMPLEMENTATION

        protected static Game instance;

        public static Game Instance
        {
            get
            {
                return instance;
            }
        }

        // PUBLIC MEMBERS

        public Scene Scene { get => m_Scene; }

        // PRIVATE MEMBERS

        private List<GameService> m_GameServices = new List<GameService>(10);
        private Scene             m_Scene;

        // MONOBEHAVIOUR INTERFACE

        private void Awake()
        {
            if (instance != null)
            {
                DestroyImmediate(this);
                return;
            }

            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }

        private void Start()
        {
            // Find first scene

            OnInitialized();
            InitializeScene();

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode loadMode)
        {
            InitializeScene();
            OnSceneLoaded();
        }

        private void InitializeScene()
        {
            m_Scene = FindObjectOfType<Scene>();

            if (m_Scene == null)
            {
                Debug.LogError("There is no active Scene object in current scene!");
                return;
            }
            m_Scene.Initialize();
        }

        private void Update()
        {
            if (m_Scene != null)
            {
                m_Scene.OnUpdate();
            }

            for (int idx = 0, count = m_GameServices.Count; idx < count; ++idx)
            {
                m_GameServices[idx].OnUpdate();
            }

            OnUpdate();
        }

        private void LateUpdate()
        {
            if (m_Scene != null)
            {
                m_Scene.OnLateUpdate();
            }

            OnLateUpdate();
        }

        private void OnDestroy()
        {
            if (m_Scene != null)
            {
                m_Scene.Deinitialize();
                m_Scene = null;
            }

            OnDeinitialized();
        }

        // PROTECTED METHODS

        protected virtual void OnInitialized() { }
        protected virtual void OnUpdate() { }
        protected virtual void OnLateUpdate() { }
        protected virtual void OnDeinitialized() { }
        protected virtual void OnSceneLoaded() { }
    }
}

