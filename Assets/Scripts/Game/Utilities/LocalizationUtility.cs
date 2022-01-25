using TVB.Core.Interactable;

namespace TVB.Game.Utilities
{
    public static class LocalizationUtility
     {
        public static int GetActionTypeTextID(EInteractableAction actionType, int customTextID)
        {
            switch (actionType)
            {
                case EInteractableAction.Talk:
                    return 100001;
                case EInteractableAction.Use:
                    return 100002;
                case EInteractableAction.Examine:
                    return 100003;
                case EInteractableAction.Take:
                    return 100004;
                case EInteractableAction.Custom:
                    return customTextID;
                default:
                    return 0;
            }
        }
     }
}
