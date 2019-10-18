using UnityEngine;
using System;
using System.Text;

namespace BuilderScenario
{
  public class BuildLog
  {
    protected StringBuilder log = new StringBuilder();
    public DateTime StartTime;
    public DateTime EndTime;

    public void Start()
    {
      this.StartTime = DateTime.Now;
      this.Line("Build started at time: " + this.StartTime.ToString(), (string) null, (string) null);
    }

    public void End()
    {
      this.EndTime = DateTime.Now;
      this.Line("Build end at time: " + this.StartTime.ToString() + "total build time: " + (this.EndTime - this.StartTime).ToString(), (string) null, (string) null);
    }

    public virtual void Line(string line, string who = null, string level = null)
    {
      string str = who == null ? "" : "[" + who + "] ";
      this.log.AppendFormat("[{0}] {1}{2}: {3} \n", (object) DateTime.Now.ToShortTimeString(), (object) (level == null ? "" : "[" + level + "] "), (object) str, (object) line);
    }

    public override string ToString()
    {
      return this.log.ToString();
    }
  }
  
  public class BuildLogTC : BuildLog
  {
    public override void Line(string line, string who = null, string level = null)
    {
      base.Line(line, who, level);
      Debug.Log((object) ("##teamcity[progressMessage '" + line + "']"));
    }
  }
}

