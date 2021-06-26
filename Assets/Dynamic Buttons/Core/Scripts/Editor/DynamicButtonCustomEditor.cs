using UnityEditor;

namespace DynamicButtons {

    [CustomEditor (typeof (DynamicButton), true)]
    public class DynamicButtonCustomEditor : DynamicButtonBaseCustomEditor {

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