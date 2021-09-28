using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TVB.Core.GUI;

public static class GUIExtensions
{
    public static void SetActive(this GUIView @this, bool state)
    {
        if (@this.gameObject.activeSelf == state)
            return;

        @this.gameObject.SetActive(state);
    }

    public static void SetActive(this TextMeshProUGUI @this, bool state)
    {
        if (@this.gameObject.activeSelf == state)
            return;

        @this.gameObject.SetActive(state);
    }

    public static void SetActive(this GUIComponent @this, bool state)
    {
        if (@this.gameObject.activeSelf == state)
            return;

        @this.gameObject.SetActive(state);
    }

    public static void SetActive(this Button @this, bool state)
    {
        if (@this.gameObject.activeSelf == state)
            return;

        @this.gameObject.SetActive(state);
    }

    public static void SetActive(this RectTransform @this, bool state)
    {
        if (@this.gameObject.activeSelf == state)
            return;

        @this.gameObject.SetActive(state);
    }
}
