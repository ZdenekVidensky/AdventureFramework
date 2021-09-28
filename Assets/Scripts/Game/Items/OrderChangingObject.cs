namespace Levels
{
    using UnityEngine;
    using UnityEngine.Assertions;

    public class OrderChangingObject : MonoBehaviour
    {
        // CONFIGURATION

        [SerializeField] private Transform m_Target;
        [SerializeField] private SpriteRenderer m_SpriteRenderer;
        [SerializeField] private int m_BehindOrder;
        [SerializeField] private int m_FrontOrder;

        // PRIVATE MEMBERS

        private Transform m_Transform;

        // MONOBEHAVIOUR

        void Awake()
        {
            Assert.IsNotNull(m_Target);
            Assert.IsNotNull(m_SpriteRenderer);

            m_Transform = transform;
        }

        void Update()
        {
            if (m_Target == null || m_SpriteRenderer == null)
                return;

            if (m_Target.position.y >= m_Transform.position.y)
            {
                if (m_SpriteRenderer.sortingOrder != m_FrontOrder)
                {
                    m_SpriteRenderer.sortingOrder = m_FrontOrder;
                }
            }
            else
            {
                if (m_SpriteRenderer.sortingOrder != m_BehindOrder)
                {
                    m_SpriteRenderer.sortingOrder = m_BehindOrder;
                }
            }
        }
    }
}
