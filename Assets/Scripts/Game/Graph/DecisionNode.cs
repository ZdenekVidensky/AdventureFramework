namespace TVB.Core.Graph
{
    using UnityEngine;

    [CreateNodeMenu("Decision Node"), NodeTint("#49eb45")]
    class DecisionNode : BaseInteractiveNode
    {
        [Input] public BaseInteractiveNode Input;

        [Output(dynamicPortList = true)] public Decision[] Decisions;
    }

    [System.Serializable]
    public struct Decision
    {
        public int TextID;
        // Write HasItem(id) to set condition to hasItem
        // If condition starts with ! it will process condition as negative
        public string Condition;
    }
}

