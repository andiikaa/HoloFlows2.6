using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;
using UnityEngine;



public class TapToPlaceParent : MonoBehaviour, IInputClickHandler, IFocusable
{
    public string SavedAnchorFriendlyName = "SavedAnchorFriendlyName";


    private bool placing = false;
    private bool placingMode = true;

    public void AllowPlacing()
    {
        placingMode = true;
    }

    public void DisallowPlacing()
    {
        placingMode = false;
    }
    void Start()
    {
        //SpatialMapping.Instance.DrawVisualMeshes = false;

        // ***** remove comment syntax ****** //
        if (WorldAnchorManager.Instance == null)
        {
            Debug.LogError("This script expects that you have a WorldAnchorManager component in your scene.");
        }

        if (WorldAnchorManager.Instance != null)
        {
            if (!placing)
            {
                WorldAnchorManager.Instance.AttachAnchor(transform.parent.gameObject, SavedAnchorFriendlyName);
            }
        }
    }

    // Called by GazeGestureManager when the user performs a Select gesture
    public void OnInputClicked(InputClickedEventData eventData)
    {
        // On each Select gesture, toggle whether the user is in placing mode.
        if (placingMode)
        {
            placing = !placing;

            // ***** remove comment syntax ****** //
            if (placing)
            {
                WorldAnchorManager.Instance.RemoveAnchor(transform.parent.gameObject);
            }
            else
            {
                WorldAnchorManager.Instance.AttachAnchor(transform.parent.gameObject, SavedAnchorFriendlyName);
            }
        }
        //    placing = !placing;

        //// ***** remove comment syntax ****** //
        //if (placing)
        //{
        //    WorldAnchorManager.Instance.RemoveAnchor(this.transform.parent.gameObject);
        //}
        //else
        //{
        //    WorldAnchorManager.Instance.AttachAnchor(this.transform.parent.gameObject, SavedAnchorFriendlyName);
        //}



        // If the user is in placing mode, display the spatial mapping mesh.
        /*if (placing)
        {
            SpatialMapping.Instance.DrawVisualMeshes = true;
        }
        // If the user is not in placing mode, hide the spatial mapping mesh.
        else
        {
            SpatialMapping.Instance.DrawVisualMeshes = false;
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        // If the user is in placing mode,
        // update the placement to match the user's gaze.

        if (placing)
        {
            // Do a raycast into the world that will only hit the Spatial Mapping mesh.
            var headPosition = Camera.main.transform.position;
            var gazeDirection = Camera.main.transform.forward;

            this.transform.parent.position = headPosition + 2 * gazeDirection;

            Quaternion toQuat = Camera.main.transform.localRotation;
            toQuat.x = 0;
            toQuat.z = 0;
            this.transform.parent.rotation = toQuat;

            //RaycastHit hitInfo;
            //if (Physics.Raycast(headPosition, gazeDirection, out hitInfo,
            //    30.0f, SpatialMappingManager.Instance.LayerMask))
            //{
            //    // Move this object's parent object to
            //    // where the raycast hit the Spatial Mapping mesh.
            //    this.transform.parent.position = hitInfo.point;

            //    // Rotate this object's parent object to face the user.
            //    Quaternion toQuat = Camera.main.transform.localRotation;
            //    toQuat.x = 0;
            //    toQuat.z = 0;
            //    this.transform.parent.rotation = toQuat;
            //}
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        //this device will be the device which is hit by 'other'

        var thisRootParent = GetRootParent(gameObject);
        var otherRootParent = GetRootParent(other.gameObject);

        //if the object hit itself somehow
        if (thisRootParent == otherRootParent)
        {
            return;
        }

        Debug.Log(otherRootParent.name + " hit " + thisRootParent.name);

        //start to merge 2 devices
        var mergedDevice = DeviceSpawner.Instance.MergeBasicDevices(otherRootParent, thisRootParent);
        //TODO andere devices ausblenden und noch nicht zerstören

        Destroy(otherRootParent);
        Destroy(thisRootParent);
    }

    private GameObject GetRootParent(GameObject go)
    {
        if (go.transform.parent != null)
        {
            return GetRootParent(go.transform.parent.gameObject);
        }
        return go;
    }

    public void OnTriggerExit(Collider other)
    {
        //Debug.Log("Trigger left");
    }

    public void OnFocusEnter()
    {
        transform.GetChild(0).localScale = new Vector3(5, 5, 5);
    }

    public void OnFocusExit()
    {
        transform.GetChild(0).localScale = Vector3.zero;
    }
}

