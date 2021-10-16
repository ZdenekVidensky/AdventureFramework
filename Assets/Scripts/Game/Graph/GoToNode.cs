namespace TVB.Game.Graph
{
    using UnityEngine;
    using Sirenix.OdinInspector;

    [CreateNodeMenu("GoToNode"), NodeTint("#2e7928")]
    [NodeWidth(250)]
    class GoToNode : BaseInteractiveNode
    {
        [Input] public BaseInteractiveNode Input;
        [Output] public BaseInteractiveNode Output;

        public Vector2 Destination;
    }
}
