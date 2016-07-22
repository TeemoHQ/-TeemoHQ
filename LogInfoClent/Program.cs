using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LogInfoClent
{
    class Program
    {
        static void Main(string[] args)
        {
            var IP = ConfigurationManager.AppSettings["IP"].ToString();
            var Port = ConfigurationManager.AppSettings["Port"].ToString();
            if (string.IsNullOrEmpty(IP))
            {
                return;
            }
            IPAddress address = IPAddress.Parse(IP);
            var multicast = new IPEndPoint(address, 0);
            var client = new UdpClient(int.Parse(Port)) { MulticastLoopback = true };
            client.JoinMulticastGroup(multicast.Address);
            while (true)
            {
                var bytes = client.Receive(ref multicast);
                var line = Encoding.UTF8.GetString(bytes);
                LogPacket logPacket = JsonConvert.DeserializeObject<LogPacket>(line);
                Console.WriteLine("名称：{0}", logPacket.Name);
                Console.WriteLine("IP地址：{0}", logPacket.IP);
                Console.WriteLine("日志地址：{0}", logPacket.LogAllInfo);
                Console.WriteLine("-------------------------------------分割线-------------------------------------");
                if (string.IsNullOrEmpty(line))
                    break;
            }
            client.Close();
        }
    }

    public class LogPacket
    {
        public string Name { get; set; }

        public string IP { get; set; }

        public string LogAllInfo { get; set; }
    }
}
