using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

[assembly: Dependency(typeof(JapanGames2.Droid.DeviceInfoHandler))]

namespace JapanGames2.Droid
{
    internal class DeviceInfoHandler : Interfaces.IDeviceInfoHandler
    {
        public string GetDeviceInfo()
        {
            return $"Android {Build.VERSION.Release}";
        }
    }
}