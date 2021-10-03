namespace TVB.Game
{
    using UnityEngine;

    public class Player : MonoBehaviour
    {
        // CONFIGURATION

        //[SerializeField]
        private SceneSettings m_SceneSettings;

        // PUBLIC MEMBERS

        public Vector3 Position => m_Transform.position;

        // PRIVATE MEMBERS

        private Transform     m_Transform;
        private float         m_ScaleLevelsDistance;
        private bool          m_CanWalk;
        private bool          m_IsBusy;

        void Awake()
        {
            m_Transform           = GetComponent<Transform>();
        }

        void Update()
        {
            UpdateScale();
        }

        // PUBLIC METHODS

        public void SetSceneSettings(SceneSettings sceneSettings)
        {
            m_SceneSettings = sceneSettings;
            m_ScaleLevelsDistance = Mathf.Abs(m_SceneSettings.BottomScale.YPosition - m_SceneSettings.TopScale.YPosition);
        }

        // PRIVATE METHODS

        private void UpdateScale()
        {
            if (m_SceneSettings == null)
                return;

            float percent = m_Transform.position.y / m_ScaleLevelsDistance;
            float scale = Mathf.Lerp(m_SceneSettings.BottomScale.Scale, m_SceneSettings.TopScale.Scale, percent);

            m_Transform.localScale = new Vector3(scale, scale, m_Transform.localScale.z);
        }
    }
}
