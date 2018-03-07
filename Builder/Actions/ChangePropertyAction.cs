using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace BuilderScenario
{
    [SelfNamedTagAttrubute]
    public class ChangePropertyPrefabAction : IAction
    {
        public int ScriptId { get; set; }
        public string TargetGUID { get; set; }
        public List<IChangeProperty> OverridedProperty { get; set; }
        public bool CanAction { get; set; }

        public Component targetObj = null;
        private ReorderableList properties = null;
        private bool isInit = false;


        public ChangePropertyPrefabAction()
        {
            TargetGUID = "";
            OverridedProperty = new List<IChangeProperty>();
        }

        public void Do(BuildScenario conf, int target, BuildLog log)
        {
            var obj = (GameObject)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(TargetGUID), typeof(UnityEngine.GameObject));
            var components = obj.GetComponents<Component>();
            if (components.Length > ScriptId && ScriptId > 0)
            {
                foreach (var changer in OverridedProperty)
                {
                    log.Line("changing property in prefab " + AssetDatabase.GUIDToAssetPath(TargetGUID), "change prefab property", "info");
                    changer.ChangeProperty(components[ScriptId]);
                }
            }
        }

        public void EditorInspector(BuildScenario conf, int target)
        {
            if (properties == null)
            {
                properties = new ReorderableList(OverridedProperty, typeof(IChangeProperty), true, true, true, true);
                properties.drawHeaderCallback = (Rect r) => EditorGUI.LabelField(r, "Overrided properties", EditorStyles.boldLabel);
                properties.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    EditorGUI.LabelField(rect, OverridedProperty[index].GetType().Name);
                };
                properties.onAddDropdownCallback = (Rect r, ReorderableList l) => BuilderWindow.AddDropdownCallback<IChangeProperty>(r, l);
            }
            if(!isInit)
            {
                var obj = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(TargetGUID));
                if (obj != null)
                {
                    var comps = obj.GetComponents<Component>();
                    if (comps != null)
                        targetObj = comps[ScriptId];
                }
                isInit = true;
            }
            var _targetObj = (Component)EditorGUILayout.ObjectField("Target component", targetObj, typeof(Component), false);
            if (_targetObj != targetObj)
            {
                targetObj = _targetObj;
                if (targetObj == null)
                {
                    TargetGUID = "";
                    ScriptId = -1;
                }
                else
                {
                    TargetGUID = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(targetObj.gameObject.GetInstanceID()));
                    short i = 0;
                    foreach (var item in targetObj.gameObject.GetComponents<Component>())
                    {
                        if (item == targetObj)
                        {
                            ScriptId = i;
                            break;
                        }
                        i++;
                    }
                }
                OverridedProperty.Clear();
            }

            properties.DoLayoutList();

            if (properties.index >= 0 && OverridedProperty.Count > properties.index)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                OverridedProperty[properties.index].EditorInspector(this);
                EditorGUILayout.EndVertical();
            }
        }

        public void EditorListInspector(BuildScenario conf, int target, Rect rect)
        {
            EditorGUI.LabelField(rect, "Prefab change property");
            CanAction = EditorGUI.Toggle(new Rect(rect.position + new Vector2(rect.size.x - 20.0f, 0.0f), new Vector2(20.0f, rect.size.y)), CanAction);
        }
    }
}
