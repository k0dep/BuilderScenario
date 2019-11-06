using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BuilderScenario
{
    [TagAlias("Map")]
    public class MapConfigData : IConfigMapData
    {
        public Dictionary<string, object> Values { get; set; }
        
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return Values.Select(MapObjectToRightValue).GetEnumerator();
        }

        private KeyValuePair<string, object> MapObjectToRightValue(KeyValuePair<string, object> arg)
        {
            // Для того чтобы bool.ToString() возвращал не True а true
            if (arg.Value is bool boolValue)
            {
                return new KeyValuePair<string, object>(arg.Key, boolValue.ToString().ToLower());
            }

            return arg;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}