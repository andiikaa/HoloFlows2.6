using HoloToolkit.Unity;
using UnityEngine;
#if WINDOWS_UWP
using Windows.Devices.Enumeration;
#endif

namespace HoloFlows.Manager
{
    /// <summary>
    /// Find remote services via dns
    /// 
    /// https://docs.microsoft.com/de-de/windows/uwp/devices-sensors/enumerate-devices-over-a-network
    /// </summary>
    public class RemoteServiceManager : Singleton<RemoteServiceManager>
    {
#if WINDOWS_UWP
    DeviceWatcher watchter;
#endif

        public string OpenHabAddress { get; private set; }
        public string ProteusAddress { get; private set; }

        private const string OPENHAB_NAME = "_openhab";
        private const string PROTEUS_NAME = "_proteus";

        private const string allDevicesWithIp = "System.Devices.IpAddress:<>[]";

        // Use this for initialization
        void Start()
        {
#if WINDOWS_UWP

        //AQS
        //System.Devices.IpAddress:<>[]
        //

        watchter = DeviceInformation.CreateWatcher(allDevicesWithIp, null, DeviceInformationKind.AssociationEndpoint);
        watchter.Added += Watchter_Added;
        watchter.Start();
#else
            Debug.LogError("ServiceDiscovery not supported on the current plattform");
#endif
        }

#if WINDOWS_UWP
    private void Watchter_Added(DeviceWatcher sender, DeviceInformation args)
    {
        Debug.Log(string.Format("ID: {0}, Name: {1}", args.Id, args.Name));
        if (args.Properties != null)
        {
            Debug.Log(string.Format("Properties:"));
            foreach (string key in args.Properties.Keys)
            {
                Debug.Log(string.Format("  Key: {0} Value: {1}", key, args.Properties[key]));
            }
        }
    }
#endif

    }
}
