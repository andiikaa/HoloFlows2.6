using UnityEngine;

public abstract class DeviceBehaviorBase : MonoBehaviour
{
    protected enum DeviceType { BASIC, TWO_WAY, THREE_WAY, MULTI }

    public bool IsBasicDevice { get { return GetDeviceType() == DeviceType.BASIC; } }
    public bool IsTwoWayDevice { get { return GetDeviceType() == DeviceType.TWO_WAY; } }
    public bool IsThreeWayDevice { get { return GetDeviceType() == DeviceType.THREE_WAY; } }
    public bool IsMultiDevice { get { return GetDeviceType() == DeviceType.MULTI; } }

    protected abstract DeviceType GetDeviceType();


}
