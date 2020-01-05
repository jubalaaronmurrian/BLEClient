using System;
using System.Collections.Generic;
using System.ComponentModel;
using Android.App;
using Android.Bluetooth;
using Android.Bluetooth.LE;
using Android.Runtime;

namespace BLE
{

    public class BLECallback : ScanCallback,INotifyPropertyChanged
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
                OnPropertyChanged("ScanFailure");
                this.scanFailure = value;
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
                OnPropertyChanged("ScanResults");
                this.scanResults = value;
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
            if(handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

    }

    public class BLEDiscoveryService : INotifyPropertyChanged
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
            this.blueToothManager = (BluetoothManager)Application.BluetoothService;
            this.blueToothAdapter = blueToothManager.Adapter;
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
