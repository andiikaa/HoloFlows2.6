using UnityEngine;

namespace HoloFlows
{
    //REMOVED Tmds.MDns because of compile errors while deploying
    /// <summary>
    /// IMPORTANT: IT IS POSSIBLE THAT NETWORK DISCOVERY WILL CRASH UNITY. Disable or debug the following method:
    /// CheckNetworkInterfaceStatuses in <see cref="ServiceBrowser"/>
    /// </summary>
    public class HoloFlowDiscovery : MonoBehaviour
    {
        public const string proteusMdnsName = "_proteus._tcp";
        public const string openhabMdnsName = "_openhab-server._tcp";

        public void Start()
        {
            string[] serviceTypes = { proteusMdnsName, openhabMdnsName };

            //ServiceBrowser serviceBrowser = new ServiceBrowser();
            //serviceBrowser.ServiceAdded += onServiceAdded;
            //serviceBrowser.ServiceRemoved += onServiceRemoved;
            //serviceBrowser.ServiceChanged += onServiceChanged;
            //serviceBrowser.StartBrowse(serviceTypes);
        }

        //static void onServiceChanged(object sender, ServiceAnnouncementEventArgs e)
        //{
        //    //printService('~', e.Announcement);
        //}

        //static void onServiceRemoved(object sender, ServiceAnnouncementEventArgs e)
        //{
        //    //printService('-', e.Announcement);
        //}

        //static void onServiceAdded(object sender, ServiceAnnouncementEventArgs e)
        //{
        //    printService('+', e.Announcement);
        //    HandleService(e.Announcement);
        //}

        //static void printService(char startChar, ServiceAnnouncement service)
        //{
        //    Debug.LogFormat("{2} Found: {0} on {1}", service.Type, string.Join(", ", service.Addresses), startChar);
        //}

        //static void HandleService(ServiceAnnouncement service)
        //{
        //    if (service.Addresses.Count > 1)
        //    {
        //        Debug.LogWarningFormat("more than on addresses found for service '{0}'. Using first one", service.Type);
        //    }

        //    if (service.Addresses.Count == 0)
        //    {
        //        Debug.LogWarningFormat("no address for service '{0}' included.", service.Type);
        //        return;
        //    }

        //    if (proteusMdnsName == service.Type) { Settings.PROTEUS_BASE_URI = service.Addresses[0].ToString() + ":" + service.Port; }
        //    if (openhabMdnsName == service.Type) { Settings.OPENHAB_URI = service.Addresses[0].ToString() + ":" + service.Port; }
        //}
    }
}
