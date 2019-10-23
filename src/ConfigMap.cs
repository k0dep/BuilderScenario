using System.Collections.Generic;

namespace BuilderScenario
{
    public class ConfigMap : IConfigMap
    {
        private readonly IBuildLogger m_Logger;
        private readonly Dictionary<string, object> _data = new Dictionary<string, object>();

        public ConfigMap(IBuildLogger logger)
        {
            m_Logger = logger;
        }
        
        public string Get(string name, string defValue = null, bool useInterpolation = true)
        {
            m_Logger.Log($"requected config map '{name}'");
            if (_data.TryGetValue(name, out var data))
            {
                if (useInterpolation)
                {
                    if (data is IInterpolable interpolable)
                    {
                        return Interpolate(interpolable.Interpolate(this));
                    }
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
                input = input.Replace($"${{{env.Key}}}", env.Value.ToString());
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