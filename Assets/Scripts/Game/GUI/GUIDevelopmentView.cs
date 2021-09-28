namespace TVB.Game.GUI
{
    using UnityEngine;
    
    using TVB.Core.Attributes;
    using TVB.Core.GUI;

    using GUIText = TMPro.TextMeshProUGUI;

    public class GUIDevelopmentView : GUIView
    {
        // CONSTANTS

        private const string COPYRIGHT_FORMAT = "Copyright {0} TVB Software";
        private const string FPS_FORMAT       = "FPS {0}";

        // PRIVATE MEMBERS

        [GetComponentInChildren("Version", true), SerializeField, HideInInspector]
        private GUIText m_Version;
        [GetComponentInChildren("Copyright", true), SerializeField, HideInInspector]
        private GUIText m_Copyright;
        [GetComponentInChildren("FPS", true), SerializeField, HideInInspector]
        private GUIText m_FPS;

        // PUBLIC METHODS

        public void DisplayFPS(bool state)
        {
            m_FPS.SetActive(state);
        }

        // GUIVIEW OVERRIDES

        public override void OnOpen()
        {
            base.OnOpen();

            m_Version.text = Application.version;
            m_Copyright.text = string.Format(COPYRIGHT_FORMAT, System.DateTime.UtcNow.Year);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            m_FPS.text = string.Format(FPS_FORMAT, (int)(1f / Time.deltaTime));
        }

    }
}
