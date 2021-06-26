using UnityEngine;

namespace DynamicButtons {
    
    public class ColorTween : Tween<Color> {

        protected override Color CalculateValue (float percentage) {
            return Color.Lerp (ValueFrom, ValueTo, percentage);
        }
    }
}