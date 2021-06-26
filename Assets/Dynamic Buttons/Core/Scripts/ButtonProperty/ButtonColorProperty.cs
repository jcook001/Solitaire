using System;
using UnityEngine;

namespace DynamicButtons {

    [Serializable]
    public class ButtonColorProperty : ButtonProperty<Color> {

        public static ButtonColorProperty defaultColorProperty {
            get {
                var result = new ButtonColorProperty {
                    DefaultValue = new Color (1, 1, 1, 1),
                    HighlightedValue = new Color (1, 1, 1, 1),
                    SelectedValue = new Color (1, 1, 1, 1),
                    PressedValue = new Color (1, 1, 1, 1),
                    DisabledValue = new Color (1, 1, 1, 1)
                };
                return result;
            }
        }

        public static ButtonColorProperty fontColorProperty {
            get {
                var result = new ButtonColorProperty {
                    DefaultValue = new Color (0, 0, 0, 1),
                    HighlightedValue = new Color (0, 0, 0, 1),
                    SelectedValue = new Color (0, 0, 0, 1),
                    PressedValue = new Color (0, 0, 0, 1),
                    DisabledValue = new Color (0, 0, 0, 1)
                };
                return result;
            }
        }
    }
}