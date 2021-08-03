using MG.Pwsh.Rdp.Extensions;
using MG.Pwsh.Rdp.Serialization;
using MG.Pwsh.Rdp.Settings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;

namespace MG.Pwsh.Rdp.Commands
{
    [Cmdlet(VerbsData.Out, "RdpFile")]
    [OutputType(typeof(RdpFile))]
    public class OutRdpFile : GetRdpFile
    {
        #region FIELDS/CONSTANTS


        #endregion

        #region PARAMETERS
        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        public RdpConfig InputObject { get; set; }

        [Parameter(Mandatory = true, Position = 0, ValueFromPipelineByPropertyName = true)]
        [Alias("FilePath")]
        public sealed override string Path { get; set; }

        public sealed override SwitchParameter Default
        {
            get => false;
            set => throw new NotImplementedException();
        }   // Not used in this command.

        [Parameter(Mandatory = false)]
        public SwitchParameter PassThru { get; set; }

        #endregion

        #region CMDLET PROCESSING
        protected override void BeginProcessing()
        {
            base.BeginProcessing();
        }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();
        }

        protected override void EndProcessing()
        {
            foreach (string path in this.ReturnPaths().TestPaths())
            {
                using (RdpWriter writer = new RdpWriter(path))
                {
                    this.InputObject.Write(writer);
                }

                if (this.PassThru)
                {
                    this.ReadAndOutputFile(path);
                }
            }
        }

        #endregion

        #region BACKEND METHODS
        private void ReadAndOutputFile(string path)
        {
            RdpFile file = RdpReader.Read(path);
            base.WriteObject(file);
        }

        #endregion
    }
}