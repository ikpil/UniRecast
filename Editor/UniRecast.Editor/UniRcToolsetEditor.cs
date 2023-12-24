namespace UniRecast.Editor
{
    using UniRecast.Toolsets;
    using UnityEditor;
    using UnityEngine;

    [CanEditMultipleObjects]
    [CustomEditor(typeof(UniRcToolset))]
    public class UniRcToolsetEditor : Editor
    {
        private void OnSceneGUI()
        {
            Debug.Log("asdf");
        }
 
    }
}