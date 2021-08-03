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
    [Cmdlet(VerbsCommon.New, "RdpConfig")]
    public class NewRdpConfig : PSCmdlet
    {
        #region FIELDS/CONSTANTS


        #endregion

        #region PARAMETERS
        [Parameter(Mandatory = false)]
        public SwitchParameter AllowDesktopComposition { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter AllowFontSmoothing { get; set; }

        [Parameter(Mandatory = false)]
        public string AlternateShell { get; set; }

        [Parameter(Mandatory = false)]
        public AudioCaptureMode AudioCaptureMode { get; set; }

        [Parameter(Mandatory = false)]
        public AudioMode AudioMode { get; set; }

        [Parameter(Mandatory = false)]
        public AuthenticationLevel AuthenticationLevel { get; set; }

        [Parameter(Mandatory = false)]
        public bool AutoReconnectionEnabled { get; set; }

        [Parameter(Mandatory = false)]
        public bool BandwidthAutoDetect { get; set; }

        [Parameter(Mandatory = false)]
        public bool EnableCompression { get; set; }

        [Parameter(Mandatory = false)]
        public ConnectionType ConnectionType { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter DisableThemes { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter DisableWallpaper { get; set; }

        [Parameter(Mandatory = false)]
        public bool DisplayConnectionBar { get; set; }

        [Parameter(Mandatory = false)]
        public string DriveStoreDirect { get; set; }

        [Parameter(Mandatory = false)]
        public string FullAddress { get; set; }

        [Parameter(Mandatory = false)]
        public bool GatewayBrokeringType { get; set; }

        [Parameter(Mandatory = false)]
        public GatewayCredentialsSource GatewayCredentialsSource { get; set; }

        [Parameter(Mandatory = false)]
        public string GatewayHostName { get; set; }

        [Parameter(Mandatory = false)]
        public GatewayUsageMethod GatewayUsageMethod { get; set; }

        [Parameter(Mandatory = false)]
        public string KdcProxyName { get; set; }

        [Parameter(Mandatory = false)]
        public KeyboardHook KeyboardHook { get; set; }

        [Parameter(Mandatory = false)]
        public NegotiateSecurityLayer NegotiateSecurityLayer { get; set; }

        [Parameter(Mandatory = false)]
        public bool NetworkAutoDetect { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter PromptForCredentials { get; set; }

        [Parameter(Mandatory = false)]
        public bool PromptCredentialsOnce { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter RdgIsKdcProxy { get; set; }

        [Parameter(Mandatory = false)]
        public bool RedirectClipboard { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter RedirectComPorts { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter RedirectPosDevices { get; set; }

        [Parameter(Mandatory = false)]
        public bool RedirectPrinters { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter RedirectSmartCards { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter RemoteApplicationMode { get; set; }

        [Parameter(Mandatory = false)]
        public ScreenMode ScreenModeId { get; set; }

        [Parameter(Mandatory = false)]
        public string ShellWorkingDirectory { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter UseMultiMonitor { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter UseRedirectionServerName { get; set; }

        [Parameter(Mandatory = false)]
        public bool VideoPlaybackMode { get; set; }

        [Parameter(Mandatory = false)]
        public string WindowPosition { get; set; }

        [Parameter(Mandatory = false)]
        public Hashtable OtherSettings { get; set; }

        #endregion

        #region CMDLET PROCESSING
        protected override void BeginProcessing()
        {
            base.BeginProcessing();
        }

        protected override void ProcessRecord()
        {
            var config = new RdpConfig(this.MyInvocation.BoundParameters);
            this.SetGatewayUsage(this.MyInvocation.BoundParameters, config);
            base.WriteObject(config);
        }

        #endregion

        #region BACKEND METHODS

        private void SetGatewayUsage(IDictionary<string, object> boundParameters, RdpConfig config)
        {
            if (boundParameters.ContainsKey(nameof(GatewayUsageMethod)) &&
                this.GatewayUsageMethod != GatewayUsageMethod.DoNotUse &&
                this.GatewayUsageMethod != GatewayUsageMethod.DoNotUseAndSetBypass)
            {
                config.GatewayProfileUsageMethod = true;
            }
        }

        #endregion

        //public static HashSet<string> SettingsSet = new HashSet<string>(SettingNames.Keys, StringComparer.CurrentCultureIgnoreCase);
        //public static Dictionary<string, object> GetDefaults() => new Dictionary<string, object>
        //{
        //    //{ "administrative session", false },
        //    { "allow desktop composition", false },
        //    { "allow font smoothing", false },
        //    //{ "alternate full address", string.Empty },
        //    { "alternate shell", string.Empty },
        //    { "audiocapturemode", AudioCaptureMode.DoNotCaptureAudio },
        //    { "audiomode", AudioMode.PlaySoundsLocally },
        //    { "authentication level", AuthenticationLevel.PromptOnFail },
        //    { "autoreconnection enabled", true },
        //    { "bandwidthautodetect", true },
        //    { "bitmapcachepersistenable", true },
        //    { "compression", true },
        //    { "connection type", ConnectionType.Satellite },
        //    { "desktopheight", 600 },
        //    { "desktopwidth", 800 },
        //    { "disable cursor setting", false },
        //    { "disable full window drag", true },
        //    { "disable menu anims", true },
        //    { "disable themes", false },
        //    { "disable wallpaper", true },
        //    { "displayconnectionbar", true },
        //    { "drivestoredirect", string.Empty },
        //    { "enableworkspacereconnect", false },
        //    { "enablecredsspsupport", true },
        //    { "full address", string.Empty },
        //    { "gatewaybrokeringtype", 0 },
        //    { "gatewaycredentialssource", GatewayCredentialsSource.AllowUserToSelectLater },
        //    { "gatewayhostname", string.Empty },
        //    { "gatewayprofileusagemethod", false },
        //    { "gatewayusagemethod", Settings.GatewayUsageMethod.DoNotUseAndSetBypass },
        //    { "kdcproxyname", string.Empty },
        //    { "keyboardhook", KeyboardHook.KeyCombosFullScreenOnly },
        //    { "negotiate security layer", NegotiateSecurityLayer.UseNegotiate },
        //    { "networkautodetect", true },
        //    { "prompt for credentials", false },
        //    { "promptcredentialonce", true },
        //    { "rdgiskdcproxy", false },
        //    { "redirectclipboard", true },
        //    { "redirectcomports", false },
        //    { "redirectposdevices", false },
        //    { "redirectprinters", true },
        //    { "redirectsmartcards", false },
        //    { "remoteapplicationmode", false },
        //    { "screen mode id", ScreenMode.FullScreen },
        //    { "session bpp", 32 },
        //    { "shell working directory", string.Empty },
        //    { "use multimon", false },
        //    { "use redirection server name", false },
        //    { "videoplaybackmode", true },
        //    { "winposstr", "0,3,0,0,800,600" }
        //};
    }
}