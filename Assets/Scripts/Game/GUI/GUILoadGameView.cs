namespace TVB.Game.GUI
{
    using TVB.Core.Attributes;
    using TVB.Core.GUI;
    using UnityEngine;

    public class GUILoadGameView : GUIView
    {
        // CONFIGURATION

        [SerializeField]
        private AudioClip m_ButtonSound; // TODO: Move to some settings

        // PRIVATE MEMBERS

        [SerializeField, HideInInspector, GetComponentInChildren("SlotsList", true)]
        private GUISaveSlotsList m_SlotsList;

        public override void OnInitialized()
        {
            base.OnInitialized();

        }

        public override void OnDeinitialized()
        {

            base.OnDeinitialized();
        }

        public override void OnOpen()
        {
            base.OnOpen();

            m_SlotsList.InitializeSaveSlots(true);
        }
    }
}
