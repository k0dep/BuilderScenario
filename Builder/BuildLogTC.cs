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
  
  public class BuildLog
  {
    public virtual void Line(string line, string who = null, string level = null)
    {
      Debug.Log((object) ("##teamcity[progressMessage '" + line + "']"));
    }

    public void Start()
    {
    }
    
    public void End()
    {
    }
  }
}
