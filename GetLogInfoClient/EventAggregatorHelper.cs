using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetLogInfoClient
{
    public class EventAggregatorHelper
    {
        private static readonly IEventAggregator eventAggregator = new EventAggregator();
        public static IEventAggregator EventAggregator
        {
            get { return eventAggregator; }
        }
    }
}
