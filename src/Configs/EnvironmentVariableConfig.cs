using System;
using UnityEngine;

namespace BuilderScenario.Configs
{
    [TagAlias("Env")]
    public class EnvironmentVariableConfig
    {
        public string UnixEnv { get; set; } = null;
        public string WinEnv { get; set; } = null;
        
        public override string ToString()
        {
            if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.LinuxEditor)
            {
                return Environment.GetEnvironmentVariable(UnixEnv);
            }
            return Environment.GetEnvironmentVariable(WinEnv);
        }
    }
    
    [TagAlias("EnvPWD", true)]
    public class PWDEnvironmentVariableConfig
    {
        public override string ToString()
        {
            return Environment.CurrentDirectory;
        }
    }
}