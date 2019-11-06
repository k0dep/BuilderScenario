using System.Collections.Generic;

namespace BuilderScenario
{
    public interface IConfigMap
    {
        string Get(string name, string defValue = null, bool useInterpolation = true);
        IConfigMap GetSection(string name);
        void Set(string name, object value);
        string Interpolate(string input);

        string this[string key] { get; set; }
        
        IDictionary<string, object> Data { get; }

        IEnumerable<KeyValuePair<string, object>> Fill(IEnumerable<KeyValuePair<string, object>> data);
        void Set(IEnumerable<KeyValuePair<string, object>> data);
    }
}