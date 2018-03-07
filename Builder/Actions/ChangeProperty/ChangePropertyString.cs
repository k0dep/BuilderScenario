using UnityEditor;

namespace BuilderScenario
{
    [SelfNamedTagAttrubute]
    public class ChangePropertyString : ChangePropertyBase
    {
        public string StringValue { get; set; } = "";

        [YamlDotNet.Serialization.YamlIgnore]
        public override SerializedPropertyType PropertyType { get { return SerializedPropertyType.String; } }

        public override void ChangeProperty(UnityEngine.Object obj)
        {
            var targetObj = new SerializedObject(obj);
            var prop = targetObj.FindProperty(Name);
            if (prop == null)
                return;

            prop.stringValue = StringValue;
            prop.serializedObject.ApplyModifiedPropertiesWithoutUndo();

        }

        public override void EditorInspector(ChangePropertyPrefabAction parent)
        {
            base.EditorInspector(parent);
            StringValue = EditorGUILayout.TextField("Assign value", StringValue);
        }
    }

}
