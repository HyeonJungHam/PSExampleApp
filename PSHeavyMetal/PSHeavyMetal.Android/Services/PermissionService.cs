﻿using Android.Bluetooth;
using Android.Content;
using PalmSens.Core.Simplified.XF.Application.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PSHeavyMetal.Droid.Services
{
    public class BluetoothPermission : Xamarin.Essentials.Permissions.BasePlatformPermission
    {
        public override (string androidPermission, bool isRuntime)[] RequiredPermissions => new List<(string androidPermission, bool isRuntime)>
        {
            (Android.Manifest.Permission.BluetoothScan, true),
            (Android.Manifest.Permission.BluetoothConnect, true)
        }.ToArray();
    }

    public class PermissionService : IPermissionService
    {
        public bool CheckBlueToothEnabled()
        {
            var bluetoothAdapter = BluetoothAdapter.DefaultAdapter;
            return bluetoothAdapter.IsEnabled;
        }

        public void OpenBluetoothSettings()
        {
            var intentOpenBluetoothSettings = new Intent();
            intentOpenBluetoothSettings.SetAction(Android.Provider.Settings.ActionBluetoothSettings);
            intentOpenBluetoothSettings.SetFlags(ActivityFlags.NewTask);
            Android.App.Application.Context.StartActivity(intentOpenBluetoothSettings);
        }

        public Task<PermissionStatus> RequestBluetoothPermission()
        {
            return RequestPermission<BluetoothPermission>();
        }

        public async Task<PermissionStatus> RequestPermission<T>() where T : Permissions.BasePermission, new()
        {
            var status = await Permissions.CheckStatusAsync<T>();

            if (status == PermissionStatus.Granted)
                return status;

            //if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
            //{
            //    // Prompt the user to turn on in settings
            //    // On iOS once a permission has been denied it may not be requested again from the application
            //    return status;
            //}

            if (Permissions.ShouldShowRationale<T>())
            {
                // Prompt the user with additional information as to why the permission is needed
            }

            status = await Permissions.RequestAsync<T>();

            return status;
        }
    }
}