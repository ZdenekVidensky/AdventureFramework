namespace TVB.Game.Navigation
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public static class AStar
    {
        public static List<Vector2>Search(NavMesh startMesh, NavMesh destinationMesh)
        {
            List<Vector2> result = new List<Vector2>(16);

            List<NavMesh> closedSet = new List<NavMesh>(16);
            List<NavMesh> openSet = new List<NavMesh>(16);
            List<NavMesh> finalPath = new List<NavMesh>(16);

            openSet.Add(startMesh);

            Dictionary<NavMesh, NavMesh> cameFrom = new Dictionary<NavMesh, NavMesh>();

            while(openSet.Any() == true)
            {
                NavMesh current = openSet.Where(m => Function(m, startMesh, destinationMesh) == openSet.Min(n => Function(n, startMesh, destinationMesh))).FirstOrDefault();
                
                if (current == destinationMesh)
                {
                    return ReconstructPath(cameFrom, destinationMesh);
                }

                openSet.Remove(current);
                closedSet.Add(current);

                foreach (var neighbour in current.Neighbours)
                {

                    if (closedSet.Contains(neighbour))
                        continue;

                    float tentativeGScore = GetDistance(startMesh, current) + GetDistance(current, neighbour);

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                    else if (tentativeGScore >= GetDistance(startMesh, neighbour))
                    {
                        continue;
                    }

                    cameFrom[neighbour] = current;
                }
            }

            throw new System.Exception("No optimal way found!");
        }

        public static List<Vector2> ReconstructPath(Dictionary<NavMesh, NavMesh> cameFrom, NavMesh current)
        {
            List<Vector2> result = new List<Vector2>();
            result.Add(current.Position);

            while (cameFrom.Keys.Contains(current))
            {
                current = cameFrom[current];
                result.Add(current.Position);
            }

            result.Reverse();

            return result;
        }

        public static float GetDistance(NavMesh A, NavMesh B)
        {
            return Vector2.Distance(A.Position, B.Position);
        }


        public static float GetHeuristic(NavMesh A, NavMesh destinationMesh)
        {
            return Vector2.Distance(A.Position, destinationMesh.Position);
        }

        public static float Function(NavMesh x, NavMesh startMesh, NavMesh destinationMesh)
        {
            return GetDistance(startMesh, x) + GetHeuristic(x, destinationMesh);
        }
    }
}
