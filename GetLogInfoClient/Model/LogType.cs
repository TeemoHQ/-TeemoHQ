using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetLogInfoClient.Model
{

    public enum LogType
    {
        Log_Update,
        Log_Main,
        Log_Net,
        Log_Device,
        [Obsolete]
        Log_Cash,
        Log_POS,
        Log_Printer,

    }
}
