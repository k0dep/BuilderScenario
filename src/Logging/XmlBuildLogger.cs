using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace BuilderScenario
{
    public class XmlBuildLogger : IBuildLogger
    {
        private readonly StringBuilder _builder = new StringBuilder();
        private readonly Stack<string> _scopeStack = new Stack<string>();

        public XmlBuildLogger()
        {
            _builder.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        }

        private string _indentLevel => string.Join("", Enumerable.Range(0, _scopeStack.Count).Select(i => "\t"));
        
        public void BeginScope(string scopeMessage)
        {
            _builder.AppendLine($"{_indentLevel}<scope title=\"{Escape(scopeMessage)}\">");
            _scopeStack.Push(scopeMessage);
        }

        public void EndScope()
        {
            _scopeStack.Pop();
            _builder.AppendLine($"{_indentLevel}</scope>");
        }

        public void Log(string message)
        {
            _builder.AppendLine($"{_indentLevel}<log dt=\"{DateTime.Now:yyyy.MM.dd hh:mm:ss}\">{Escape(message)}</log>");
        }

        public void Warning(string message)
        {
            _builder.AppendLine($"{_indentLevel}<warning dt=\"{DateTime.Now:yyyy.MM.dd hh:mm:ss}\">{Escape(message)}</warning>");
        }

        public void Error(string message)
        {
            _builder.AppendLine($"{_indentLevel}<error dt=\"{DateTime.Now:yyyy.MM.dd hh:mm:ss}\">{Escape(message)}</error>");
        }

        public void Exception(Exception exception)
        {
            _builder.AppendLine($"{_indentLevel}<exception dt=\"{DateTime.Now:yyyy.MM.dd hh:mm:ss}\">{Escape(exception.ToString())}</exception>");
        }

        public override string ToString()
        {
            return _builder.ToString();
        }
        
        public string Escape(string text)
        {
            return new XElement("t", text).LastNode.ToString();
        } 
    }
}