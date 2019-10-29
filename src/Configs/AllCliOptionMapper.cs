using System;
using System.Collections;
using System.Collections.Generic;

namespace BuilderScenario
{
    [TagAlias("AllCliOptionMap")]
    public class AllCliOptionMapper : IConfigMapData
    {
        public string Prefix { get; set; }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            var args = Environment.GetCommandLineArgs();
            var keyPrefix = $"--{Prefix}";

            for (int i = 0; i < args.Length; i++)
            {
                if (!args[i].StartsWith(keyPrefix))
                {
                    continue;
                }

                var key = args[i].Substring(keyPrefix.Length);
                var value = args[++i];
                
                yield return new KeyValuePair<string, object>(key, value);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}