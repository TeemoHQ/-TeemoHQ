using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetLogInfoClient.Model
{
    public class LogInfo
    {
        public int Id { get; set; }

        public DateTime Time { get; set; }


        public int Level { get; set; }

        public string Message { get; set; }

        public string StackTrace { get; set; }
    }
}
