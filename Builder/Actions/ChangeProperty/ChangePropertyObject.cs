using UnityEditor;

namespace BuilderScenario
{
    [SelfNamedTagAttrubute]
    public class ChangePropertyObject : ChangePropertyBase
    {
        public string ObjectGUID { get; set; } = "";

        [YamlDotNet.Serialization.YamlIgnore]
        public override SerializedPropertyType PropertyType { get { return SerializedPropertyType.ObjectReference; } }

        public override void ChangeProperty(UnityEngine.Object obj)
        {
            var targetObj = new SerializedObject(obj);
            var prop = targetObj.FindProperty(Name);
            if (prop == null)
                return;

            prop.objectReferenceValue = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(ObjectGUID), typeof(UnityEngine.Object));
            prop.serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }

        public override void EditorInspector(ChangePropertyPrefabAction parent)
        {
            base.EditorInspector(parent);
            ObjectGUID = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(EditorGUILayout.ObjectField("Assign reference", AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(ObjectGUID), typeof(UnityEngine.Object)), typeof(UnityEngine.Object), false).GetInstanceID()));
        }
    }
}
