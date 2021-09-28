namespace TVB.Game.Graph
{
    using UnityEngine;
    using Sirenix.OdinInspector;
    using TVB.Core.Localization;

    [CreateNodeMenu("Dialogue Node"), NodeTint("#4564e9"), NodeWidth(300)]
    class DialogueLineNode : BaseInteractiveNode
    {
        [Input] public BaseInteractiveNode  Input;
        [Output] public BaseInteractiveNode Output;

        public ECharacter  Character;
        public int         TextID;
        public bool        PlayTalkAnimation = true;

        [MultiLineProperty(5), DisableInEditorMode, DisableInPlayMode, ShowInInspector,]
        public string      Text => TextDatabase.Localize[TextID];
    }

    public enum ECharacter
    {
        Player,
        This
    }
}
