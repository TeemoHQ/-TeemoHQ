using GetLogInfoClient.Model;
using Microsoft.Practices.Prism.Mvvm;
using System.Windows;

namespace GetLogInfoClient.Dialog
{
    /// <summary>
    /// LogInfoDetailDialog.xaml 的交互逻辑
    /// </summary>
    public partial class LogInfoDetailDialog : Window
    {
        public LogInfoDetailDialog(LogPacket logPacket)
        {
            InitializeComponent();
            this.DetailTB.Text = string.Format("名称:{0}\nIP：{1}\n时间：{2}\n等级：{3}\n堆栈轨迹：{4}\n信息：{5}"
                , logPacket.Name
                , logPacket.IP
                , logPacket.LogInfo.Time.ToString("yyyy-MM-dd HH:mm:ss")
                , logPacket.LogInfo.Level.ToString()
                , logPacket.LogInfo.StackTrace
                , logPacket.LogInfo.Message);
        }
    }
}
