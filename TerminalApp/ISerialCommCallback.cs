using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalApp
{
    interface ISerialCommCallback
    {
        void setData(string data);
    }
}
