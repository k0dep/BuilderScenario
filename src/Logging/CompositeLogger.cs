using System;

namespace BuilderScenario
{
    public class CompositeLogger : IBuildLogger
    {
        private readonly IBuildLogger[] _loggers;

        public CompositeLogger(params IBuildLogger[] loggers)
        {
            _loggers = loggers;
        }

        public void BeginScope(string scopeMessage)
        {
            foreach (var logger in _loggers)
            {
                logger.BeginScope(scopeMessage);
            }
        }

        public void EndScope()
        {
            foreach (var logger in _loggers)
            {
                logger.EndScope();
            }
        }

        public void Log(string message)
        {
            foreach (var logger in _loggers)
            {
                logger.Log(message);
            }
        }

        public void Warning(string message)
        {
            foreach (var logger in _loggers)
            {
                logger.Warning(message);
            }
        }

        public void Error(string message)
        {
            foreach (var logger in _loggers)
            {
                logger.Error(message);
            }
        }

        public void Exception(Exception e)
        {
            foreach (var logger in _loggers)
            {
                logger.Exception(e);
            }
        }
    }
}