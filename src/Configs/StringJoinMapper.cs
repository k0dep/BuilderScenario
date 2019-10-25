namespace BuilderScenario
{
    [TagAlias("StringJoinMap")]
    public class StringJoinMapper
    {
        public string Join { get; set; }
        public string[] Values { get; set; }
        
        public override string ToString()
        {
            return string.Join(Join, Values);
        }
    }
}