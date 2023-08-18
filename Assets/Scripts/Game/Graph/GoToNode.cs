namespace TVB.Core.Graph
{
    using Sirenix.OdinInspector;
    using TVB.Game;
    using UnityEngine;

    [CreateNodeMenu("GoToNode"), NodeTint("#2e7928")]
    [NodeWidth(250)]
    class GoToNode : BaseInteractiveNode
    {
        [Input] public BaseInteractiveNode Input;
        [Output] public BaseInteractiveNode Output;

        public Vector2 Destination;

        [Button("Set Player position")]
        private void SetPlayerPosition()
        {
            Player player = FindObjectOfType<Player>(true);
            if (player == null)
                return;

            Destination = player.transform.position;
        }
    }
}
