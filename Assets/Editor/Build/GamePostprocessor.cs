namespace TVB.Game
{
    using UnityEngine;
    using UnityEditor.Build;
    using UnityEditor.Build.Reporting;

    using TVB.Core.Graph;

    class GamePostprocessor : IProcessSceneWithReport
    {
        int IOrderedCallback.callbackOrder { get { return 0; } }

        //bool m_FirstSceneProcessed = false;

        void IProcessSceneWithReport.OnProcessScene(UnityEngine.SceneManagement.Scene scene, BuildReport report)
        {
            //if (m_FirstSceneProcessed == false)
            //{
            //    GameObject gameObj = new GameObject("Game");
            //    gameObj.AddComponent<AdventureGame>();
            //    gameObj.AddComponent<GraphManager>();
            //    m_FirstSceneProcessed = true;
            //}
        }
    }
}