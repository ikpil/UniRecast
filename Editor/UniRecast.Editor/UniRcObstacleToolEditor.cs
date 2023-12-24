namespace UniRecast.Editor
{
    using UniRecast.Toolsets;
    using UnityEditor;

    [CustomEditor(typeof(UniRcObstacleTool))]
    public class UniRcObstacleToolEditor : UniRcToolEditor
    {
        protected override void Layout()
        {
            UniRcGui.Text(this.GetType().Name);
            UniRcGui.Separator();
        }
    }
}