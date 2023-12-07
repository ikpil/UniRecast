namespace UniRecast.Editor
{
    using UnityEditor;
    using UniRecast.Toolsets;

    [CustomEditor(typeof(UniRcCrowdAgentProfilingTool))]
    public class UniRcCrowdAgentProfilingToolEditor : UniRcToolEditor
    {
        protected override void Layout()
        {
            UniRcGui.Text(this.GetType().Name);
            UniRcGui.Separator();
        }
    }
}