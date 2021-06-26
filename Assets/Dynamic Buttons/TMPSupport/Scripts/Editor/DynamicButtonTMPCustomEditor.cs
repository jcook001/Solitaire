using UnityEditor;

namespace DynamicButtons {

    [CustomEditor (typeof (DynamicButtonTMP), true)]
    public class DynamicButtonTMPCustomEditor : DynamicButtonBaseCustomEditor {

        SerializedProperty textField;

        protected override void OnEnable () {
            base.OnEnable ();
            textField = serializedObject.FindProperty ("textField");
        }

        protected override void DrawChildFields () {
            EditorGUILayout.PropertyField (textField);
        }
    }
}