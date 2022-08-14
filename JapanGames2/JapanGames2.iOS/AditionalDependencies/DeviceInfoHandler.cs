using Foundation;
using JapanGames2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(JapanGames2.iOS.DeviceInfoHandler))]

namespace JapanGames2.iOS
{
    internal class DeviceInfoHandler : Interfaces.IDeviceInfoHandler
    {
        public string GetDeviceInfo()
        {
            UIDevice device = new UIDevice();

            return $"{device.SystemName} {device.SystemVersion}";
        }
    }
}