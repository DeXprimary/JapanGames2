using JapanGames2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.System.Profile;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(JapanGames2.UWP.DeviceInfoHandler))]

namespace JapanGames2.UWP
{
    internal class DeviceInfoHandler : Interfaces.IDeviceInfoHandler
    {
        public string GetDeviceInfo()
        {
            EasClientDeviceInformation devInfo = new EasClientDeviceInformation();

            var deviceFamilyVersion = AnalyticsInfo.VersionInfo.DeviceFamilyVersion;
            var version = ulong.Parse(deviceFamilyVersion);
            var majorVersion = (version & 0xFFFF000000000000L) >> 48;
            var minorVersion = (version & 0x0000FFFF00000000L) >> 32;
            var buildVersion = (version & 0x00000000FFFF0000L) >> 16;
            var revisionVersion = (version & 0x000000000000FFFFL);
            var systemVersion = $"{majorVersion}.{minorVersion}.{buildVersion}.{revisionVersion}";

            return $"{devInfo.OperatingSystem} {systemVersion}";
        }
    }
}
