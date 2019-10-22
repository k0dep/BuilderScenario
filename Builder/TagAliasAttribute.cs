using System;

namespace BuilderScenario
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class TagAliasAttribute : Attribute
    {
        public string Name { get; set; }
        public bool IsEmpty { get; set; }

        public TagAliasAttribute(string name, bool isEmpty = false)
        {
            Name = name;
            IsEmpty = isEmpty;
        }
    }
}