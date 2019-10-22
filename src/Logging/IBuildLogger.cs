using System;

namespace BuilderScenario
{
    public interface IBuildLogger
    {
        void BeginScope(string scopeMessage);
        void EndScope();

        void Log(string message);
        void Warning(string message);
        void Error(string message);
        void Exception(Exception e);
    }
}