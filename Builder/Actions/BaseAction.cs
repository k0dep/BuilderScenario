using System.Collections;
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

    public interface IBuildJob
    {
        bool Enabled { get; set; }
        bool Run(IBuildContext context);
    }

    public interface IBuildJobInspector
    {
        void DrawInspector(BuildScenario conf);
    }
    
    public interface IBuildJobHeader
    {
        void DrawHeader(BuildScenario conf);
    }

    public interface IBuildListJob : IBuildJob
    {
        IBuildJob[] ChildJobs { get; }
    }

    public interface IBuildContext
    {
        ILogger Logger { get; }
        IDictionary ConfigStore { get; }
        T GetService<T>();
        BuildScenario Scenario { get; }
    }

    public class DummyJob : IBuildJob
    {
        public bool Enabled { get; set; }
        public bool Run(IBuildContext context)
        {
            return Enabled;
        }
    }
    
    public class DummyListJob : IBuildListJob
    {
        public bool Enabled { get; set; }
        public bool Run(IBuildContext context)
        {
            return Enabled;
        }

        public IBuildJob[] ChildJobs { get; set; }
    }
}
