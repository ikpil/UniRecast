using UnityEditor;

namespace Plugins.UniRecast.Extensions
{
    public static class UniRcSerializedObjectExtensions
    {
        public static SerializedProperty SafeFindProperty(this SerializedObject serializedObject, string name)
        {
            string safeName = name;
            if (name[0] == '_')
            {
                safeName = name.Substring(1);
            }
            
            return serializedObject.FindProperty(safeName);
        }
    }
}