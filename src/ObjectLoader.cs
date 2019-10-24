using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.Converters;
using YamlDotNet.Serialization.ObjectFactories;

namespace BuilderScenario
{
    public class ObjectLoader : IObjectLoader
    {
        public IBuildLogger Logger { get; set; }
        public string BasePath { get; set; }

        private readonly Deserializer _deserializer;

        public ObjectLoader(string basePath, IBuildLogger logger)
        {
            BasePath = basePath;
            Logger = logger;
            
            var yamlBuilder = new DeserializerBuilder();
            foreach (var type in GetMappingTypes())
                yamlBuilder.WithTagMapping("!" + type.Value, type.Key);
            
            yamlBuilder.WithTypeConverter(new EmptyConverter(GetEmptyTypes()));
            
            _deserializer = yamlBuilder.Build();
        }

        public T Load<T>(string configRelativePath)
        {
            var path = Path.Combine(BasePath, configRelativePath);
            var data = File.ReadAllText(path);
            Logger.Log($"loaded file at path: {path}");
            return Deserialize<T>(data);
        }

        public T Deserialize<T>(string data)
        {
            var result = _deserializer.Deserialize<T>(data);
            
            Logger.Log($"deserialized config to type {result.GetType()}, TType: {typeof(T)}");
            
            return result;
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