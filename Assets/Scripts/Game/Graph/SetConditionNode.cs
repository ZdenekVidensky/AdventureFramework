namespace TVB.Core.Graph
{
    using UnityEngine;
    using Sirenix.OdinInspector;


    [CreateNodeMenu("Set Condition Node"), NodeTint("#24f2c9")]
    [NodeWidth(250)]
    class SetConditionNode : BaseInteractiveNode
    {
        [Input] public BaseInteractiveNode Input;
        [Output] public BaseInteractiveNode Output;

        [LabelWidth(100)]
        [HideIf("ConditionType", EConditionType.HasItem)]
        public string ConditionName;

        [ShowIf("ConditionType", EConditionType.Boolean)]
        [LabelWidth(100)]
        public bool BooleanValue;
    }
}
