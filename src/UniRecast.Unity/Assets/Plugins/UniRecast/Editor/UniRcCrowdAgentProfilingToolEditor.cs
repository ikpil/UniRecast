using UniRecast.Runtime.Toolsets;
using UnityEditor;
using UnityEngine;

namespace UniRecast.Editor
{
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