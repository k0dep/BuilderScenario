using System;
using System.Collections;
using System.Collections.Generic;

namespace BuilderScenario
{
    [TagAlias("AllEnvironmentMap", true)]
    public class AllEnvironmentMapper : IConfigMapData
    {
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            foreach (DictionaryEntry env in Environment.GetEnvironmentVariables())
            {
                yield return new KeyValuePair<string, object>(env.Key.ToString(), env.Value);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}