using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BuilderScenario
{
    public class FlatTextLogger : IBuildLogger
    {
        private readonly StringBuilder _builder = new StringBuilder();
        private readonly Stack<string> _scopeStack = new Stack<string>();

        private string _indentLevel => string.Join("", Enumerable.Range(0, _scopeStack.Count).Select(i => "\t"));
        
        public void BeginScope(string scopeMessage)
        {
            _builder.AppendLine($"{_indentLevel}[{DateTime.Now:yyyy.MM.dd hh:mm:ss} | begin scope]: {scopeMessage}");
            _scopeStack.Push(scopeMessage);
        }

        public void EndScope()
        {
            var currentScope = _scopeStack.Pop();
            _builder.AppendLine($"{_indentLevel}[{DateTime.Now:yyyy.MM.dd hh:mm:ss} | end scope]: {currentScope}");
        }

        public void Log(string message)
        {
            _builder.AppendLine($"{_indentLevel}[{DateTime.Now:yyyy.MM.dd hh:mm:ss} | log]: {message}");
        }

        public void Warning(string message)
        {
            _builder.AppendLine($"{_indentLevel}[{DateTime.Now:yyyy.MM.dd hh:mm:ss} | warning]: {message}");
        }

        public void Error(string message)
        {
            _builder.AppendLine($"{_indentLevel}[{DateTime.Now:yyyy.MM.dd hh:mm:ss} | error]: {message}");
        }

        public void Exception(Exception exception)
        {
            _builder.AppendLine($"{_indentLevel}[{DateTime.Now:yyyy.MM.dd hh:mm:ss} | exception]: {exception.ToString()}");
        }

        public override string ToString()
        {
            return _builder.ToString();
        }
    }
}