using UnityEngine;

using TVB.Game.Graph;
using TVB.Game.Interactable;
using TVB.Game;

public class LevelLoader : MonoBehaviour, IInteractable
{
    [SerializeField]
    private string m_SceneName;
    EInteractableAction IInteractable.ActionType => EInteractableAction.None;

    string IInteractable.Name => gameObject.name;

    InteractiveGraph IInteractable.InteractiveGraph => null;

    private void OnMouseDown()
    {
        if (AdventureGame.Instance.IsBusy == true)
            return;

        if (AdventureGame.Instance.IsGamePaused == true)
            return;

        (this as IInteractable).OnInteract();
    }

    int IInteractable.CustomTextID => -1;

    void IInteractable.Destroy()
    {
    }

    void IInteractable.OnInteract()
    {
        AdventureGame.Instance.LoadSceneAsync(m_SceneName);
    }

    void IInteractable.OnUseItem(string itemID)
    {
    }
}
