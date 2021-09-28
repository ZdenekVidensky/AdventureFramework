namespace TVB.Game
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "Achivement", menuName = "Items/Achievement")]

    public class Achievement : ScriptableObject
    {
        // STATIC NAMES

        public const string SWORD_ACHIEVEMENT = "sword_achievement";

        public string ID;
        public int    TextID;
        public Sprite Sprite;
    }
}
