namespace TVB.Core.Graph
{
    using UnityEngine;

    [CreateNodeMenu("Destroy Object Node"), NodeTint("#c54c35")]
    [NodeWidth(250)]
    class DestroyObjectNode : BaseInteractiveNode
    {
        [Input] public BaseInteractiveNode Input;
    }
}
