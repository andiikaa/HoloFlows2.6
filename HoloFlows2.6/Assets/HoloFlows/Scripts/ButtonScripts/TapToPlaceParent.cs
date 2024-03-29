﻿using HoloFlows.Devices;
using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;
using UnityEngine;

namespace HoloFlows.ButtonScripts
{

    public class TapToPlaceParent : MonoBehaviour, IInputClickHandler, IFocusable
    {
        //colors when placing boxes are colliding
        private static readonly Color BLOBB_COLOR_DEFAULT = new Color(0f, 0.6f, 0.877f, 1f);
        private static readonly Color BLOBB_COLOR_HIT = Color.red;

        private string deviceId;

        /// <summary>
        /// This is the id, which is used to find/save the postion of the device control.
        /// </summary>
        public string DeviceId
        {
            get { return deviceId; }
            set
            {
                if (deviceId != null && WorldAnchorManager.IsInitialized)
                {
                    WorldAnchorManager.Instance.RemoveAnchor(deviceId);
                }
                deviceId = value;
                SetInitialAnchor();
            }
        }

        //to check if placing is enabled
        private bool placing = false;
        private bool placingMode = true;

        //to check if two boxes are hitting each other
        private bool triggerEntered = false;
        private int triggerEnteredCount = 0;

        //last hitted objects
        private GameObject thisRootParent = null;
        private GameObject otherRootParent = null;

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
            SetInitialAnchor();
        }

        // Called by GazeGestureManager when the user performs a Select gesture
        public void OnInputClicked(InputClickedEventData eventData)
        {
            TogglePlacing();
        }

        /// <summary>
        /// Starts or ends the placing depending on the current state (placingMode).
        /// </summary>
        public void TogglePlacing()
        {
            if (placingMode)
            {
                placing = !placing;
                if (placing)
                {
                    Debug.LogFormat("Remove anchor for '{0}'", transform.parent.gameObject.name);
                    WorldAnchorManager.Instance.RemoveAnchor(transform.parent.gameObject);
                }
                else
                {
                    Debug.LogFormat("Updating anchor with id '{0}'", DeviceId);
                    if (DeviceId == null)
                    {
                        Debug.LogError("no device id set. no anchor is set");
                    }

                    if (DeviceId != null)
                    {
                        WorldAnchorManager.Instance.AttachAnchor(transform.parent.gameObject, DeviceId);
                    }

                    // TODO reload the position via unity? with the load method? 
                    // https://docs.unity3d.com/560/Documentation/ScriptReference/VR.WSA.Persistence.WorldAnchorStore.html
                    // OR: share anchor via https://docs.unity3d.com/Manual/windowsholographic-anchorsharing.html
                    // the sharing could maybe be used for persistence in database?


                    //TODO we cant share the position in this way
                    //var request = new UpdateWorldPositionRequest("", DeviceUid);
                    //SerializableVector3 sv3 = transform.parent.position;

                    //TODO update request to openhab
                    //StartCoroutine(request.ExecuteRequest(sv3.ToJson()));
                    //Debug.LogFormat("Position: {0}", sv3.ToString());
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            // If the user is in placing mode,
            // update the placement to match the user's gaze.
            DoPlacing();
            DoMerging();
        }

        public void RemoveAnchor()
        {
            if (WorldAnchorManager.IsInitialized) { }
        }

        private void DoMerging()
        {
            if (triggerEntered)
            {
                triggerEnteredCount++;
                if (triggerEnteredCount > 100)
                {
                    triggerEnteredCount = 0;
                    if (thisRootParent != null && otherRootParent != null)
                    {
                        MergeDevices();
                    }
                }
            }
        }

        private void MergeDevices()
        {
            GameObject mergedDevice = DeviceSpecificMerge();
            if (mergedDevice != null)
            {
                Destroy(otherRootParent);
                Destroy(thisRootParent);

                //TODO merge request to openhab

                TapToPlaceParent tapToPlaceOfMerged = mergedDevice.GetComponentInChildren<TapToPlaceParent>(true);
                if (tapToPlaceOfMerged == null)
                {
                    Debug.LogError("TapToPlace Component not found for merged device!");
                }
                if (tapToPlaceOfMerged != null)
                {
                    //simulate tap to trigger the placing mode
                    tapToPlaceOfMerged.OnFocusEnter();
                    tapToPlaceOfMerged.OnInputClicked(null);
                }
            }
        }

        private GameObject DeviceSpecificMerge()
        {
            DeviceBehaviorBase otherRootDevice = otherRootParent.GetComponent<DeviceBehaviorBase>();
            DeviceBehaviorBase thisRootDevice = thisRootParent.GetComponent<DeviceBehaviorBase>();

            if (otherRootDevice == null || thisRootDevice == null)
            {
                Debug.LogError("could not merge. device behavior not found");
                return null;
            }

            switch (otherRootDevice.GetDeviceType())
            {
                case Devices.DeviceType.BASIC:
                    if (thisRootDevice.IsBasicDevice) return DeviceSpawner.Instance.MergeBasicDevices((BasicDevice)otherRootDevice, (BasicDevice)thisRootDevice);
                    if (thisRootDevice.IsTwoPieceDevice) return DeviceSpawner.Instance.MergeBasicAndTwoPiece((BasicDevice)otherRootDevice, (TwoPieceDevice)thisRootDevice);
                    return null;
                case Devices.DeviceType.TWO_PIECE:
                    if (thisRootDevice.IsBasicDevice) return DeviceSpawner.Instance.MergeBasicAndTwoPiece((BasicDevice)thisRootDevice, (TwoPieceDevice)otherRootDevice);
                    return null;
                default:
                    return null;
            }
        }

        private void DoPlacing()
        {
            if (!placing)
            {
                return;
            }

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

        private void SetInitialAnchor()
        {
            if (!placing && DeviceId != null && WorldAnchorManager.IsInitialized)
            {
                WorldAnchorManager.Instance.AttachAnchor(transform.parent.gameObject, DeviceId);
            }
            else
            {
                Debug.LogWarning("cant attach anchor. anchormanager is or device id is null or placing mode is active");
            }
        }

        public void OnTriggerEnter(Collider other)
        {
            //this device will be the device which is hit by 'other'
            GameObject tmpThisRootParent = GetRootParent(gameObject);
            GameObject tmpOtherRootParent = GetRootParent(other.gameObject);

            //if the object hit itself somehow
            if (tmpThisRootParent == tmpOtherRootParent)
            {
                return;
            }

            thisRootParent = tmpThisRootParent;
            otherRootParent = tmpOtherRootParent;

            Debug.Log(otherRootParent.name + " hit " + thisRootParent.name);

            ChangePlacingBoxColor(tmpThisRootParent, BLOBB_COLOR_HIT);

            triggerEntered = true;
        }

        private GameObject GetRootParent(GameObject go)
        {
            if (go.transform.parent != null)
            {
                return GetRootParent(go.transform.parent.gameObject);
            }
            return go;
        }

        private void ChangePlacingBoxColor(GameObject deviceObject, Color color)
        {
            if (deviceObject == null) { return; }

            Transform child = deviceObject.transform.Find("Cube/Box-Visual");
            if (child == null)
            {
                Debug.LogErrorFormat("Box-Visual not found in '{0}'", deviceObject.name);
                return;
            }

            MeshRenderer mesh = child.gameObject.GetComponent<MeshRenderer>();
            if (mesh == null)
            {
                Debug.LogErrorFormat("Box-Visual has no mesh '{0}'", child.gameObject.name);
                return;
            }

            if (mesh.materials.Length == 0)
            {
                Debug.LogErrorFormat("Box-Visual has no mesh material '{0}'", child.gameObject.name);
                return;
            }

            mesh.materials[0].color = color;
        }

        public void OnTriggerExit(Collider other)
        {
            triggerEntered = false;
            triggerEnteredCount = 0;
            GameObject tmpThisRootParent = GetRootParent(gameObject);
            ChangePlacingBoxColor(tmpThisRootParent, BLOBB_COLOR_DEFAULT);
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
}

