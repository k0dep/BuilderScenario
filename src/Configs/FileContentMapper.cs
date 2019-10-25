using System;
using System.IO;

namespace BuilderScenario
{
    [TagAlias("FileContentMap")]
    public class FileContentMapper : IInterpolable
    {
        public string FilePath { get; set; }
        
        public string Interpolate(IConfigMap configMap)
        {
            var filePath = configMap.Interpolate(FilePath);
            try
            {
                return File.ReadAllText(filePath);
            }
            catch (Exception e)
            {
                throw new Exception($"Cant read file at path: {filePath}", e);
            }
        }
    }
}