namespace TVB.Game.GUI
{
    using UnityEngine;
    using UnityEngine.UI;

    using TVB.Core.GUI;

    public class GUIActivePlaces : GUIComponent
    {
        // CONFIGURATION

        [Header("Icons")]
        [SerializeField]
        private Image m_WalkIcon;
        [SerializeField]
        private Image m_TalkIcon;
        [SerializeField]
        private Image m_LookIcon;
        [SerializeField]
        private Image m_TakeIcon;

        // PRIVATE MEMBERS

        private void Start()
        {
            Instantiate<Image>(m_WalkIcon, RectTransform);

            this.SetActive(false);
        }
    }
}
