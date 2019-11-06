using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace BuilderScenario
{
    public class ConfigMap : IConfigMap
    {
        private readonly IBuildLogger Logger;
        private readonly IServiceCollection ServiceCollection;
        private readonly Dictionary<string, object> _data = new Dictionary<string, object>();

        public ConfigMap(IServiceCollection serviceCollection, IBuildLogger logger = null)
        {
            ServiceCollection = serviceCollection;
            Logger = logger;
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
            
            Logger?.Log($"requested config map: '{name}' value: '{result}'");

            return result;
        }

        public IConfigMap GetSection(string name)
        {
            if (_data.TryGetValue(name, out var section))
            {
                if (section is IConfigMapData sectionData)
                {
                    var sectionConfig = new ConfigMapSection(this, ServiceCollection, Logger);
                    sectionConfig.Fill(sectionData);
                    return sectionConfig;
                }
            }

            return null;
        }

        public void Set(string name, object value)
        {
            if (value == null)
            {
                return;
            }
            
            _data[name] = value;
        }

        public virtual string Interpolate(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return null;
            }
            
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
                if (_data.ContainsKey(variable))
                {
                    result = result.Replace($"${{{variable}}}", Get(variable, null, false));
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

        public IDictionary<string, object> Data => _data;
        
        public IEnumerable<KeyValuePair<string, object>> Fill(IEnumerable<KeyValuePair<string, object>> data)
        {
            ServiceCollection.Inject(data);

            var prevVars = new Dictionary<string, object>();

            foreach (var configEntry in data)
            {
                Logger?.Log($"set config '{configEntry.Key}' with value '{configEntry.Value}'");

                if (Data.TryGetValue(configEntry.Key, out var existsData))
                {
                    prevVars[configEntry.Key] = existsData;
                }
                
                Set(configEntry.Key, configEntry.Value);
            }

            return prevVars.AsEnumerable();
        }

        public void Set(IEnumerable<KeyValuePair<string, object>> data)
        {
            foreach (var prevVar in data)
            {
                Set(prevVar.Key, prevVar.Value);
            }
        }
    }

    public class ConfigMapSection : ConfigMap
    {
        public readonly ConfigMap Parent;
        public ConfigMapSection(ConfigMap parent, IServiceCollection serviceCollection, IBuildLogger logger = null)
            : base(serviceCollection, logger)
        {
            Parent = parent ?? throw new ArgumentNullException(nameof(parent));
        }

        public override string Interpolate(string input)
        {
            return Parent.Interpolate(base.Interpolate(input));
        }
    }
}