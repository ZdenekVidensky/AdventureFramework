namespace TVB.Game.GUI
{
    using UnityEngine;

    using TVB.Core.Attributes;
    using TVB.Core.GUI;

    public class GUISaveGameView : GUIView
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

            m_SlotsList.InitializeSaveSlots(false);
        }
    }
}
