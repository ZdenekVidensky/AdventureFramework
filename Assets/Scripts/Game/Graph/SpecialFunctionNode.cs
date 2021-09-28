namespace TVB.Game.Graph
{
    using UnityEngine;

    [CreateNodeMenu("Special Function Node"), NodeTint("#c54c35")]
    [NodeWidth(250)]
    class SpecialFunctionNode : BaseInteractiveNode
    {
        [Input] public BaseInteractiveNode Input;
        [Output] public BaseInteractiveNode Output;

        public string MethodName;
        public float  Delay = 0f;
    }
}
