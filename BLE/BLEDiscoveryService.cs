using System;
using System.Collections.Generic;
using System.ComponentModel;
using Android.App;
using Android.Bluetooth;
using Android.Bluetooth.LE;
using Android.Runtime;

namespace BLE
{

    public class BLECallback : ScanCallback, INotifyPropertyChanged
    {

        private ScanFailure scanFailure;
        private IList<ScanResult> scanResults = null;

        public ScanFailure ScanFailure
        {
            get
            {
                return this.scanFailure;
            }

            private set
            {
                this.scanFailure = value;
                OnPropertyChanged("ScanFailure");
                
            }
        }
        public IList<ScanResult> ScanResults
        {
            get
            {
                return this.scanResults;
            }

            private set
            {
                this.scanResults = value;
                OnPropertyChanged("ScanResults");
                
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public override void OnScanResult([GeneratedEnum] ScanCallbackType callbackType, ScanResult result)
        {
            base.OnScanResult(callbackType, result);
        }

        public override void OnScanFailed([GeneratedEnum] ScanFailure errorCode)
        {
            this.ScanFailure = errorCode;
            base.OnScanFailed(errorCode);
        }

        public override void OnBatchScanResults(IList<ScanResult> results)
        {
            this.ScanResults = results;
            base.OnBatchScanResults(results);
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {

                switch (name)
                {
                    case "ScanResults":
                        handler(this.ScanResults, new PropertyChangedEventArgs(name));
                        break;
                    case "ScanFailure":
                        handler(this.ScanFailure, new PropertyChangedEventArgs(name));
                        break;
                }

            }
        }

    }

    public class BLEDiscoveryService : INotifyPropertyChanged, IBLEDiscoveryService
    {
        private BluetoothManager blueToothManager = null;
        private BluetoothAdapter blueToothAdapter = null;
        private BluetoothLeScanner blueToothLeScanner = null;
        private BLECallback bleCallback = null;

        public event PropertyChangedEventHandler PropertyChanged;

        public ScanFailure ScanFailure
        {

            get
            {
                return this.bleCallback.ScanFailure;
            }

        }

        public IList<ScanResult> ScanResults
        {
            get
            {
                return this.bleCallback.ScanResults;
            }
        }

        public BLEDiscoveryService()
        {
            this.blueToothManager = (BluetoothManager)Application.Context.GetSystemService(Application.BluetoothService);
            this.blueToothAdapter = blueToothManager.Adapter ?? BluetoothAdapter.DefaultAdapter;
            this.blueToothLeScanner = this.blueToothAdapter.BluetoothLeScanner;
            this.bleCallback = new BLECallback();
            this.bleCallback.PropertyChanged += BleCallback_PropertyChanged;
        }

        public void StartScan()
        {
            this.blueToothLeScanner.StartScan(this.bleCallback);
        }

        public void StopScan()
        {
            this.blueToothLeScanner.StopScan(this.bleCallback);
        }

        private void BleCallback_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(sender, e);
            }
        }

    }
}
