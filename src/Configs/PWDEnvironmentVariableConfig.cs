using System;

namespace BuilderScenario.Configs
{
    [TagAlias("EnvPWD", true)]
    public class PWDEnvironmentVariableConfig
    {
        public override string ToString()
        {
            return Environment.CurrentDirectory;
        }
    }
}