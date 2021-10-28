namespace TVB.Game.Interactable
{   
    public interface ITalkable
    {
        void SetIsTalking(bool state);

        ETalkableCharacter Character { get; }
    }

    public enum ETalkableCharacter
    {
        Player, // Do not assign to any other NPC!
        GhostWoman
    }
}
