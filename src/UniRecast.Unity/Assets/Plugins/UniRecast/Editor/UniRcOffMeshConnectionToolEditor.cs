using UniRecast.Runtime.Toolsets;
using UnityEditor;

namespace UniRecast.Editor
{
    [CustomEditor(typeof(UniRcOffMeshConnectionTool))]
    public class UniRcOffMeshConnectionToolEditor : UniRcToolEditor
    {
        protected override void Layout()
        {
            UniRcGui.Text(this.GetType().Name);
            UniRcGui.Separator();
        }

    }
}