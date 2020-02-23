using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalApp
{
    /// <summary>
    /// Interface to update Controls with data recieved on connected com port
    /// </summary>
    interface ISerialCommCallback
    {
        void setData(string data);
    }
}
