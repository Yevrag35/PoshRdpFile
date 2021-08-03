using System;
using System.Collections.Generic;
using System.Text;

namespace MG.Pwsh.Rdp.Settings
{
    public enum AudioCaptureMode
    {
        DoNotCaptureAudio,
        CaptureAndSendToRemote
    }

    public enum AudioMode
    {
        PlaySoundsLocally,
        PlaySoundsRemotely,
        DoNotPlaySounds
    }

    public enum AudioQualityMode
    {
        DynamicallyAdjust,
        MediumQuality,
        BestQuality
    }

    public enum AuthenticationLevel
    {
        ConnectNoWarningOnFail,
        DoNotConnectOnFail,
        PromptOnFail,
        NotRequired
    }

    public enum ConnectionType
    {
        Modem56K,
        LowSpeedBroadband,
        Satellite,
        HighSpeedBroadband,
        WAN,
        LAN,
        Automatic       // Requires 'bandwidthautodetect'
    }

    public enum GatewayCredentialsSource
    {
        AskForPasswordNTLM = 0,
        UseSmartCard = 1,
        AllowUserToSelectLater = 4
    }

    public enum GatewayUsageMethod
    {
        DoNotUse,
        AlwaysUse,
        BypassForLocalAddresses,
        UseDefaults,
        DoNotUseAndSetBypass
    }

    public enum KeyboardHook
    {
        KeyCombosOnLocal,
        KeyCombosOnRemote,
        KeyCombosFullScreenOnly
    }

    public enum NegotiateSecurityLayer
    {
        UseSSL,
        UseNegotiate
    }

    public enum ScreenMode
    {
        Windowed = 1,
        FullScreen = 2
    }
}
