using UnityEngine;

namespace DynamicButtons {
    
    public class FloatTween : Tween<float> {

        protected override float CalculateValue (float percentage) {
            return Mathf.Lerp (ValueFrom, ValueTo, percentage);
        }
    }
}