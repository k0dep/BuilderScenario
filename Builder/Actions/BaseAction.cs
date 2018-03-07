using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BuilderScenario
{
    public interface IAction
    {
        bool CanAction { get; set; }
        void Do(BuildScenario conf, int target, BuildLog log);
        void EditorInspector(BuildScenario conf, int target);
        void EditorListInspector(BuildScenario conf, int target, Rect rect);

    }
}
