using TMPro;
using UnityEngine;

namespace DynamicButtons {

    [ExecuteInEditMode]
    public class DynamicButtonTMP : DynamicButtonBase {

        public TextMeshProUGUI textField;

        protected override void TransitionButtonProperties (DynamicButtonState state, bool instant) {
            base.TransitionButtonProperties (state, instant);

            if (ButtonTransitionType == DynamicButtonTransitionType.PROPERTY) {
                PropertiesExecutor.ApplyTMPTextProperties (tweenController, instant, textField, TextProperties, state);
            }
        }
    }
}