using System;

namespace BuilderScenario.Configs
{
    [TagAlias("EnvUniversal")]
    public class EnvironmentVariableUniversalConfig
    {
        public string Value { get; set; }

        public override string ToString()
        {
            return Environment.GetEnvironmentVariable(Value);
        }
    }
}