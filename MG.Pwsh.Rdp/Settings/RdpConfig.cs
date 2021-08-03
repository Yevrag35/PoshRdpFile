using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using MG.Pwsh.Rdp.Serialization;
using MG.Pwsh.Rdp.Settings;

namespace MG.Pwsh.Rdp
{
    public class RdpConfig
    {
        private const BindingFlags PROP_FLAGS = BindingFlags.Public | BindingFlags.Instance;
        internal StringBuilder Extras { get; set; }

        public bool AllowDesktopComposition { get; set; }
        public bool AllowFontSmoothing { get; set; }
        public string AlternateShell { get; set; }
        public AudioCaptureMode AudioCaptureMode { get; set; }
        public AudioMode AudioMode { get; set; } = AudioMode.PlaySoundsLocally;
        public AuthenticationLevel AuthenticationLevel { get; set; } = AuthenticationLevel.PromptOnFail;
        public bool AutoReconnectionEnabled { get; set; } = true;
        public bool BandwidthAutoDetect { get; set; } = true;
        public bool EnableCompression { get; set; } = true;
        public ConnectionType ConnectionType { get; set; } = ConnectionType.Satellite;
        public bool DisableThemes { get; set; }
        public bool DisableWallpaper { get; set; } = true;
        public bool DisplayConnectionBar { get; set; } = true;
        public string DriveStoreDirect { get; set; }
        public string FullAddress { get; set; }
        public bool GatewayBrokeringType { get; set; }
        public GatewayCredentialsSource GatewayCredentialsSource { get; set; } = GatewayCredentialsSource.AllowUserToSelectLater;
        public string GatewayHostName { get; set; }
        public bool GatewayProfileUsageMethod { get; set; }
        public GatewayUsageMethod GatewayUsageMethod { get; set; } = GatewayUsageMethod.DoNotUseAndSetBypass;
        public string KdcProxyName { get; set; }
        public KeyboardHook KeyboardHook { get; set; } = KeyboardHook.KeyCombosFullScreenOnly;
        public NegotiateSecurityLayer NegotiateSecurityLayer { get; set; } = NegotiateSecurityLayer.UseNegotiate;
        public bool NetworkAutoDetect { get; set; }
        public bool PromptForCredentials { get; set; }
        public bool PromptCredentialsOnce { get; set; }
        public bool RdgIsKdcProxy { get; set; }
        public bool RedirectClipboard { get; set; } = true;
        public bool RedirectComPorts { get; set; }
        public bool RedirectPosDevices { get; set; }
        public bool RedirectPrinters { get; set; } = true;
        public bool RedirectSmartCards { get; set; }
        public bool RemoteApplicationMode { get; set; }
        public ScreenMode ScreenModeId { get; set; } = ScreenMode.FullScreen;
        public string ShellWorkingDirectory { get; set; }
        public bool UseMultiMonitor { get; set; }
        public bool UseRedirectionServerName { get; set; }
        public bool VideoPlaybackMode { get; set; } = true;
        public string WindowPosition { get; set; } = "0,3,0,0,800,600";
        public OtherSettingsDictionary OtherSettings { get; set; } = new OtherSettingsDictionary();

        internal Dictionary<string, string> GetReverse() => SettingNames.ToDictionary(
            x => x.Value,
            e => e.Key,
            StringComparer.CurrentCultureIgnoreCase);

        private readonly Dictionary<string, string> SettingNames = new Dictionary<string, string>(37, StringComparer.CurrentCultureIgnoreCase)
        {
            { nameof(AllowDesktopComposition), "allow desktop composition" },
            { nameof(AllowFontSmoothing), "allow font smoothing" },
            { nameof(AlternateShell), "alternate shell" },
            { nameof(AudioCaptureMode), "audiocapturemode" },
            { nameof(AudioMode), "audiomode" },
            { nameof(AuthenticationLevel), "authentication level" },
            { nameof(AutoReconnectionEnabled), "autoreconnection enabled" },
            { nameof(BandwidthAutoDetect), "bandwidthautodetect" },
            { nameof(ConnectionType), "connection type" },
            { nameof(EnableCompression), "compression" },
            { nameof(DisableThemes), "disable themes" },
            { nameof(DisableWallpaper), "disable wallpaper" },
            { nameof(DisplayConnectionBar), "displayconnectionbar" },
            { nameof(DriveStoreDirect), "drivestoredirect" },
            { nameof(FullAddress), "full address" },
            { nameof(GatewayBrokeringType), "gatewaybrokeringtype" },
            { nameof(GatewayCredentialsSource), "gatewaycredentialsSource" },
            { nameof(GatewayHostName), "gatewayhostname" },
            { nameof(GatewayProfileUsageMethod), "gatewayprofileusagemethod" },
            { nameof(GatewayUsageMethod), "gatewayusagemethod" },
            { nameof(KdcProxyName), "kdcproxyname" },
            { nameof(KeyboardHook), "keyboardhook" },
            { nameof(NegotiateSecurityLayer), "negotiate security layer" },
            { nameof(NetworkAutoDetect), "networkautodetect" },
            { nameof(PromptForCredentials), "prompt for credentials" },
            { nameof(PromptCredentialsOnce), "promptcredentialonce" },
            { nameof(RdgIsKdcProxy), "rdgiskdcproxy" },
            { nameof(RedirectClipboard), "redirectclipboard" },
            { nameof(RedirectComPorts), "redirectcomports" },
            { nameof(RedirectPosDevices), "redirectposdevices" },
            { nameof(RedirectPrinters), "redirectprinters" },
            { nameof(RedirectSmartCards), "redirectsmartcards" },
            { nameof(RemoteApplicationMode), "remoteapplicationmode" },
            { nameof(ScreenModeId), "screen mode id" },
            { nameof(ShellWorkingDirectory), "shell working directory" },
            { nameof(UseMultiMonitor), "use multimon" },
            { nameof(UseRedirectionServerName), "use redirection server name" },
            { nameof(VideoPlaybackMode), "videoplaybackmode" },
            { nameof(WindowPosition), "winposstr" }
        };

        public RdpConfig()
        {
            this.Extras = new StringBuilder();
        }
        public RdpConfig(IDictionary<string, object> applySettings)
            : this()
        {
            Populate(this, applySettings, this.GetReverse());
        }

        private T Cast<T>(object o)
        {
            return (T)o;
        }
        protected private object CastForRead<T>(object o)
        {
            if (o is int intVal && typeof(T).Equals(typeof(bool)))
                return Convert.ToBoolean(intVal);

            else
                return Convert.ToString(o);
        }
        protected private T CastEnum<T>(object value) where T : Enum
        {
            if (value is int intVal)
            {
                return (T)Enum.ToObject(typeof(T), intVal);
            }
            else if (value is string strVal)
            {
                return (T)Enum.Parse(typeof(T), strVal);
            }
            else
                throw new InvalidCastException("Unable to cast " + nameof(value) + " to either type 'string' or 'int'.");
        }
        private static IDictionary CloneDictionary(IDictionary dictionary)
        {
            var hashtable = new Hashtable(dictionary.Count);
            foreach (DictionaryEntry de in dictionary)
            {
                hashtable.Add(de.Key, de.Value);
            }

            return hashtable;
        }
        private MethodInfo GetCastMethod() => this.GetType()
            .GetMethod("Cast", BindingFlags.NonPublic | BindingFlags.Instance);
        private MethodInfo GetCastForReadMethod(Type genericType) => this.GetType()
            .GetMethod("CastForRead", BindingFlags.NonPublic | BindingFlags.Instance).MakeGenericMethod(genericType);
        private MethodInfo GetCastEnumMethod(Type genericType) => this.GetType()
            .GetMethod("CastEnum", BindingFlags.NonPublic | BindingFlags.Instance).MakeGenericMethod(genericType);
        public string GetNonMatchingLines() => this.Extras.ToString();
        private Dictionary<string, PropertyInfo> GetPropertyInfos()
        {
            return this.GetType()
                .GetProperties(PROP_FLAGS)
                .Where(x => x.Name != nameof(RdpFile.FilePath))
                .ToDictionary(x => x.Name, StringComparer.CurrentCultureIgnoreCase);
        }
        private static void Populate(RdpConfig config, IDictionary<string, object> applySettings, IDictionary<string, string> reverse)
        {
            Dictionary<string, PropertyInfo> propDict = config.GetPropertyInfos();
            foreach (var kvp in applySettings)
            {
                if (null == kvp.Value || !propDict.ContainsKey(kvp.Key))
                    continue;

                if (!propDict[kvp.Key].PropertyType.Equals(typeof(OtherSettingsDictionary)))
                {
                    PropertyInfo pi = propDict[kvp.Key];
                    MethodInfo cast = null;
                    if (pi.PropertyType.IsEnum && !(kvp.Value is Enum))
                    {
                        cast = config.GetCastEnumMethod(pi.PropertyType);
                    }
                    else
                    {
                        cast = config.GetCastMethod().MakeGenericMethod(pi.PropertyType);
                    }

                    object appliedValue = cast.Invoke(config, new object[] { kvp.Value });

                    propDict[kvp.Key].SetValue(config, appliedValue);
                }
                else if (propDict[kvp.Key].PropertyType.Equals(typeof(OtherSettingsDictionary)) && kvp.Value is IDictionary ht)
                {
                    ht = config.WeedOutDuplicates(ht, reverse, propDict);
                    propDict[kvp.Key].SetValue(config, new OtherSettingsDictionary(ht));
                }
            }
        }
        protected static void PopulateFromRead(RdpConfig config, IDictionary<string, object> applySettings, IDictionary<string, string> reverse)
        {
            Dictionary<string, PropertyInfo> propDict = config.GetPropertyInfos();
            foreach (var kvp in applySettings)
            {
                if (!propDict.ContainsKey(kvp.Key))
                    continue;

                if (!propDict[kvp.Key].PropertyType.Equals(typeof(OtherSettingsDictionary)))
                {
                    PropertyInfo pi = propDict[kvp.Key];
                    MethodInfo cast = null;
                    if (pi.PropertyType.IsEnum && !(kvp.Value is Enum))
                    {
                        cast = config.GetCastEnumMethod(pi.PropertyType);
                    }
                    else
                    {
                        cast = config.GetCastForReadMethod(pi.PropertyType);
                    }

                    object appliedValue = cast.Invoke(config, new object[] { kvp.Value });

                    propDict[kvp.Key].SetValue(config, appliedValue);
                }
                else if (propDict[kvp.Key].PropertyType.Equals(typeof(OtherSettingsDictionary)) && kvp.Value is IDictionary ht)
                {
                    ht = config.WeedOutDuplicates(ht, reverse, propDict);
                    propDict[kvp.Key].SetValue(config, new OtherSettingsDictionary(ht));
                }
            }
        }
        private IDictionary WeedOutDuplicates(IDictionary dictionary, IDictionary<string, string> reverse, IDictionary<string, PropertyInfo> dict)
        {
            IDictionary newTable = new Hashtable();

            if (dictionary is Hashtable ht)
                newTable = (Hashtable)ht.Clone();

            else if (dictionary is ICloneable cloneable)
                newTable = (IDictionary)cloneable.Clone();

            else
                newTable = CloneDictionary(dictionary);

            foreach (DictionaryEntry de in dictionary)
            {
                object val = de.Value;

                if (de.Key is string key)
                {
                    PropertyInfo found = null;
                    if (reverse.ContainsKey(key) && dict.ContainsKey(reverse[key]))
                    {
                        found = dict[reverse[key]];
                    }
                    else if (dict.ContainsKey(key))
                    {
                        found = dict[key];
                    }
                    else
                    {
                        continue;
                    }
                    if (found.PropertyType.IsEnum)
                    {
                        var castEnumMethod = this.GetCastEnumMethod(found.PropertyType);
                        val = castEnumMethod.Invoke(this, new object[] { de.Value });
                    }

                    found.SetValue(this, val);
                    newTable.Remove(de.Key);
                }
            }

            return newTable;
        }
        public void Write(RdpWriter writer)
        {
            Type type = this.GetType();
            var dict = this.GetPropertyInfos();
            var reverse = this.GetReverse();

            foreach (var piKvp in dict.Where(x => x.Key != nameof(OtherSettings)))
            {
                writer.WriteSetting(SettingNames[piKvp.Key]);
                object value = piKvp.Value.GetValue(this);
                if (null == value)
                {
                    value = string.Empty;
                }

                writer.WriteValue(value);
            }

            foreach (var kvp in this.OtherSettings)
            {
                //if (dict.ContainsKey(kvp.Key) || (
                //    reverse.ContainsKey(kvp.Key) && dict.ContainsKey(reverse[kvp.Key])))
                //{
                //    continue;
                //}

                writer.WriteSetting(kvp.Key);
                object oVal = kvp.Value;
                if (null == oVal)
                    oVal = string.Empty;

                writer.WriteValue(oVal);
            }

            if (this.Extras.Length > 0)
            {
                writer.WriteRaw(this.Extras.ToString().Trim());
            }
        }

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
        //    { "gatewaybrokeringtype", false },
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