namespace TVB.Core.Graph
{
    using UnityEngine;
    using XNode;

    [CreateAssetMenu]
    public class InteractiveGraph : NodeGraph
    {
        public Node GetFirstNode()
        {
            for (int idx = 0, count = nodes.Count; idx < count; idx++)
            {
                Node node = nodes[idx];

                NodePort input = node.GetPort("Input")?.Connection;

                if (input == null)
                    return node;
            }

            return null;
        }


    }
}
