namespace UniRecast.Editor
{
    using UniRecast.Toolsets;
    using UnityEditor;

    [CustomEditor(typeof(UniRcDynamicUpdateTool))]
    public class UniRcDynamicUpdateToolEditor : UniRcToolEditor
    {
        protected override void Layout()
        {
            UniRcGui.Text(this.GetType().Name);
            UniRcGui.Separator();
        }
    }
}