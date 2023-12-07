namespace UniRecast.Editor
{
    using UniRecast.Toolsets;
    using UnityEditor;

    [CustomEditor(typeof(UniRcJumpLinkBuilderTool))]
    public class UniRcJumpLinkBuilderToolEditor : UniRcToolEditor
    {
        protected override void Layout()
        {
            UniRcGui.Text(this.GetType().Name);
            UniRcGui.Separator();
        }
 
    }
}