namespace BuilderScenario
{
    [System.AttributeUsage(System.AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class SelfNamedTagAttrubute : System.Attribute
    {
        public SelfNamedTagAttrubute () {}
    }
    
    [System.AttributeUsage(System.AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class TagAliasAttribute : System.Attribute
    {
        public string Name { get; set; }

        public TagAliasAttribute(string name)
        {
            Name = name;
        }
    }
}