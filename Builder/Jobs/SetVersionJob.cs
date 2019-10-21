using UnityEditor;

namespace BuilderScenario
{
    [TagAlias("SetVersion")]
    public class SetVersionJob : IBuildJob
    {
        public string Version { get; set; }
        
        public void Run()
        {
            PlayerSettings.bundleVersion = Version;
        }
    }
}