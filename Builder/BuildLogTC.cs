using UnityEngine;

namespace BuilderScenario
{
  public class BuildLogTC : BuildLog
  {
    public override void Line(string line, string who = null, string level = null)
    {
      base.Line(line, who, level);
      Debug.Log((object) ("##teamcity[progressMessage '" + line + "']"));
    }
  }
}
