﻿namespace TVB.Core.Graph
{
    //using Sirenix.OdinInspector;
    using TVB.Game;
    using UnityEngine;

    [CreateNodeMenu("Take Item Node"), NodeTint("#2e7928")]
    [NodeWidth(250)]
    class TakeItemNode : BaseInteractiveNode
    {
        [Input] public BaseInteractiveNode Input;
        [Output] public BaseInteractiveNode Output;

        //[PreviewField(75)]
        public InventoryItem Item;
        //[LabelWidth(120)]
        public bool DestroyAfterTake = true;
    }
}
