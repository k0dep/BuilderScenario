using System.Collections;
using System.Collections.Generic;

namespace BuilderScenario
{
    [TagAlias("ConfigFile")]
    public class ConfigMapFileData : IConfigMapData
    {
        public string FileName { get; set; }

        private IObjectLoader _loader;
        private IServiceCollection _servoceCollection;

        public void Inject(IObjectLoader loader, IServiceCollection serviceCollection)
        {
            _loader = loader;
            _servoceCollection = serviceCollection;
        }
        
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            var data = _loader.Load<IConfigMapData>(FileName);
            _servoceCollection.Inject(data);
            return data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}