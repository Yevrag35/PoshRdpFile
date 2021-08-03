using MG.Pwsh.Rdp.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;

namespace MG.Pwsh.Rdp.Commands
{
    [Cmdlet(VerbsCommon.Get, "RdpFile", DefaultParameterSetName = "NonDefault")]
    [OutputType(typeof(RdpFile))]
    [Alias("Get-RdpConfig")]
    public class GetRdpFile : PSCmdlet
    {
        #region FIELDS/CONSTANTS
        private List<string> _paths;

        #endregion

        #region PARAMETERS
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true,
            ParameterSetName = "NonDefault")]
        [Alias("FullName")]
        public virtual string Path { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "Default")]
        public virtual SwitchParameter Default { get; set; }

        #endregion

        #region CMDLET PROCESSING
        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            _paths = new List<string>(1);
        }

        protected override void ProcessRecord()
        {
            if (!this.Default)
            {
                _paths.AddRange(this.GetPaths(this.Path));
            }
            else
            {
                _paths.Add(this.GetDefaultRdpPath());
            }
        }

        protected override void EndProcessing()
        {
            foreach (string path in _paths)
            {
                base.WriteObject(RdpReader.Read(path));
            }
        }

        #endregion

        #region BACKEND METHODS
        private string GetDefaultRdpPath()
        {
            string docsFol = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            return System.IO.Path.Combine(docsFol, "Default.rdp");
        }
        private IEnumerable<string> GetPaths(string path)
        {
            IEnumerable<string> resolved = null;
            try
            {
                resolved = base.GetResolvedProviderPathFromPSPath(path, out ProviderInfo pi)
                    .Distinct(StringComparer.CurrentCultureIgnoreCase);
            }
            catch (ItemNotFoundException)
            {
                resolved = new string[1] { base.GetUnresolvedProviderPathFromPSPath(path) };
            }

            if (null != resolved)
            {
                foreach (string p in resolved)
                {
                    yield return p;
                }
            }
        }
        protected private IEnumerable<string> ReturnPaths() => _paths;

        #endregion
    }
}