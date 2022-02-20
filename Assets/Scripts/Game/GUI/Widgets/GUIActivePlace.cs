namespace TVB.Game.GUI
{
    using UnityEngine;
    using UnityEngine.UI;
    using TVB.Core.GUI;

    using GUIText = TMPro.TextMeshProUGUI;

    public class GUIActivePlace : GUIComponent
    {
        // PUBLIC METHODS

        public void SetData(int textID, Sprite sprite, Vector3 position)
        {
            GUILocalizedText localizedText = GetComponentInChildren<GUILocalizedText>(true);
            Image icon                     = GetComponentInChildren<Image>(true);
            GUIText text                   = GetComponentInChildren<GUIText>(true);

            localizedText.SetTextElement(text);
            localizedText.TextID = textID;
            localizedText.Localize();

            icon.sprite        = sprite;
            icon.raycastTarget = false;

            (transform as RectTransform).position = position;
        }
    }
}
