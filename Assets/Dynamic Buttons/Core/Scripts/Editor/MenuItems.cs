using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace DynamicButtons {
    public partial class MenuItems {

        [MenuItem ("GameObject/UI/Dynamic Button", false, 10000)]
        private static void AddDynamicButtonOption () {
            GameObject gameObject = new GameObject ("Dynamic Button");

            var rect = gameObject.AddComponent<RectTransform> ();
            rect.sizeDelta = new Vector2 (160f, 30f);
            if (Selection.transforms.Length > 0) {
                rect.SetParent (Selection.transforms[0], false);
            }

            GameObject textGO = new GameObject ("Text");
            RectTransform textRect = textGO.AddComponent<RectTransform> ();
            Text text = textGO.AddComponent<Text> ();

            text.alignment = TextAnchor.MiddleCenter;

            textRect.SetParent (gameObject.transform);
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.sizeDelta = Vector2.zero;
            textRect.localPosition = Vector2.zero;
            textRect.localScale = Vector3.one;

            DynamicButton dynamicButton = gameObject.AddComponent<DynamicButton> ();
            dynamicButton.textField = text;
        }
    }
}