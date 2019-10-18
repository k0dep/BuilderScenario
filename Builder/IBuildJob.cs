using System.Diagnostics;
using UnityEditor;
using UnityEngine;

namespace BuilderScenario
{
    public interface IBuildJob
    {
        bool Disabled { get; set; }
    }
    
    [TagAlias("dummy")]
    public class DummyJob : IBuildJob
    {
        public bool Disabled { get; set; }
        
        public bool Run()
        {
            return Disabled;
        }
    }
    
    [TagAlias("debugLog")]
    public class LogMessageJob : IBuildJob
    {
        public bool Disabled { get; set; }
        
        public string Message { get; set; }
        
        public bool Run(ILogger logger)
        {
            logger.Log(Message);
            return Disabled;
        }
    }
    
    [TagAlias("SetVersion")]
    public class SetVersionJob : IBuildJob
    {
        public bool Disabled { get; set; }
        
        public string Version { get; set; }
        
        public void Run()
        {
            PlayerSettings.bundleVersion = Version;
        }
    }
}