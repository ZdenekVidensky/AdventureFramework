namespace TVB.Game.Items
{
    using System.Collections;

    using UnityEngine;

    using TVB.Game.Interactable;

    public class SecretBush : InteractableItem
    {
        // CONSTANTS

        private const float FINAL_Y = -2.36f;

        // CONFIGURATION

        [SerializeField]
        private float       m_Speed = 2f;
        [Header("Sounds")]
        [SerializeField]
        private AudioClip   m_SwordSound;
        [SerializeField]
        private AudioClip   m_BushSound;
        [SerializeField]
        private AudioSource m_AudioSource;

        // PRIVATE MEMBERS

        private Transform m_Transform;

        // PUBLIC METHODS

        public void ShowPath()
        {
            StartCoroutine(ShowPath_Coroutine());
        }

        // PRIVATE METHODS

        private IEnumerator ShowPath_Coroutine()
        {
            m_AudioSource.PlayOneShot(m_SwordSound);

            while(m_AudioSource.isPlaying == true)
                yield return null;

            m_AudioSource.PlayOneShot(m_BushSound);

            while(m_Transform.position.y > FINAL_Y)
            {
                m_Transform.Translate(Vector3.down * Time.deltaTime * m_Speed);
                yield return null;
            }
        }

        // MONOBEHAVIOUR INTERFACE

        private void Awake()
        {
            m_Transform = transform;
        }
    }
}
