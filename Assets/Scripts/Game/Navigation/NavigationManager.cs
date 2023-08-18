namespace TVB.Game.Navigation
{
    //using Sirenix.OdinInspector;
    using System.Collections.Generic;
    using UnityEngine;

    [RequireComponent(typeof(PolygonCollider2D))]
    public class NavigationManager : MonoBehaviour
    {

        private const string BLOCK_LAYER_NAME = "Block";
        private const float DOUBLECLICK_DELAY = 0.3f;

        // PRIVATE MEMBERS

        private NavMesh[] m_NavMeshes = new NavMesh[0];
        private Camera m_MainCamera;
        private float m_LastClickTime;

        // MONOBEHAVIOUR INTERFACE

        private void Awake()
        {
            m_MainCamera = Camera.main;

            FillNavMeshes();
        }


        private void OnMouseUp()
        {
            if (AdventureGame.Instance.IsBusy == true)
                return;

            if (AdventureGame.Instance.IsInventoryOpen == true)
                return;

            if (AdventureGame.Instance.AreActivePlacesVisible == true)
                return;

            if (AdventureGame.Instance.IsGamePaused == true)
                return;

            AdventureGame.Instance.GraphManager.ForceStopProcessing();

            Player player = AdventureGame.Instance.Player;

            if (player == null)
            {
                Debug.LogError("Player is not in the scene!");
                return;
            }

            Vector3 destinationPoint = m_MainCamera.ScreenToWorldPoint(Input.mousePosition);

            if (Time.timeSinceLevelLoad - m_LastClickTime < DOUBLECLICK_DELAY)
            {
                player.SkipTo(destinationPoint);
                return;
            }

            m_LastClickTime = Time.timeSinceLevelLoad;

            GoTo(destinationPoint, player);
        }

        public void GoTo(Vector3 destinationPoint, Player player)
        {
            if (IsPathBlockingSomething(player.Position, destinationPoint) == true)
            {
                NavMesh startNavMesh = GetClosesNavMeshToPoint(player.Position);
                NavMesh destinationNavMesh = GetClosesNavMeshToPoint(destinationPoint);

                List<Vector2> path = AStar.Search(startNavMesh, destinationNavMesh);
                path.Add(destinationPoint);

                player.GoTo(path);
            }
            else
            {
                player.GoTo(destinationPoint);
            }
        }

        private static bool IsPathBlockingSomething(Vector3 playerPosition, Vector3 destination)
        {
            Vector3 direction = destination - playerPosition;
            float distance = Vector3.Distance(playerPosition, destination);
            int layerMask = LayerMask.GetMask(BLOCK_LAYER_NAME);

            RaycastHit2D hit = Physics2D.Raycast(playerPosition, direction, distance, layerMask);
            Debug.DrawRay(playerPosition, direction, Color.green, 3f);

            return hit.collider != null;
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

        // EDITOR

        //[Button("Find and fill all NavMeshes neighbours")]
        private void FillNavMeshes()
        {
            m_NavMeshes = GetComponentsInChildren<NavMesh>(true);

            foreach (var item in m_NavMeshes)
            {
                item.Neighbours.Clear();

                foreach (var subItem in m_NavMeshes)
                {
                    if (subItem == item)
                        continue;

                    item.Neighbours.Add(subItem);
                }
            }
        }
    }
}

