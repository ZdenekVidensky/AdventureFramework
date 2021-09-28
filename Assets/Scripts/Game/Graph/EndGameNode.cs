namespace TVB.Game.Graph
{
    using UnityEngine;

    [CreateNodeMenu("End Game Node"), NodeTint("#c54c35")]
    [NodeWidth(250)]
    class EndGameNode : BaseInteractiveNode
    {
        [Input] public BaseInteractiveNode Input;
    }
}
