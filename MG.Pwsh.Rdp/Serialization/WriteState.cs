using System;
using System.Collections.Generic;
using System.Text;

namespace MG.Pwsh.Rdp
{
    public enum WriteState
    {
        Error = 0,
        Closed = 1,
        Setting = 2,
        Value = 3,
        EOL = 4
    }
}
