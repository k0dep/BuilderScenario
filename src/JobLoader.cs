using System;
using System.Collections.Generic;
using System.Linq;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.Converters;
using YamlDotNet.Serialization.ObjectFactories;

namespace BuilderScenario
{
    public class JobLoader
    {
        public T Deserialize<T>(string data)
        {
            var yamlBuilder = new DeserializerBuilder();
            foreach (var type in GetMappingTypes())            
                yamlBuilder.WithTagMapping("!" + type.Value, type.Key);
            
            yamlBuilder.WithTypeConverter(new EmptyConverter(GetEmptyTypes()));

            return yamlBuilder.Build().Deserialize<T>(data);
        }
        
        private static Dictionary<Type, string> GetMappingTypes()
        {         
            var res = new Dictionary<Type, string>();
            var allTypes = new List<Type>();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                allTypes.AddRange(assembly.GetTypes());

            foreach(var type in allTypes)
                foreach(var attr in type.GetCustomAttributes(false))
                    if(attr is TagAliasAttribute tag)
                    {
                        res.Add(type, tag.Name);
                        break;
                    }

            return res;
        }
        
        private static Dictionary<Type, string> GetEmptyTypes()
        {         
            var res = new Dictionary<Type, string>();
            var allTypes = new List<Type>();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                allTypes.AddRange(assembly.GetTypes());

            foreach(var type in allTypes)
                foreach(var attr in type.GetCustomAttributes(false))
                    if(attr is TagAliasAttribute tag && tag.IsEmpty)
                    {
                        res.Add(type, tag.Name);
                        break;
                    }

            return res;
        }
    }

    public class EmptyConverter : IYamlTypeConverter
    {
        private Dictionary<Type, string> _emptyTypes;
        
        public EmptyConverter(Dictionary<Type, string> getEmptyTypes)
        {
            _emptyTypes = getEmptyTypes;
        }

        public bool Accepts(Type type)
        {
            return _emptyTypes.ContainsKey(type);
        }

        public object ReadYaml(IParser parser, Type type)
        {
            parser.MoveNext();
            return Activator.CreateInstance(type);
        }

        public void WriteYaml(IEmitter emitter, object value, Type type)
        {
        }
    }
}