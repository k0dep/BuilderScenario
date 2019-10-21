using System;
using System.Collections.Generic;

namespace BuilderScenario
{
    public class JobExecuteResult
    {
        public bool IsSucces { get; set; }
        public string Status { get; set;  }
        public string Title { get; set;  }
        public DateTime Start { get; set; }
        public DateTime Finish { get; set;  }
        public List<string> Logs { get; set; }

        public JobExecuteResult()
        {
            Logs = new List<string>();
        }
    }
}