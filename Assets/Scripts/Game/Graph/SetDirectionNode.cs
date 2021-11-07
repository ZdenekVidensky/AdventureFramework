﻿namespace TVB.Game.Graph
{
    using UnityEngine;

    [CreateNodeMenu("SetDirectionNode"), NodeTint("#2e7928")]
    [NodeWidth(250)]
    class SetDirectionNode : BaseInteractiveNode
    {
        [Input] public BaseInteractiveNode Input;
        [Output] public BaseInteractiveNode Output;

        public EDirection Direction;
    }
}