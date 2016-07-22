using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Multicast_Practice
{
    class Program
    {
        public static int Num = 0;
        [STAThread]
        private static void Main(string[] args)
        {
            var IP = ConfigurationManager.AppSettings["IP"].ToString();
            var Port = ConfigurationManager.AppSettings["Port"].ToString();
            if (string.IsNullOrEmpty(IP))
            {
                return;
            }
            IPAddress address = IPAddress.Parse(IP);
            var multicast = new IPEndPoint(address, int.Parse(Port));
            var client = new UdpClient { MulticastLoopback = true };
            client.JoinMulticastGroup(multicast.Address);
            client.Connect(multicast);
            while (true)
            {
                Thread.Sleep(3000);
                if (Num == 0)
                {
                    var LogInfo = new LogInfo
                    {
                        Id = 1,
                        Time = DateTime.Now,
                        Level = 1,
                        Message = "1",
                        StackTrace = "1"
                    };
                    var list = new List<LogInfo>(); list.Add(LogInfo);
                     var LogPacket = new LogPacket
                    {
                        Name = "Log_Update",
                        IP = "1",
                        LogAllInfo = JsonConvert.SerializeObject(list)
                    };
                    var aa = JsonConvert.SerializeObject(LogPacket);
                    var bytes = Encoding.UTF8.GetBytes(aa);
                    Console.WriteLine("发送对象1：" + DateTime.Now.ToString());
                    client.Send(bytes, bytes.Length);
                    Num++;
                }
                else if (Num == 1)
                {
                    var LogInfo2 = new LogInfo
                    {
                        Id = 2,
                        Time = DateTime.Now,
                        Level = 2,
                        Message = "2",
                        StackTrace = "2"
                    };
                    var list = new List<LogInfo>(); list.Add(LogInfo2);
                    var LogPacket2 = new LogPacket
                    {
                        Name = "Log_Device",
                        IP = "2",
                        LogAllInfo = JsonConvert.SerializeObject(list)
                    };
                    var bb = JsonConvert.SerializeObject(LogPacket2);
                    var bytes = Encoding.UTF8.GetBytes(bb);
                    Console.WriteLine("发送对象2：" + DateTime.Now.ToString());
                    client.Send(bytes, bytes.Length);
                    Num++;
                }
                else
                {
                    var LogInfo3 = new LogInfo
                    {
                        Id = 3,
                        Time = DateTime.Now,
                        Level = 3,
                        Message = "3",
                        StackTrace = "3"
                    };

                    var list = new List<LogInfo>(); list.Add(LogInfo3);
                    var LogPacket3 = new LogPacket
                    {
                        Name = "Log_Cash",
                        IP = "3",
                        LogAllInfo = JsonConvert.SerializeObject(list)
                    };
                    var cc = JsonConvert.SerializeObject(LogPacket3);
                    var bytes = Encoding.UTF8.GetBytes(cc);
                    Console.WriteLine("发送对象3：" + DateTime.Now.ToString());
                    client.Send(bytes, bytes.Length);
                    Num = 0;
                }

            }
            client.Close();
        }

        public class LogPacket
        {
            public string Name { get; set; }

            public string IP { get; set; }

            public string LogAllInfo { get; set; }
        }

        public class LogInfo
        {
            public int Id { get; set; }

            public DateTime Time { get; set; }

            public int Level { get; set; }

            public string Message { get; set; }

            public string StackTrace { get; set; }
        }
    }
}
