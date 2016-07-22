using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetLogInfoClient.Model
{
    public class LogPacket
    {
        public string Name { get; set; }

        public string IP { get; set; }

        public string LogAllInfo { get; set; }

        public LogInfo LogInfo
        {
            get
            {
                
                if (!string.IsNullOrEmpty(LogAllInfo))
                {
                    return JsonConvert.DeserializeObject<List<LogInfo>>(LogAllInfo)[0];
                }
                return new LogInfo();
            }
        }

        public override string ToString()
        {
            return string.Format("名称：{0}\nIP地址：{1}\n日志内容：{2}\n", Name, IP, LogAllInfo);
        }

    }
}
