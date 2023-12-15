namespace UniRecast.Core
{
    using UnityEditor;

    public static class UniRcSerializedObjectExtensions
    {
        public static SerializedProperty FindPropertySafe(this SerializedObject serializedObject, string name)
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