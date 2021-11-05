namespace TVB.Game
{
    using System.Collections.Generic;
    using UnityEngine;

    using TVB.Game.Interactable;
    using TVB.Core.Attributes;
    using Sirenix.OdinInspector;

    public class Player : MonoBehaviour, ITalkable
    {
        // CONFIGURATION

        [SerializeField]
        private float          m_MovementSpeed = 5f;
        [SerializeField]
        private float          m_StoppingDistance = 0.01f;

        // PUBLIC MEMBERS

        public Vector3 Position => m_Transform.position;
        public bool IsGoing     => m_CurrentPath != null;

        // ANIMATOR PARAMETERS

        private static int DIRECTION_ANIMATOR_PARAMETER = Animator.StringToHash("Direction");

        ETalkableCharacter ITalkable.Character => ETalkableCharacter.Player;

        // PRIVATE MEMBERS


        private float          m_ScaleLevelsDistance;
        private SceneSettings  m_SceneSettings;

        private List<Vector2>  m_CurrentPath = null;
        private int            m_CurrentPathIndex;
        private Vector3        m_CurrentDestinationPoint;
        private Vector3        m_CurrentDirection;

        private EDirection     m_Direction;
        [GetComponent(true), SerializeField, HideInInspector]
        private Transform      m_Transform;
        [GetComponent(true), SerializeField, HideInInspector]
        private Animator       m_Animator;
        [GetComponent(true), SerializeField, HideInInspector]
        private SpriteRenderer m_SpriteRenderer;

        void Update()
        {
            if (AdventureGame.Instance.IsInventoryOpen == true)
                return;

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
                    SetDirection(item.Direction);
                }
            }
        }

        public void GoTo(Vector2 destinationPoint)
        {
            List<Vector2> path        = new List<Vector2>(1) { destinationPoint };
            m_CurrentPathIndex        = 0;
            m_CurrentDestinationPoint = path[m_CurrentPathIndex];
            m_CurrentDirection        = (m_CurrentDestinationPoint - m_Transform.position);

            SetDirection(CalculateNewDirection(m_CurrentDirection));

            m_CurrentPath = path;
        }

        public void GoTo(List<Vector2> path)
        {
            m_CurrentPathIndex        = 0;
            m_CurrentDestinationPoint = path[m_CurrentPathIndex];
            m_CurrentDirection        = (m_CurrentDestinationPoint - m_Transform.position);

            SetDirection(CalculateNewDirection(m_CurrentDirection));
            m_CurrentPath             = path;
        }

        public void SkipTo(Vector3 destinationPoint)
        {
            m_Transform.position      = destinationPoint;
            m_CurrentDirection        = (destinationPoint - m_Transform.position);

            SetDirection(CalculateNewDirection(m_CurrentDirection));

            m_CurrentPath             = null;
            m_CurrentDestinationPoint = Vector3.zero;
            m_CurrentDestinationPoint = Vector3.zero;
        }

        public void SetDirection(EDirection newDirection)
        {
            m_Direction = newDirection;

            m_SpriteRenderer.flipX = m_Direction == EDirection.Right;
            m_Animator.SetInteger(DIRECTION_ANIMATOR_PARAMETER, (int)m_Direction);
        }

        // PRIVATE METHODS

        private void UpdateMovement()
        {
            if (m_CurrentPath == null)
                return;

            m_Transform.position += m_CurrentDirection.normalized * (m_MovementSpeed * Time.deltaTime);

            if (Vector2.Distance(m_Transform.position, m_CurrentDestinationPoint) <= m_StoppingDistance)
            {
                if (m_CurrentPathIndex == (m_CurrentPath.Count - 1))
                {
                    m_CurrentPath = null;
                    m_CurrentPathIndex = 0;
                }
                else
                {
                    m_CurrentPathIndex       += 1;
                    m_CurrentDestinationPoint = m_CurrentPath[m_CurrentPathIndex];
                    m_CurrentDirection        = (m_CurrentDestinationPoint - m_Transform.position);
                    SetDirection(CalculateNewDirection(m_CurrentDirection));
                }
            }
        }

        private EDirection CalculateNewDirection(Vector3 direction)
        {
            float absX = Mathf.Abs(direction.x);
            float absY = Mathf.Abs(direction.y);

            if (absX > absY)
            {
                // Left or right

                if (direction.x > 0)
                    return EDirection.Right;
                else if (direction.x < 0)
                    return EDirection.Left;
            }
            else
            {
                // Up or down

                if (direction.y > 0)
                    return EDirection.Up;
                else if (direction.y < 0)
                    return EDirection.Down;
            }

            return EDirection.Down;
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

    public enum EDirection
    {
        Down,
        Up,
        Left,
        Right
    }
}
