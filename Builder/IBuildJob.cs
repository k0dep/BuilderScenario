namespace BuilderScenario
{
    public interface IBuildJob
    {
        bool Enabled { get; set; }
    }
    
    public class DummyJob : IBuildJob
    {
        public bool Enabled { get; set; }
        
        public bool Run()
        {
            return Enabled;
        }
    }
}