using UnityEngine;

namespace HoloFlows
{
    public class Settings
    {

        private static string openhabUri = "http://192.168.1.144:8080";
        private static string proteusBaseUri = "http://192.168.1.144:8082";

        public static string OPENHAB_URI
        {
            get { return openhabUri; }
            set
            {
                if (openhabUri == value) return;
                Debug.LogFormat("openhab uri changed from '{0}' to '{1}'", openhabUri, value);
                openhabUri = value;
            }
        }

        public static string PROTEUS_BASE_URI
        {
            get { return proteusBaseUri; }
            set
            {
                if (proteusBaseUri == value) return;
                Debug.LogFormat("proteus uri changed from '{0}' to '{1}'", proteusBaseUri, value);
                proteusBaseUri = value;
            }
        }


        public const string UNKNOWN_STATE = "-";
        public const int POLLING_DELAY_FRAMES = 11;
    }
}
