using System;

namespace BuilderScenario
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class TagAliasAttribute : Attribute
    {
        public string Name { get; set; }

        public TagAliasAttribute(string name)
        {
            Name = name;
        }
    }
}