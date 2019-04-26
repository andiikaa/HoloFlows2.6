using HoloFlows.Devices;
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

        /// <summary>
        /// This is the id, which is used to find/save the postion of the device control.
        /// </summary>
        public string SavedAnchorFriendlyName { get; set; } = "SavedAnchorFriendlyName";

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
            DoPlacing();
            DoMerging();

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
            GameObject mergedDevice = DeviceSpawner.Instance.MergeBasicDevices(otherRootParent, thisRootParent);
            if (mergedDevice != null)
            {
                Destroy(otherRootParent);
                Destroy(thisRootParent);

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

