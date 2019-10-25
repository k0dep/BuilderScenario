namespace BuilderScenario.Tests
{
    public class ConfigMapMock : IConfigMap
    {
        public string Get(string name, string defValue = null, bool useInterpolation = true)
        {
            return null;
        }

        public void Set(string name, object value)
        {
        }

        public string Interpolate(string input)
        {
            return null;
        }

        public string this[string key]
        {
            get => null;
            set{}
        }
    }
}