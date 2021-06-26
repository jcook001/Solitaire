using System;

namespace DynamicButtons {

    [Serializable]
    public class ButtonStringProperty : ButtonProperty<String> {

        public static ButtonStringProperty defaultButtonTextProperty {
            get {
                var result = new ButtonStringProperty {
                    DefaultValue = "Button",
                    PressedValue = "",
                    HighlightedValue = "",
                    SelectedValue = "",
                    DisabledValue = ""
                };
                return result;
            }
        }
    }
}