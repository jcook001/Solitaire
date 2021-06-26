using UnityEngine;

namespace DynamicButtons {

    [System.Serializable]
    public class TextProperties {

        [SerializeField]
        private ButtonStringProperty text = ButtonStringProperty.defaultButtonTextProperty;
        [SerializeField]
        private ButtonIntProperty fontSize = ButtonIntProperty.defaultFontSizeProperty;
        [SerializeField]
        private ButtonColorProperty fontColor = ButtonColorProperty.fontColorProperty;

        public ButtonIntProperty FontSize {
            get { return fontSize; }
            set { fontSize = value; }
        }

        public ButtonColorProperty FontColor {
            get { return fontColor; }
            set { fontColor = value; }
        }

        public ButtonStringProperty Text {
            get { return text; }
            set { text = value; }
        }
    }
}