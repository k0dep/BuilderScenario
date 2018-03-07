using UnityEditor;

namespace BuilderScenario
{
    [SelfNamedTagAttrubute]
    public class ChangePropertyInt : ChangePropertyBase
    {
        public int IntValue { get; set; } = 0;

        [YamlDotNet.Serialization.YamlIgnore]
        public override SerializedPropertyType PropertyType { get { return SerializedPropertyType.Integer; } }

        public override void ChangeProperty(UnityEngine.Object obj)
        {
            var targetObj = new SerializedObject(obj);
            var prop = targetObj.FindProperty(Name);
            if (prop == null)
                return;

            prop.intValue = IntValue;
            prop.serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }

        public override void EditorInspector(ChangePropertyPrefabAction parent)
        {
            base.EditorInspector(parent);
            IntValue = EditorGUILayout.IntField("Assign value", IntValue);
        }
    }

}
