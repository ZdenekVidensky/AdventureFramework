namespace TVB.Core.Graph
{
    using Sirenix.OdinInspector;
    using TVB.Game;
    using UnityEngine;

    [CreateNodeMenu("Condition Node"), NodeTint("#e2e04d")]
    [NodeWidth(250)]
    class ConditionNode : BaseInteractiveNode
    {
        [Input] public BaseInteractiveNode Input;
        [Output] public BaseInteractiveNode True;
        [Output] public BaseInteractiveNode False;

        [LabelWidth(100)]
        [HideIf("ConditionType", EConditionType.HasItem)]
        public string ConditionName;

        [LabelWidth(100)]
        public EConditionType ConditionType;

        [ShowIf("ConditionType", EConditionType.Boolean)]
        [LabelWidth(100)]
        [ShowIf("ConditionType", EConditionType.HasItem)]
        [PreviewField(75)]
        public InventoryItem Item;
    }

    public enum EConditionType
    {
        Boolean,
        HasItem
    }
}
