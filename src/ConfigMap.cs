using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace BuilderScenario
{
    public class ConfigMap : IConfigMap
    {
        private readonly IBuildLogger m_Logger;
        private readonly Dictionary<string, object> _data = new Dictionary<string, object>();

        public ConfigMap(IBuildLogger logger = null)
        {
            m_Logger = logger;
        }
        
        public string Get(string name, string defValue = null, bool useInterpolation = true)
        {
            var result = defValue;
            
            if (_data.TryGetValue(name, out var data))
            {
                if (useInterpolation)
                {
                    if (data is IInterpolable interpolable)
                        result = Interpolate(interpolable.Interpolate(this));
                    else
                        result = Interpolate(data.ToString());
                }
                else
                    result = data.ToString();
            }
            
            m_Logger?.Log($"requested config map: '{name}' value: '{result}'");

            return result;
        }

        public void Set(string name, object value)
        {
            _data[name] = value;
        }

        public string Interpolate(string input)
        {
            var matchers = string.Join("|", _data.Keys)
                    .Replace(".", "\\.")
                    .Replace("-", "\\-")
                    .Replace(":", "\\:");
            
            var result = input;
            const int MAX_ITERATIONS = 100;
            var i = 0;
            for (; i <= MAX_ITERATIONS; i++)
            {
                var matches = Regex.Match(result, $"\\$\\{{(?<match>{matchers})\\}}");
                if (!matches.Success)
                    break;

                var variable = matches.Groups["match"].Value;
                if (_data.TryGetValue(variable, out var value))
                {
                    result = result.Replace($"${{{variable}}}", value.ToString());
                }
            }

            if (i == MAX_ITERATIONS)
            {
                throw new StackOverflowException($"cant intrepolate string '{input}'. check recursive variables");
            }

            return result;
        }

        public string this[string key]
        {
            get => Get(key, null, true);
            set => Set(key, value);
        }
    }
}