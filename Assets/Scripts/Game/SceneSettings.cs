namespace TVB.Game
{
    //using Sirenix.OdinInspector;
    using TVB.Core.Localization;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Data/SceneSettings")]
    public class SceneSettings : ScriptableObject
    {
        public string SceneName;
        public int SceneNameID;

        //[MultiLineProperty(5), DisableInEditorMode, DisableInPlayMode, ShowInInspector]
        public string DisplayName => TextDatabase.Localize[SceneNameID];

        [Header("Scales")]
        public ScaleLevel TopScale;
        public ScaleLevel BottomScale;

        [Header("Previous scene")]
        public PreviousSceneLocation[] PreviousSceneLocations;

        [Header("Music")]
        public AudioClip BackgroundMusic;
        public AudioClip AmbientMusic;
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
        public EDirection Direction;
    }
}
