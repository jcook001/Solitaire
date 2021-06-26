using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DynamicButtons {

    public partial class PropertiesExecutor {

        public const float TweenDuration = 0.1f;

        private static void PerformIntTween (TweenController tweenController, int valueFrom, int valueTo, UnityAction<int> callback) {
            IntTween tween = new IntTween {
                Duration = TweenDuration,
                ValueFrom = valueFrom,
                ValueTo = valueTo,
                Callback = callback
            };

            tweenController.startTween (tween);
        }

        private static void PerformFloatTween (TweenController tweenController, float valueFrom, float valueTo, UnityAction<float> callback) {
            FloatTween tween = new FloatTween {
                Duration = TweenDuration,
                ValueFrom = valueFrom,
                ValueTo = valueTo,
                Callback = callback
            };

            tweenController.startTween (tween);
        }

        private static void PerformColorTween (TweenController tweenController, Color valueFrom, Color valueTo, UnityAction<Color> callback) {
            ColorTween tween = new ColorTween {
                Duration = TweenDuration,
                ValueFrom = valueFrom,
                ValueTo = valueTo,
                Callback = callback
            };

            tweenController.startTween (tween);
        }

        public static void ApplyTextProperties (TweenController tweenController, bool instant, Text textField, TextProperties properties, DynamicButtonState state) {
            if (textField == null)
                return;

            textField.text = properties.Text.getValue (state);

            if (tweenController != null && !instant) {
                PerformIntTween (tweenController,
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

        public static void ApplyImageProperties (TweenController tweenController, bool instant, Image imageField, ImageProperties properties, DynamicButtonState state) {
            if (imageField == null)
                return;

            imageField.sprite = properties.Sprite.getValue (state);
            imageField.material = properties.Material.getValue (state);

            if (tweenController != null && !instant) {
                PerformColorTween (tweenController,
                    imageField.color,
                    properties.Color.getValue (state),
                    (color => imageField.color = color));
            } else {
                imageField.color = properties.Color.getValue (state);
            }
        }

        public static void ApplyRoundedRectangleProperties (TweenController tweenController, bool instant, RoundedRectangleGraphic roundedRectangleField, RoundedRectangleProperties properties, DynamicButtonState state) {
            if (roundedRectangleField == null)
                return;

            if (tweenController != null && !instant) {
                PerformColorTween (tweenController,
                    roundedRectangleField.color,
                    properties.Color.getValue (state),
                    (color => roundedRectangleField.color = color));

                PerformColorTween (tweenController,
                    roundedRectangleField.BorderColor,
                    properties.BorderColor.getValue (state),
                    (color => roundedRectangleField.BorderColor = color));

                PerformFloatTween (tweenController,
                    roundedRectangleField.BorderWidth,
                    properties.BorderWidth.getValue (state),
                    (width => roundedRectangleField.BorderWidth = width));
            } else {
                roundedRectangleField.BorderColor = properties.BorderColor.getValue (state);
                roundedRectangleField.color = properties.Color.getValue (state);
                roundedRectangleField.BorderWidth = properties.BorderWidth.getValue (state);
            }
        }
    }
}