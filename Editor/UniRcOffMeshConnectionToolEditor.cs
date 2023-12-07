namespace UniRecast.Editor
{
    using UniRecast.Extensions;
    using UniRecast.Toolsets;
    using UnityEditor;

    [CustomEditor(typeof(UniRcOffMeshConnectionTool))]
    public class UniRcOffMeshConnectionToolEditor : UniRcToolEditor
    {
        private SerializedProperty _bidir;

        private void OnEnable()
        {
            _bidir = serializedObject.FindPropertySafe(nameof(_bidir));
        }

        protected override void Layout()
        {
            UniRcGui.RadioButton("One Way", _bidir, 0);
            UniRcGui.RadioButton("Bidirectional", _bidir, 1);

            serializedObject.ApplyModifiedProperties();
        }
    }
}