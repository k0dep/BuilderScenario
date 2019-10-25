using System.IO;
using System.Linq;

namespace BuilderScenario
{
    [TagAlias("PathCombineMap")]
    public class PathCombineMapper : IInterpolable
    {
        public string[] Values { get; set; }

        public override string ToString()
        {
            return Path.Combine(Values);
        }

        public string Interpolate(IConfigMap configMap)
        {
            return Path.GetFullPath(Path.Combine(Values.Select(configMap.Interpolate).ToArray()));
        }
    }
}