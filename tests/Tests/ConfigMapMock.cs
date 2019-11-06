using System.Collections.Generic;

namespace BuilderScenario.Tests
{
    public class ConfigMapMock : IConfigMap
    {
        public string Get(string name, string defValue = null, bool useInterpolation = true)
        {
            return null;
        }

        public IConfigMap GetSection(string name)
        {
            throw new System.NotImplementedException();
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

        public IDictionary<string, object> Data => null;
        public IEnumerable<KeyValuePair<string, object>> Fill(IEnumerable<KeyValuePair<string, object>> data)
        {
            throw new System.NotImplementedException();
        }

        public void Set(IEnumerable<KeyValuePair<string, object>> data)
        {
            throw new System.NotImplementedException();
        }
    }
}