using UnityEngine;

namespace BuilderScenario
{
    [TagAlias("debugLog")]
    public class LogMessageJob : IBuildJob
    {
        public string Message { get; set; }
        
        public void Run(ILogger logger)
        {
            logger.Log(Message);
        }
    }
}