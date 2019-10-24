using System.Collections;
using System.Collections.Generic;

namespace BuilderScenario
{
    [TagAlias("ConfigSequence")]
    public class ConfigMapSequence : IConfigMapData
    {
        public IConfigMapData[] Sequence { get; set; }

        public void Inject(IServiceCollection serviceCollection)
        {
            foreach (var seqItem in Sequence)
            {
                serviceCollection.Inject(seqItem);
            }
        }
        
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            var merged = new Dictionary<string, object>();

            foreach (var sequenceEntry in Sequence)
            {
                foreach (var entry in sequenceEntry)
                {
                    merged[entry.Key] = entry.Value;
                }
            }

            return merged.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}