namespace TVB.Game.Navigation
{
    using Sirenix.OdinInspector;
    using UnityEngine;

    public class NavigationManager : MonoBehaviour
    {

        // PRIVATE MEMBERS

        private NavMesh[] m_NavMeshes = new NavMesh[0];

        // MONOBEHAVIOUR INTERFACE

        private void Awake()
        {
            FillNavMeshes();
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

