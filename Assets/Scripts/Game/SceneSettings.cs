namespace TVB.Game
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "Data/SceneSettings")]
    public class SceneSettings : ScriptableObject
    {
        [Header("Scales")]
        public ScaleLevel TopScale;
        public ScaleLevel BottomScale;
    }

    [System.Serializable]
    public struct ScaleLevel
    {
        public float YPosition;
        public float Scale;
    }

    // TODO: Music, Ambient
    // TODO: Location based on previous scenes
}
