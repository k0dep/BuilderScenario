using System.Collections.Generic;

namespace BuilderScenario
{
    public class ConfigMap : IConfigMap
    {
        private readonly Dictionary<string, object> _data = new Dictionary<string, object>();
        
        public string Get(string name, string defValue = null, bool useInterpolation = true)
        {
            if (_data.TryGetValue(name, out var data))
            {
                if (useInterpolation)
                {
                    return Interpolate(data.ToString());
                }

                return data.ToString();
            }

            return defValue;
        }

        public void Set(string name, object value)
        {
            _data[name] = value;
        }

        public string Interpolate(string input)
        {
            foreach (var env in _data)
            {
                input = input.Replace($"${{{env.Key}}}", Interpolate(env.Value.ToString()));
            }

            return input;
        }

        public string this[string key]
        {
            get => Get(key, null, true);
            set => Set(key, value);
        }
    }
}