using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace BuilderScenario
{
    public class XmlBuildDirectToFileLogger : IBuildLogger
    {
        private readonly StringBuilder _builder = new StringBuilder();
        private readonly Stack<string> _scopeStack = new Stack<string>();
        
        private readonly StreamWriter _file;

        public XmlBuildDirectToFileLogger(string logFilePath)
        {
            _file = new StreamWriter(File.OpenWrite(logFilePath));
            Write("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        }

        ~XmlBuildDirectToFileLogger()
        {
            _file.Flush();
            _file.Close();
        }

        private string _indentLevel => string.Join("", Enumerable.Range(0, _scopeStack.Count).Select(i => "\t"));
        
        public void BeginScope(string scopeMessage)
        {
            Write($"{_indentLevel}<scope title=\"{Escape(scopeMessage)}\">");
            _scopeStack.Push(scopeMessage);
        }

        public void EndScope()
        {
            _scopeStack.Pop();
            Write($"{_indentLevel}</scope>");
        }

        public void Log(string message)
        {
            Write($"{_indentLevel}<log dt=\"{DateTime.Now:yyyy.MM.dd hh:mm:ss}\">{Escape(message)}</log>");
        }

        public void Warning(string message)
        {
            Write($"{_indentLevel}<warning dt=\"{DateTime.Now:yyyy.MM.dd hh:mm:ss}\">{Escape(message)}</warning>");
        }

        public void Error(string message)
        {
            Write($"{_indentLevel}<error dt=\"{DateTime.Now:yyyy.MM.dd hh:mm:ss}\">{Escape(message)}</error>");
        }

        public void Exception(Exception exception)
        {
            Write($"{_indentLevel}<exception dt=\"{DateTime.Now:yyyy.MM.dd hh:mm:ss}\">{Escape(exception.ToString())}</exception>");
        }

        public override string ToString()
        {
            return _builder.ToString();
        }
        
        public string Escape(string text)
        {
            return new XElement("t", text).LastNode.ToString();
        }

        private void Write(string text)
        {
            _builder.AppendLine(text);
            _file.WriteLine(text);
            _file.Flush();
        }
    }
}