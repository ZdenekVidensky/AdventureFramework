namespace TVB.Game.Graph
{
    using UnityEngine;

    [CreateNodeMenu("Wait for back Node"), NodeTint("#f51419")]
    [NodeWidth(250)]
    class WaitForBackNode : BaseInteractiveNode
    {
        [Input] public BaseInteractiveNode Input;
        [Output] public BaseInteractiveNode Output;
    }
}
