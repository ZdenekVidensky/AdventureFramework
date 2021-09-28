namespace TVB.Game.Graph
{
    using UnityEngine;

    [CreateNodeMenu("Wait for seconds Node"), NodeTint("#c54c35")]
    [NodeWidth(250)]
    class WaitForSecondsNode : BaseInteractiveNode
    {
        [Input] public BaseInteractiveNode Input;
        [Output] public BaseInteractiveNode Output;

        public float Seconds;
    }
}
