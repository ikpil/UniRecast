using UniRecast.Runtime.Toolsets;
using UnityEditor;
using UnityEngine;

namespace UniRecast.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(UniRcToolset))]
    public class UniRcToolsetEditor : UnityEditor.Editor
    {
        private void OnSceneGUI()
        {
            Debug.Log("asdf");
        }
 
    }
}