using MG.Attributes;
using MG.Pwsh.Rdp.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MG.Pwsh.Rdp
{
    public enum SettingType
    {
        [AdditionalValue("s")]
        //[Type(typeof(string))]
        String,

        [AdditionalValue("i")]
        //[Type(typeof(int))]
        Integer
    }
}
