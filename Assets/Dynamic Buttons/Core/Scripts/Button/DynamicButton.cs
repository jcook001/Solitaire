using UnityEngine;
using UnityEngine.UI;

namespace DynamicButtons {

    [ExecuteInEditMode]
    public class DynamicButton : DynamicButtonBase {

        public Text textField;

        protected override void TransitionButtonProperties (DynamicButtonState state, bool instant) {
            base.TransitionButtonProperties (state, instant);

            if (ButtonTransitionType == DynamicButtonTransitionType.PROPERTY) {
                PropertiesExecutor.ApplyTextProperties (tweenController, instant, textField, TextProperties, state);
            }
        }
    }
}