using System.Collections;
using System.Collections.Generic;

namespace BuilderScenario
{
    [TagAlias("Map")]
    public class MapConfigData : IConfigMapData
    {
        public Dictionary<string, object> Values { get; set; }
        
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}