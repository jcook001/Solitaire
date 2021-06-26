using System;

namespace DynamicButtons {

    [Serializable]
    public class ButtonFloatProperty : ButtonProperty<float> {

        public static ButtonFloatProperty defaultFloatProperty {
            get {
                var result = new ButtonFloatProperty {
                    DefaultValue = 0,
                    PressedValue = 0,
                    HighlightedValue = 0,
                    SelectedValue = 0,
                    DisabledValue = 0
                };
                return result;
            }
        }
    }
}