namespace TVB.Game
{
    using System.Collections.Generic;
    using UnityEngine;

    using TVB.Game.Interactable;

    public class Player : MonoBehaviour, ITalkable
    {
        // CONFIGURATION

        [SerializeField]
        private float m_MovementSpeed = 5f;
        [SerializeField]
        private float m_StoppingDistance = 0.01f;

        //[SerializeField]
        private SceneSettings m_SceneSettings;

        // PUBLIC MEMBERS

        public Vector3 Position => m_Transform.position;
        public bool IsGoing     => m_CurrentPath != null;

        ETalkableCharacter ITalkable.Character => ETalkableCharacter.Player;

        // PRIVATE MEMBERS

        private Transform     m_Transform;
        private float         m_ScaleLevelsDistance;

        private List<Vector2> m_CurrentPath = null;
        private int           m_CurrentPathIndex;

        void Awake()
        {
            m_Transform           = GetComponent<Transform>();
        }

        void Update()
        {
            UpdateMovement();
            UpdateScale();
        }

        // PUBLIC METHODS

        public void SetSceneSettings(SceneSettings sceneSettings)
        {
            m_SceneSettings       = sceneSettings;
            m_ScaleLevelsDistance = Mathf.Abs(m_SceneSettings.BottomScale.YPosition - m_SceneSettings.TopScale.YPosition);

            // Set position based on previous scene

            for (int idx = 0; idx < sceneSettings.PreviousSceneLocations.Length; idx++)
            {
                PreviousSceneLocation item = sceneSettings.PreviousSceneLocations[idx];

                if (item.SceneName == AdventureGame.Instance.PreviousScene)
                {
                    m_Transform.position = item.Position;
                }
            }
        }

        public void GoTo(Vector2 destinationPoint)
        {
            m_CurrentPath      = new List<Vector2>(1) { destinationPoint };
            m_CurrentPathIndex = 0;
        }

        public void GoTo(List<Vector2> path)
        {
            m_CurrentPath        = path;
            m_CurrentPathIndex   = 0;
        }

        public void SkipTo(Vector2 destinationPoint)
        {
            m_CurrentPath        = null;
            m_Transform.position = destinationPoint;
        }

        // PRIVATE METHODS

        private void UpdateMovement()
        {
            if (m_CurrentPath == null)
                return;

            Vector3 destinationPoint = m_CurrentPath[m_CurrentPathIndex];
            Vector3 direction        = (destinationPoint - m_Transform.position);

            m_Transform.position += direction.normalized * (m_MovementSpeed * Time.deltaTime);

            if (Vector2.Distance(m_Transform.position, destinationPoint) <= m_StoppingDistance)
            {
                if (m_CurrentPathIndex == (m_CurrentPath.Count - 1))
                {
                    m_CurrentPath = null;
                    m_CurrentPathIndex = 0;
                }

                m_CurrentPathIndex += 1;
            }
        }

        private void UpdateScale()
        {
            if (m_SceneSettings == null)
                return;

            float percent = Mathf.Abs(m_Transform.position.y / m_ScaleLevelsDistance);
            float scale = Mathf.Lerp(m_SceneSettings.BottomScale.Scale, m_SceneSettings.TopScale.Scale, percent);

            m_Transform.localScale = new Vector3(scale, scale, m_Transform.localScale.z);
        }

        void ITalkable.SetIsTalking(bool state)
        {
            // TODO: talking animation
        }
    }
}
