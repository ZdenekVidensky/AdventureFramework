namespace TVB.Game.GUI
{
    using TVB.Core.Attributes;
    using TVB.Core.GUI;
    using TVB.Core.Localization;
    using UnityEngine;
    using UnityEngine.UI;
    using GUIText = TMPro.TextMeshProUGUI;

    public class GUIAchievementUnlockedView : GUIView
    {
        // CONFIGURATION
        [SerializeField]
        private float m_DisplayDuration = 3f;
        [SerializeField]
        private AudioClip m_AchievementSound;

        // PRIVATE MEMBERS

        private bool m_Displayed;
        private float m_Duration;

        [GetComponentInChildren("Icon", true), SerializeField, HideInInspector]
        private Image m_Icon;
        [GetComponentInChildren("Name", true), SerializeField, HideInInspector]
        private GUIText m_AchievementName;

        public override void OnOpen()
        {
            base.OnOpen();

            m_Duration = m_DisplayDuration;
            m_Displayed = true;

            Frontend.PlaySound(m_AchievementSound);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (m_Displayed == false)
                return;

            m_Duration -= Time.unscaledDeltaTime;

            if (m_Duration <= 0f)
            {
                m_Displayed = false;
                Close();
            }
        }

        public void SetData(Achievement achievement)
        {
            m_Icon.sprite = achievement.Sprite;
            m_AchievementName.text = TextDatabase.Localize[achievement.TextID];
        }
    }
}
