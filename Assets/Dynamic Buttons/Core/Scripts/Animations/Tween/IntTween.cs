using UnityEngine;

namespace DynamicButtons {

    public class IntTween : Tween<int> {

        protected override int CalculateValue (float percentage) {
            return (int) Mathf.Lerp (ValueFrom, ValueTo, percentage);
        }
    }
}