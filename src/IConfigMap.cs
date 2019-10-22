namespace BuilderScenario
{
    public interface IConfigMap
    {
        string Get(string name, string defValue = null, bool useInterpolation = true);
        void Set(string name, object value);
        string Interpolate(string input);

        string this[string key] { get; set; }
    }
}