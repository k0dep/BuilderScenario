using System;
using System.Collections.Generic;
using UnityEngine;

namespace BuilderScenario
{
    public class UnityBuildLogger : IBuildLogger
    {
        private Stack<string>  _scopeStack = new Stack<string>();
        
        public void BeginScope(string scopeMessage)
        {
            _scopeStack.Push(scopeMessage);
            Debug.Log($"Begin scope: {scopeMessage}");
        }

        public void EndScope()
        {
            Debug.Log($"End scope: {_scopeStack.Pop()}");
        }

        public void Log(string message)
        {
            Debug.Log(message);
        }

        public void Warning(string message)
        {
            Debug.LogWarning(message);
        }

        public void Error(string message)
        {
            Debug.LogError(message);
        }

        public void Exception(Exception e)
        {
            Debug.LogException(e);
        }
    }
}