namespace TVB.Game.Items
{
    using UnityEngine;

    public class Flame : MonoBehaviour
    {
        // CONFIGURATION

        [SerializeField]
        private float m_Speed;
        [SerializeField]
        private float m_MinIntensity;
        [SerializeField]
        private float m_MaxIntensity;
        [SerializeField]
        private Light m_Light;

        // PRIVATE MEMBERS

        private float m_Parameter;

        // MONOBEHAVIOUR INTERFACE

        void Update()
        {
            m_Parameter      += Time.deltaTime * m_Speed;
            float t           = Mathf.Cos(m_Parameter);
            m_Light.intensity = Mathf.Lerp(m_MinIntensity, m_MaxIntensity, Mathf.Abs(t));

            if (m_Parameter > 100f)
            {
                m_Parameter = 0f;
            }
        }
    }
}
