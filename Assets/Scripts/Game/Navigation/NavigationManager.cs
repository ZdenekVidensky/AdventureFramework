namespace TVB.Game.Navigation
{
    using Sirenix.OdinInspector;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    [RequireComponent(typeof(PolygonCollider2D))]
    public class NavigationManager : MonoBehaviour
    {

        // PRIVATE MEMBERS

        private NavMesh[]         m_NavMeshes = new NavMesh[0];
        private PolygonCollider2D m_WalkableArea;
        private Camera            m_MainCamera;

        // MONOBEHAVIOUR INTERFACE

        private void Awake()
        {
            m_WalkableArea = GetComponentInChildren<PolygonCollider2D>(true);
            m_MainCamera   = Camera.main;

            FillNavMeshes();
        }

        private void OnMouseDown()
        {
            if (AdventureGame.Instance.IsBusy == true)
                return;

            Player player = AdventureGame.Instance.Player;

            if (player == null)
            {
                Debug.LogError("Player is not in the scene!");
                return;
            }

            Vector3 destinationPoint = m_MainCamera.ScreenToWorldPoint(Input.mousePosition);

            NavMesh startNavMesh = GetClosesNavMeshToPoint(player.Position);
            NavMesh destinationNavMesh = GetClosesNavMeshToPoint(destinationPoint);

            List<Vector2> path = AStar.Search(startNavMesh, destinationNavMesh);
            path.Add(destinationPoint);

            player.GoTo(path);

            //for (int idx = 0, count = path.Count - 1; idx < count; ++idx)
            //{
            //    Debug.DrawLine(path[idx], path[idx + 1], Color.green, 3f);                
            //}
        }

        private NavMesh GetClosesNavMeshToPoint(Vector2 point)
        {
            float distance = float.MaxValue;
            NavMesh result = null;

            for (int idx = 0; idx < m_NavMeshes.Length; ++idx)
            {
                NavMesh navMesh = m_NavMeshes[idx];
                Vector2 position = navMesh.Position;
                float newDistance = Vector2.Distance(point, position);

                if (newDistance < distance)
                {
                    distance = newDistance;
                    result = navMesh;
                }
            }

            return result;
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        // EDITOR

#if UNITY_EDITOR
        [Button("Find and fill all NavMeshes neighbours")]
        private void FillNavMeshes()
        {
            m_NavMeshes = GetComponentsInChildren<NavMesh>(true);

            foreach(var item in m_NavMeshes)
            {
                item.Neighbours.Clear();

                foreach(var subItem in m_NavMeshes)
                {
                    if (subItem == item)
                        continue;

                    item.Neighbours.Add(subItem);
                }
            }
        }
#endif
    }
}

