using GetLogInfoClient.Event;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetLogInfoClient.Model
{
    public class LogTheme: BindableBase
    {
        public string Name { get; set; }

        private bool isChecked;
        public bool IsChecked
        {
            get
            {
                return isChecked;
            }
            set
            {
                var arg = new LogThemeEventArg() { IsChecked= IsChecked,Name=Name };
                EventAggregatorHelper.EventAggregator.GetEvent<PubSubEvent<LogThemeEventArg>>().Publish(arg);
                this.SetProperty(ref this.isChecked,value);
            }
        }
    }
}
