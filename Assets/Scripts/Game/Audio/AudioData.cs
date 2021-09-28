namespace TVB.Game
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "Audio Data", menuName = "Items/Audio Data")]

    public class AudioData : ScriptableObject
    {
        [Header("Menu")]
        public AudioClip MenuBackgroundMusic;

        [Header("Demo")]
        public AudioClip DemoBackgroundMusic;
        public AudioClip DemoAmbientBackground;
    }
}
