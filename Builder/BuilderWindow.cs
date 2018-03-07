using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEditorInternal;
using System;

namespace BuilderScenario
{
    public class BuilderWindow : EditorWindow
    {
        private string PathToBuildConfig;
        private BuildScenario Config;

		private ReorderableList _targetList;
        private ReorderableList _pre_action_List;
        private ReorderableList _post_action_List;

        private ReorderableList _currentActionSelected;

        private static BuilderWindow _instance;


        [MenuItem("Build/Builder window")]
        public static void ShowWindow()
        {
            _instance = EditorWindow.GetWindow<BuilderWindow>();
        }

        void Awake()
        {
            if (EditorPrefs.HasKey("PathToBuildConfig"))
            {
                PathToBuildConfig = EditorPrefs.GetString("PathToBuildConfig");
            }
            else
            {
                if(EditorUtility.DisplayDialog("Loading", "Not has selected global scenario file. Select from file or load default scenario?", "Load from file", "Default"))
                {
                    PathToBuildConfig = SelectScenario();
                    EditorPrefs.SetString("PathToBuildConfig", PathToBuildConfig);
                }
                else
                {
                    InitConfig(true);
                    return;
                }
            }
            InitConfig();
        }

        void OnGUI()
        {

            #region toolbar
            GUILayout.BeginHorizontal(EditorStyles.toolbar);

            if (GUILayout.Toggle(false, "Load", EditorStyles.toolbarButton))
            {
                var _PathToBuildConfig = SelectScenario();
                if(EditorUtility.DisplayDialog("Warning", "Are saved current scenario?", "Ok", "Cancel"))
                {
                    PathToBuildConfig = _PathToBuildConfig;
                    InitConfig();
                }
            }

            if (GUILayout.Toggle(false, "Default", EditorStyles.toolbarButton))
            {
                InitConfig(true);
            }

            GUILayout.FlexibleSpace();

            if (GUILayout.Toggle(false, "Set global", EditorStyles.toolbarButton))
            {
                var _PathToBuildConfig = SelectScenario();
                EditorPrefs.SetString("PathToBuildConfig", _PathToBuildConfig);
            }

            if (GUILayout.Toggle(false, "Remove global key", EditorStyles.toolbarButton))
            {
                EditorPrefs.DeleteKey("PathToBuildConfig");
            }

            GUILayout.Space(30.0f);

            if (GUILayout.Toggle(false, "Save", EditorStyles.toolbarButton))
            {
                if(Config == null) return;

                var path = EditorUtility.SaveFilePanel("Save scenario", "", "build_scenario", "build");
                Config.Save(path);
            }

            GUILayout.Space(2.0f);

            if (GUILayout.Toggle(false, "?", EditorStyles.toolbarButton))
            {
                EditorUtility.DisplayDialog("Help", "Interpolation macros in strings:\n"+
                                                    "{NAME} - name of scenario\n"+
                                                    "{PATH} - path from general section of scenario\n" +
                                                    "{VER} - current version\n" +
                                                    "{BUILD} - current build\n" +
                                                    "{PROJECT_PATH} - path to unity project folder\n" +
                                                    "{TARGET_NAME} - name of target if avaliable", "OK", "");
            }

            GUILayout.EndHorizontal();
            #endregion



            if(Config != null)
            {
                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Scenario global settings: ", EditorStyles.boldLabel);

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                Config.Name = EditorGUILayout.TextField("Project name", Config.Name);

                EditorGUILayout.BeginHorizontal();
                Config.AbsolutePathForBuilds = EditorGUILayout.TextField("Path to build folder", Config.AbsolutePathForBuilds);
                if(GUILayout.Button("...", EditorStyles.miniButton, GUILayout.MaxWidth(25.0f)))
                {
                    Config.AbsolutePathForBuilds =
                        EditorUtility.OpenFolderPanel("Select path to build folder", Config.AbsolutePathForBuilds, "");
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                Config.BuildResultPath = EditorGUILayout.TextField("Path to build result", Config.BuildResultPath);
                if (GUILayout.Button("...", EditorStyles.miniButton, GUILayout.MaxWidth(25.0f)))
                {
                    Config.BuildResultPath =
                        EditorUtility.OpenFolderPanel("Select path to build result", Config.BuildResultPath, "");
                }
                EditorGUILayout.EndHorizontal();

                Config.Version = EditorGUILayout.TextField("Version", Config.Version);
                Config.Build = EditorGUILayout.IntField("Build", Config.Build);


                _pre_action_List.DoLayoutList();
                _post_action_List.DoLayoutList();
                _targetList.DoLayoutList();


                EditorGUILayout.EndVertical();

                if(_targetList.index >= 0 && _targetList.index < Config.Targets.Count)
                {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                    int ind = _targetList.index;
                    var elem = Config.Targets[ind];
                    elem.EditorInspector(Config);

                    EditorGUILayout.EndVertical();
                }


                if (_currentActionSelected != null && _currentActionSelected.index >= 0 && _currentActionSelected.index < _currentActionSelected.list.Count)
                {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    EditorGUILayout.LabelField("Action: ", EditorStyles.boldLabel);

                    var elem = (IAction)_currentActionSelected.list[_currentActionSelected.index];
                    elem.EditorInspector(Config, -1);

                    EditorGUILayout.EndVertical();
                }
            }
            else
            {
                EditorGUILayout.HelpBox("Scenario file not opened", MessageType.Info);
            }
        }

        string SelectScenario()
        {
            return EditorUtility.OpenFilePanelWithFilters("Select build scenario", "",
                    new string[] { "Builder scenario file", "build", "All files", "*" });
        }

        void InitConfig(bool isDefault = false)
        {
            try
            {
                var _Config = new BuildScenario();
                if(!isDefault)
                    _Config = BuildScenario.Load(PathToBuildConfig);

                var targetList = new ReorderableList(_Config.Targets, typeof(BuilderTarget), true, true, true, true);
                targetList.drawHeaderCallback = (Rect r) => EditorGUI.LabelField(r, "Target list", EditorStyles.boldLabel);
                targetList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => _Config.Targets[index].EditorListInspector(rect);
                targetList.onAddCallback = (ReorderableList l) => l.list.Add(new BuilderTarget());

                var pre_action_List = new ReorderableList(_Config.ActionsBeforeTargets, typeof(IAction), true, true, true, true);
                pre_action_List.drawHeaderCallback = (Rect r) => EditorGUI.LabelField(r, "Actions before targets", EditorStyles.boldLabel);
                pre_action_List.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => _Config.ActionsBeforeTargets[index].EditorListInspector(Config, -1, rect); 
                pre_action_List.onAddDropdownCallback = (Rect r, ReorderableList l) => AddDropdownCallback<IAction>(r, l);
                pre_action_List.onSelectCallback = OnSelectAction;

                var post_action_List = new ReorderableList(_Config.ActionsAfterTargets, typeof(IAction), true, true, true, true);
                post_action_List.drawHeaderCallback = (Rect r) => EditorGUI.LabelField(r, "Actions after targets", EditorStyles.boldLabel);
                post_action_List.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => _Config.ActionsAfterTargets[index].EditorListInspector(Config, -1, rect); 
                post_action_List.onAddDropdownCallback = (Rect r, ReorderableList l) => AddDropdownCallback<IAction>(r, l);
                post_action_List.onSelectCallback = OnSelectAction;

                Config = _Config;
                _targetList = targetList;
                _pre_action_List = pre_action_List;
                _post_action_List = post_action_List;
            }
            catch(Exception ex)
            {
                EditorUtility.DisplayDialog("Errow", "Error when initialize scenarion from selected file", "Ok", "");
                Debug.LogException(ex);
            }
        }

        public static void AddDropdownCallback<T>(Rect Rect, ReorderableList l)
        {
            var menu = new GenericMenu();


            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                    if (typeof(T).IsAssignableFrom(type) && !type.IsAbstract && type.IsClass)
                        menu.AddItem(new GUIContent(type.Name), false, addClickHandler<T>, (object)new BuilderActionActivator<T>() { type = type, target = (l.list as List<T>) });
            }

            menu.ShowAsContext();
        }

        public static void addClickHandler<T>(object target)
        {
            var CastedTarget = target as BuilderActionActivator<T>;

            CastedTarget.target.Add((T)Activator.CreateInstance(CastedTarget.type));
        }

        public static void OnSelectAction(ReorderableList l)
        {
            _instance._currentActionSelected = l;
        }
    }

    internal class BuilderActionActivator<T>
    {
        public Type type;
        public List<T> target;
    }
}