using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BuilderScenario
{
    [TagAlias("ConfigFile")]
    public class ConfigMapFileData : IConfigMapData
    {
        public string FileName { get; set; }
        public bool AllowFails { get; set; } = false;

        private IObjectLoader _loader;
        private IServiceCollection _servoceCollection;

        public void Inject(IObjectLoader loader, IServiceCollection serviceCollection)
        {
            _loader = loader;
            _servoceCollection = serviceCollection;
        }
        
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            try
            {
                var data = _loader.Load<IConfigMapData>(FileName);
                _servoceCollection.Inject(data);
                return data.GetEnumerator();
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load config file at path: {FileName}");

                if (!AllowFails)
                {
                    throw;
                }
                else
                {
                    Debug.LogException(e);
                }
            }

            return new Dictionary<string, object>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}