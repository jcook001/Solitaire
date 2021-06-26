using UnityEngine;

namespace DynamicButtons {

    [System.Serializable]
    public class RoundedRectangleProperties {

        [SerializeField]
        private ButtonColorProperty color = ButtonColorProperty.defaultColorProperty;
        [SerializeField]
        private ButtonColorProperty borderColor = ButtonColorProperty.defaultColorProperty;
        [SerializeField]
        private ButtonFloatProperty borderWidth = ButtonFloatProperty.defaultFloatProperty;

        public ButtonColorProperty Color {
            get { return color; }
            set { color = value; }
        }

        public ButtonColorProperty BorderColor {
            get { return borderColor; }
            set { borderColor = value; }
        }

        public ButtonFloatProperty BorderWidth {
            get { return borderWidth; }
            set { borderWidth = value; }
        }
    }
}