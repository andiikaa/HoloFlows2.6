using Tmds.MDns;
using UnityEngine;

namespace HoloFlows
{
    //FIXME: will not work within unity
    //fails at runtime
    public class HoloFlowDiscovery : MonoBehaviour
    {
        public const string proteusMdnsName = "_proteus._tcp";
        public const string openhabMdnsName = "_openhab-server._tcp";

        public void Start()
        {
            string[] serviceTypes = { proteusMdnsName, openhabMdnsName };

            ServiceBrowser serviceBrowser = new ServiceBrowser();
            serviceBrowser.ServiceAdded += onServiceAdded;
            serviceBrowser.ServiceRemoved += onServiceRemoved;
            serviceBrowser.ServiceChanged += onServiceChanged;
            serviceBrowser.StartBrowse(serviceTypes);
        }

        static void onServiceChanged(object sender, ServiceAnnouncementEventArgs e)
        {
            printService('~', e.Announcement);
        }

        static void onServiceRemoved(object sender, ServiceAnnouncementEventArgs e)
        {
            printService('-', e.Announcement);
        }

        static void onServiceAdded(object sender, ServiceAnnouncementEventArgs e)
        {
            printService('+', e.Announcement);
        }

        static void printService(char startChar, ServiceAnnouncement service)
        {
            Debug.LogFormat("{2} Found: {0} on {1}", service.Type, string.Join(", ", service.Addresses), startChar);
        }
    }
}
