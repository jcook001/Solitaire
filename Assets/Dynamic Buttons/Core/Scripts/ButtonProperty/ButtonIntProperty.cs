using System;

namespace DynamicButtons {

    [Serializable]
    public class ButtonIntProperty : ButtonProperty<int> {

        public static ButtonIntProperty defaultFontSizeProperty {
            get {
                var result = new ButtonIntProperty {
                    DefaultValue = 14,
                    PressedValue = 14,
                    HighlightedValue = 14,
                    SelectedValue = 14,
                    DisabledValue = 14
                };
                return result;
            }
        }
    }
}