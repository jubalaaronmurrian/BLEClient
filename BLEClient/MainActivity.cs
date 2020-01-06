using Android.App;
using Android.Bluetooth.LE;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using BLE;
using System.Collections.Generic;

namespace BLEClient
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, BottomNavigationView.IOnNavigationItemSelectedListener
    {
        TextView textMessage;
        ListView bleDevicesListView;
        Button startDiscoveryButton;
        Button stopDiscoveryButton;
        TextView failureTextView;
        IBLEDiscoveryService bleDiscoveryService;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            textMessage = FindViewById<TextView>(Resource.Id.message);
            BottomNavigationView navigation = FindViewById<BottomNavigationView>(Resource.Id.navigation);
            navigation.SetOnNavigationItemSelectedListener(this);

            bleDevicesListView = FindViewById<ListView>(Resource.Id.bleDevicesListView);
            startDiscoveryButton = FindViewById<Button>(Resource.Id.startDiscoveryButton);
            startDiscoveryButton.Click += StartDiscoveryButton_Click;
            stopDiscoveryButton = FindViewById<Button>(Resource.Id.stopDiscoveryButton);
            stopDiscoveryButton.Click += StopDiscoveryButton_Click;
            failureTextView = FindViewById<TextView>(Resource.Id.failureTextView);

            this.bleDiscoveryService = new BLEDiscoveryService();
            this.bleDiscoveryService.PropertyChanged += BleDiscoveryService_PropertyChanged;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        public bool OnNavigationItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.navigation_home:
                    textMessage.SetText(Resource.String.title_home);
                    return true;
                case Resource.Id.navigation_dashboard:
                    textMessage.SetText(Resource.String.title_dashboard);
                    return true;
                case Resource.Id.navigation_notifications:
                    textMessage.SetText(Resource.String.title_notifications);
                    return true;
            }
            return false;
        }

        private void StartDiscoveryButton_Click(object sender, System.EventArgs e)
        {
            this.bleDiscoveryService.StartScan();
        }

        private void StopDiscoveryButton_Click(object sender, System.EventArgs e)
        {
            this.bleDiscoveryService.StopScan();
        }

        private void BleDiscoveryService_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

            switch (e.PropertyName)
            {
                case "ScanFailure":
                    ScanFailure scanFailure = (ScanFailure)sender;
                    this.failureTextView.Text = scanFailure.ToString();
                    break;
                case "ScanResults":
                    IList<ScanResult> scanResults = sender as IList<ScanResult>;
                    if (scanResults != null)
                    {
                        IList<string> bleDevices = new List<string>();
                        foreach (ScanResult scanResult in scanResults)
                        {
                            bleDevices.Add(scanResult.Device.Name);
                        }

                        this.bleDevicesListView.Adapter = new ArrayAdapter<string>(Application.ApplicationContext, Resource.Id.bleDevicesListView, bleDevices);

                    }
                    break;
            }
        }
    }
}

