namespace TVB.Core.Graph
{
    using UnityEngine;

    [CreateNodeMenu("Unlock Achievement Node"), NodeTint("#096d0b")]
    [NodeWidth(250)]
    class UnlockAchievementNode : BaseInteractiveNode
    {
        [Input] public BaseInteractiveNode Input;
        [Output] public BaseInteractiveNode Output;

        public string ID;
    }
}
