using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace BuilderScenario
{
    public class BuilderTarget
    {
        public bool IsBuilding { get; set; }
        public string TargetName { get; set; }

        public List<IAction> Actions { get; set; }

        private ReorderableList _list;

        public BuilderTarget()
        {
            IsBuilding = false;
            TargetName = "";
            Actions = new List<IAction>();
        }

        public void EditorInspector(BuildScenario Config)
        {
            if(_list == null)
            {
                _list = new ReorderableList(Actions, typeof(IAction), true, true, true, true);
                _list.drawHeaderCallback = (Rect r) => EditorGUI.LabelField(r, "Actions", EditorStyles.boldLabel);
                _list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => Actions[index].EditorListInspector(Config, -1, rect); 
                _list.onAddDropdownCallback = (Rect r, ReorderableList l) => BuilderWindow.AddDropdownCallback<IAction>(r, l);
                _list.onSelectCallback = BuilderWindow.OnSelectAction;
            }

            IsBuilding = EditorGUILayout.Toggle("Is need build", IsBuilding);
            TargetName = EditorGUILayout.TextField("Target name", TargetName);

            _list.DoLayoutList();

            /*
            if(_list.index >= 0 && _list.index < Actions.Count)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                var elem = Actions[_list.index];
                elem.EditorInspector(Config, -1);

                EditorGUILayout.EndVertical();
            }
            */
        }

        public void EditorListInspector(Rect rect)
        {
            EditorGUI.LabelField(rect, TargetName);
            IsBuilding = EditorGUI.Toggle(new Rect(rect.position + new Vector2(rect.size.x - 20.0f, 0.0f), new Vector2(20.0f, rect.size.y)), IsBuilding);

        }
    }
}