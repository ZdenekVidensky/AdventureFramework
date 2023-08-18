namespace TVB.Core.Graph
{
    using Sirenix.OdinInspector;
    using TVB.Game;
    using UnityEngine;

    [CreateNodeMenu("Drop Item Node"), NodeTint("#000000")]
    [NodeWidth(250)]
    class DropItemNode : BaseInteractiveNode
    {
        [Input] public BaseInteractiveNode Input;
        [Output] public BaseInteractiveNode Output;

        [PreviewField(75)]
        public InventoryItem Item;
        [LabelWidth(120)]
        public bool PlayAnimation;
    }
}
