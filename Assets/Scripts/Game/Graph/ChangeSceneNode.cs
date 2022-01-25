namespace TVB.Core.Graph
{
    using UnityEngine;

    [CreateNodeMenu("ChangeSceneNode"), NodeTint("#2e7928")]
    [NodeWidth(250)]
    class ChangeSceneNode : BaseInteractiveNode
    {
        [Input] public BaseInteractiveNode Input;
        [Output] public BaseInteractiveNode Output;

        public string SceneName;
    }
}
