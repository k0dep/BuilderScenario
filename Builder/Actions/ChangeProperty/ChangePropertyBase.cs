using System.Collections.Generic;
using UnityEditor;

namespace BuilderScenario
{
    public interface IChangeProperty
    {
        void ChangeProperty(UnityEngine.Object obj);
        void EditorInspector(ChangePropertyPrefabAction parent);
    }


    public abstract class ChangePropertyBase : IChangeProperty
    {
        public string Name { get; set; } = "";
        protected List<string> Properties;
        protected int Selected = 0;

        public virtual SerializedPropertyType PropertyType { get; }

        public abstract void ChangeProperty(UnityEngine.Object obj);

        public virtual void EditorInspector(ChangePropertyPrefabAction parent)
        {
            if (Properties == null)
            {
                Properties = new List<string>();
                var so = new SerializedObject(parent.targetObj);
                var prop = so.GetIterator();
                var canNext = true;
                while (prop != null && canNext)
                {
                    if (prop.propertyType == PropertyType)
                        Properties.Add(prop.name);
                    canNext = prop.Next(true);
                }
                for (int i = 0; i < Properties.Count; i++)
                {
                    if (Properties[i] == Name)
                        Selected = i;
                }
            }

            Selected = EditorGUILayout.Popup("Property name", Selected, Properties.ToArray());
            Name = Properties[Selected];
        }
    }

}
