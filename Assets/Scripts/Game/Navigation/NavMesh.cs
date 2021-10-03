namespace TVB.Game.Navigation
{
    using System.Collections.Generic;
    using UnityEngine;

    [ExecuteInEditMode]
    public class NavMesh : MonoBehaviour
    {
        // CONFIGURATION

        public List<NavMesh> Neighbours = new List<NavMesh>(8);

        // PRIVATE MEMBERS

        private Transform m_Transform;


        // PUBLIC MEMBERS

        public Vector3 Position => m_Transform.position;


        // MONOBEHAVIOUR INTERFACE

        private void Awake()
        {
            m_Transform = this.transform;
        }

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            Gizmos.DrawIcon(transform.position, "path", true);

            foreach(NavMesh item in Neighbours)
            {
                if (item == null)
                    continue;

                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, item.transform.position);
            }
        }
    }
#endif
}
