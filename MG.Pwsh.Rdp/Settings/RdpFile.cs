using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MG.Pwsh.Rdp
{
    public class RdpFile : RdpConfig
    {
        public string FilePath { get; }

        internal RdpFile(string filePath)
            : base()
        {
            this.FilePath = filePath;
        }

        internal static RdpFile FromRead(RdpFile config, IDictionary<string, object> applySettings,
            IDictionary<string, string> reverse)
        {
            PopulateFromRead(config, applySettings, reverse);
            return config;
        }
    }
}
