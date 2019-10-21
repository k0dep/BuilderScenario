using System;
using System.Collections.Generic;

namespace BuilderScenario
{
    public class JobLoader
    {
        public T Deserialize<T>(string data)
        {
            var yamlBuilder = new YamlDotNet.Serialization.DeserializerBuilder();
            foreach (var type in GetMappingTypes())            
                yamlBuilder.WithTagMapping("!" + type.Value, type.Key);            
            
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
    }
}