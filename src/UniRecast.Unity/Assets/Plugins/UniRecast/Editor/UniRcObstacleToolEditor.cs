using UniRecast.Runtime.Toolsets;
using UnityEditor;

namespace UniRecast.Editor
{
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