using UnityEngine;
using UnityEngine.Networking;

public class OverriddenNetworkDiscovery : NetworkDiscovery
{
    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        Debug.Log(string.Format("Address: {0} Data: {1}", fromAddress, data));
    }

    public void Awake()
    {
        Initialize();
        StartAsClient();
    }
}
