using Sirenix.OdinInspector;
using TVB.Core.GUI;
using UnityEngine;

namespace TVB.Game.GUI
{
    public class GUIGridLayout : MonoBehaviour
    {
        [SerializeField]
        private Padding m_Padding;
        [SerializeField]
        private float CellSizeX;
        [SerializeField]
        private float CellSizeY;
        [SerializeField]
        private float SpacingX;
        [SerializeField]
        private float SpacingY;

        [SerializeField]
        private int ItemsPerRow;

#if UNITY_EDITOR
        [Button("Rearrange")]
#endif
        public void Rearrange()
        {
            GUIComponent[] components = GetComponentsInChildren<GUIComponent>(true);

            int line = 1;
            int xCounter = 0;

            for (int idx = 0; idx < components.Length; idx++, xCounter++)
            {
                GUIComponent item = components[idx];
#if UNITY_EDITOR
                RectTransform rectTransform = item.GetComponent<RectTransform>();
#else
                RectTransform rectTransform = item.RectTransform;
#endif

                if (idx == ItemsPerRow * line)
                {
                    line++;
                    xCounter = 0;
                }

                float positionX = (m_Padding.Left * ((xCounter + 1) % (ItemsPerRow + 1))) + CellSizeX * rectTransform.pivot.x + (SpacingX * xCounter);
                float positionY = -((m_Padding.Top * line) + CellSizeY * rectTransform.pivot.y + (SpacingY * (line - 1)));

                rectTransform.anchoredPosition = new Vector2(positionX, positionY);
            }
        }
    }

    [System.Serializable]
    public struct Padding
    {
        public float Left;
        public float Right;
        public float Top;
        public float Bottom;
    }
}
