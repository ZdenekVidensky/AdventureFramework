namespace TVB.Game
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "Data/SceneSettings")]
    public class SceneSettings : ScriptableObject
    {
        [Header("Scales")]
        public ScaleLevel TopScale;
        public ScaleLevel BottomScale;

        [Header("Previous scene")]
        public PreviousSceneLocation[] PreviousSceneLocations;
    }

    [System.Serializable]
    public struct ScaleLevel
    {
        public float YPosition;
        public float Scale;
    }

    [System.Serializable]
    public struct PreviousSceneLocation
    {
        public string SceneName;
        public Vector2 Position;
        // TODO: Direction
    }

    // TODO: Music, Ambient
}
