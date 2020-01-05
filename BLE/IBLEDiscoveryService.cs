using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Bluetooth.LE;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace BLE
{
    public interface IBLEDiscoveryService
    {
        ScanFailure ScanFailure { get;}
        IList<ScanResult> ScanResults { get;}
        event PropertyChangedEventHandler PropertyChanged;
        void StartScan();
        void StopScan();

    }
}