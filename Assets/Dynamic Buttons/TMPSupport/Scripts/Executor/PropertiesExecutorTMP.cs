using TMPro;

namespace DynamicButtons {

    public partial class PropertiesExecutor {
        public static void ApplyTMPTextProperties (TweenController tweenController, bool instant, TextMeshProUGUI textField, TextProperties properties, DynamicButtonState state) {
            if (textField == null)
                return;

            textField.text = properties.Text.getValue (state);

            if (tweenController != null && !instant) {
                PerformFloatTween (tweenController,
                    textField.fontSize,
                    properties.FontSize.getValue (state),
                    (size => textField.fontSize = size));

                PerformColorTween (tweenController,
                    textField.color,
                    properties.FontColor.getValue (state),
                    (color => textField.color = color));
            } else {
                textField.fontSize = properties.FontSize.getValue (state);
                textField.color = properties.FontColor.getValue (state);
            }
        }
    }
}