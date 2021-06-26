using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace DynamicButtons {
    public partial class MenuItems {

        [MenuItem ("GameObject/UI/Dynamic Button - TMP", false, 10001)]
        private static void AddDynamicButtonTMPOption () {
            GameObject gameObject = new GameObject ("Dynamic Button - TMP");

            var rect = gameObject.AddComponent<RectTransform> ();
            rect.sizeDelta = new Vector2 (160f, 30f);
            if (Selection.transforms.Length > 0) {
                rect.SetParent (Selection.transforms[0], false);
            }

            GameObject textGO = new GameObject ("Text");
            RectTransform textRect = textGO.AddComponent<RectTransform> ();
            TextMeshProUGUI text = textGO.AddComponent<TextMeshProUGUI> ();

            text.alignment = TextAlignmentOptions.Center;

            textRect.SetParent (gameObject.transform);
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.sizeDelta = Vector2.zero;
            textRect.localPosition = Vector2.zero;
            textRect.localScale = Vector3.one;

            DynamicButtonTMP dynamicButton = gameObject.AddComponent<DynamicButtonTMP> ();
            dynamicButton.textField = text;
        }
    }
}