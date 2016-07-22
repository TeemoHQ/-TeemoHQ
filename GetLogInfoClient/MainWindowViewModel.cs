using GetLogInfoClient.Event;
using GetLogInfoClient.Model;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GetLogInfoClient
{
    public class MainWindowViewModel : BindableBase
    {
        private static string ip = ConfigurationManager.AppSettings["IP"];
        private static string port = ConfigurationManager.AppSettings["Port"];
        private bool isRuning = false;
        private bool isCancle = true;
        private List<string> includeFiltes;
        private List<string> unIncludeFiltes;
        private List<LogPacket> removeIncludeLogPacket;
        private List<LogPacket> removeUnIncludeLogPacket;
        public MainWindowViewModel()
        {
            this.Init();
        }
        private void Init()
        {
            foreach (var name in Enum.GetNames(typeof(LogType)))
            {
                LogThemeSource.Add(new LogTheme
                {
                    IsChecked = true,
                    Name = name
                });
            }
            allLogPackets = new ObservableCollection<LogPacket>();
            this.CurrentLogPackets = new ObservableCollection<LogPacket>();
            removeIncludeLogPacket = new List<LogPacket>();
            removeUnIncludeLogPacket = new List<LogPacket>();
            EventAggregatorHelper.EventAggregator.GetEvent<PubSubEvent<LogThemeEventArg>>().Subscribe(this.HistoryLogThemeFliterHandler);
        }

        private static ObservableCollection<LogPacket> allLogPackets;

        private ObservableCollection<LogPacket> currentLogPackets;
        public ObservableCollection<LogPacket> CurrentLogPackets
        {
            get
            {
                //test data
                //if (currentLogPackets == null)
                //{
                //    var test1 = new LogPacket() { Name = "Log_Main", IP = "Test1" };
                //    var test2 = new LogPacket() { Name = "Log_Net", IP = "Test2" };
                //    currentLogPackets = new ObservableCollection<LogPacket>();
                //    currentLogPackets.Add(test1);
                //    currentLogPackets.Add(test2);
                //    allLogPackets.Add(test1);
                //    allLogPackets.Add(test2);
                //}
                return currentLogPackets;
            }
            set
            {
                this.SetProperty(ref this.currentLogPackets, value);
            }
        }

        private ObservableCollection<LogTheme> logThemeSource;
        public ObservableCollection<LogTheme> LogThemeSource
        {
            get
            {
                return logThemeSource ?? (logThemeSource = new ObservableCollection<LogTheme>());
            }
            set
            {
                this.SetProperty(ref this.logThemeSource, value);
            }
        }

        private LogPacket selectedItem;
        public LogPacket SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                this.SetProperty(ref this.selectedItem, value);
            }
        }

        private string includeFilteText;
        public string IncludeFilteText
        {
            get
            {
                return includeFilteText;
            }
            set
            {
                this.SetProperty(ref this.includeFilteText, value);
                if (!string.IsNullOrEmpty(IncludeFilteText))
                {
                    includeFiltes = includeFilteText.Split('；').ToList();
                }
                else
                {
                    includeFiltes = null;
                }
                HistoryQueryFliterHander();
            }
        }

        private string unIncludeFilteText;
        public string UnIncludeFilteText
        {
            get
            {
                return unIncludeFilteText;
            }
            set
            {
                this.SetProperty(ref this.unIncludeFilteText, value);
                if (!string.IsNullOrEmpty(UnIncludeFilteText))
                {
                    unIncludeFiltes = unIncludeFilteText.Split('；').ToList();
                }
                else
                {
                    unIncludeFiltes = null;
                }
                HistoryQueryFliterHander(false);
            }
        }

        private bool listenStartButtonIsEnabled = true;
        public bool ListenStartButtonIsEnabled
        {
            get
            {
                return listenStartButtonIsEnabled;
            }
            set
            {
                this.SetProperty(ref this.listenStartButtonIsEnabled, value);
            }
        }

        private bool cancleButtonIsEnabled;
        public bool CancleButtonIsEnabled
        {
            get
            {
                return cancleButtonIsEnabled;
            }
            set
            {
                this.SetProperty(ref this.cancleButtonIsEnabled, value);
            }
        }
        public ICommand CleanCommand
        {
            get
            {
                return new DelegateCommand(this.Clean);
            }
        }
        public ICommand CancleCommand
        {
            get
            {
                return new DelegateCommand(this.Cancle);
            }
        }
        public ICommand ListenStartCommand
        {
            get
            {
                return new DelegateCommand(this.ListenStart);
            }
        }
        private void Clean()
        {
            this.CurrentLogPackets.Clear();
        }
        private void Cancle()
        {
            this.isRuning = false;
            isCancle = true;
            this.CancleButtonIsEnabled = false;
            this.ListenStartButtonIsEnabled = true;
        }
        private void ListenStart()
        {
            this.CancleButtonIsEnabled = true;
            this.ListenStartButtonIsEnabled = false;
            this.isRuning = true;
            isCancle = false;
            Task.Factory.StartNew(Listen);
        }
        private void Listen()
        {
            if (string.IsNullOrEmpty(ip) || string.IsNullOrEmpty(port))
            {
                return;
            }
            IPAddress address = IPAddress.Parse(ip);
            var multicast = new IPEndPoint(address, 0);
            var udpClient = new UdpClient(int.Parse(port)) { MulticastLoopback = true };
            Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(100);
                    if (!this.isRuning && isCancle)
                    {
                        udpClient.Close();
                        break;
                    }
                }
            });
            udpClient.JoinMulticastGroup(multicast.Address);
            try
            {
                while (true)
                {
                    if (!this.isRuning)
                    {
                        break;
                    }
                    var bytes = udpClient.Receive(ref multicast);
                    var line = Encoding.UTF8.GetString(bytes);
                    LogPacket logPacket = JsonConvert.DeserializeObject<LogPacket>(line);
                    allLogPackets.Add(logPacket);
                    this.RealTimeFilter(logPacket);

                }
            }
            catch (Exception ex)
            {
                if (!this.isRuning && isCancle)
                {
                    udpClient.Close();
                }
            }
            udpClient.Close();
            this.isCancle = true;
        }
        private void RealTimeFilter(LogPacket logPacket = null)
        {

            foreach (var logTheme in this.LogThemeSource)
            {
                if (!logTheme.IsChecked)
                {
                    if (logPacket.Name == logTheme.Name)
                    {
                        return;
                    }
                }

            }
            var message = logPacket.ToString();
            if (this.includeFiltes != null && this.includeFiltes.Count >= 0)
            {
                foreach (var item in this.includeFiltes)
                {
                    if (string.IsNullOrEmpty(item))
                    {
                        continue;
                    }
                    if (!message.Contains(item))
                    {
                        return;
                    }
                }
            }
            if (this.unIncludeFiltes != null && this.unIncludeFiltes.Count >= 0)
            {
                foreach (var item in this.unIncludeFiltes)
                {
                    if (string.IsNullOrEmpty(item))
                    {
                        continue;
                    }
                    if (message.Contains(item))
                    {
                        return;
                    }
                }
            }
            App.Current.Dispatcher.Invoke(() =>
            {
                this.CurrentLogPackets.Add(logPacket);
            });

        }
        private void HistoryLogThemeFliterHandler(LogThemeEventArg arg)
        {
            lock (this.CurrentLogPackets)
            {
                if (arg.IsChecked)
                {
                    var removeItems = new List<LogPacket>();
                    foreach (var item in this.CurrentLogPackets)
                    {
                        if (item.Name == arg.Name)
                        {
                            removeItems.Add(item);
                        }
                    }
                    removeItems.ForEach(p => this.CurrentLogPackets.Remove(p));
                }
                else
                {
                    lock (allLogPackets)
                    {
                        foreach (var item in allLogPackets)
                        {
                            if (item.Name == arg.Name)
                            {
                                this.CurrentLogPackets.Add(item);
                            }
                        }
                    }
                }
            }
        }
        private void HistoryQueryFliterHander(bool isInclude = true)
        {
            if (isInclude)
            {
                if (this.includeFiltes != null && this.includeFiltes.Count >= 0)
                {
                    var list = new List<LogPacket>();
                    foreach (var item in this.includeFiltes)
                    {
                        if (string.IsNullOrEmpty(item))
                        {
                            continue;
                        }
                        foreach (var logPack in this.CurrentLogPackets)
                        {
                            if (!logPack.ToString().Contains(item))
                            {
                                list.Add(logPack);
                            }
                        }
                        list.ForEach((p) => { this.CurrentLogPackets.Remove(p); this.removeIncludeLogPacket.Add(p); });
                    }
                }
                else
                {
                    if (this.removeIncludeLogPacket != null)
                    {
                        var templist = this.removeIncludeLogPacket.Distinct();
                        foreach (var item in templist)
                        {
                            this.CurrentLogPackets.Add(item);
                        }
                    }
                }

            }
            else
            {
                if (this.unIncludeFiltes != null && this.unIncludeFiltes.Count >= 0)
                {
                    foreach (var item in this.unIncludeFiltes)
                    {
                        var list = new List<LogPacket>();
                        if (string.IsNullOrEmpty(item))
                        {
                            continue;
                        }
                        foreach (var logPack in this.CurrentLogPackets)
                        {
                            if (logPack.ToString().Contains(item))
                            {
                                list.Add(logPack);
                            }
                        }
                        list.ForEach((p) => { this.CurrentLogPackets.Remove(p); this.removeUnIncludeLogPacket.Add(p); });
                    }
                }
                else
                {
                    if (this.removeUnIncludeLogPacket != null)
                    {
                        var templist = this.removeUnIncludeLogPacket.Distinct();
                        foreach (var item in templist)
                        {
                            this.CurrentLogPackets.Add(item);
                        }
                    }
                }
            }
        }
    }
}
