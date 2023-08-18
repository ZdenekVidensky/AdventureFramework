namespace TVB.Core.Graph
{
    using Sirenix.OdinInspector;
    using TVB.Core.Interactable;
    using TVB.Core.Localization;
    using UnityEngine;

    [CreateNodeMenu("Talk Node"), NodeTint("#4564e9"), NodeWidth(300)]
    class TalkNode : BaseInteractiveNode
    {
        [Input] public BaseInteractiveNode Input;
        [Output] public BaseInteractiveNode Output;

        public ETalkableCharacter Character;
        public int TextID;
        public bool PlayTalkAnimation = true;

        [MultiLineProperty(5), DisableInEditorMode, DisableInPlayMode, ShowInInspector]
        public string Text => TextDatabase.Localize[TextID];
    }
}
