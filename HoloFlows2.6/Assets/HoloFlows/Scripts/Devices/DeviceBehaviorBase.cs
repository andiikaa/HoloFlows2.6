using HoloFlows.ButtonScripts;
using HoloFlows.Manager;
using HoloFlows.Model;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace HoloFlows.Devices
{
    public abstract class DeviceBehaviorBase : MonoBehaviour, IManagedObject
    {
        public bool IsBasicDevice { get { return GetDeviceType() == DeviceType.BASIC; } }
        public bool IsTwoPieceDevice { get { return GetDeviceType() == DeviceType.TWO_PIECE; } }
        public bool IsThreePieceDevice { get { return GetDeviceType() == DeviceType.THREE_PIECE; } }
        public bool IsMultiDevice { get { return GetDeviceType() == DeviceType.MULTI; } }

        public string DeviceId { get; set; }


        /// <summary>
        /// The number of frames between every polling request.
        /// </summary>
        protected int PollingFrameDelay { get; private set; } = Settings.POLLING_DELAY_FRAMES;
        private int frameNumber = 0;

        /// <summary>
        /// Gets the current <see cref=" DeviceType"/>
        /// </summary>
        protected abstract DeviceType GetDeviceType();

        /// <summary>
        /// Called, when the device should update its device states (if there are any).
        /// The Method is called every <see cref="PollingFrameDelay"/> frames.
        /// </summary>
        protected abstract void UpdateDeviceStates();


        /// <summary>
        /// Call this method every frame! It will invoke the <see cref="UpdateDeviceStates"/> method every <see cref="PollingFrameDelay"/> frames.
        /// </summary>
        protected void UpdateDeviceStatesInternal()
        {
            frameNumber++;
            if (frameNumber > PollingFrameDelay)
            {
                frameNumber = 0;
                UpdateDeviceStates();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupSource">A group which contains the buttons</param>
        /// <param name="groupTarget">A group which should contain the copied buttons</param>
        private void CopyButtonsWithBehaviors(Transform groupSource, Transform groupTarget)
        {
            if (groupSource.childCount > 1)
            {
                for (int i = 1; i < groupSource.childCount; i++)
                {
                    Transform child = groupSource.GetChild(i);

                    //TODO a better check would be nice
                    if (child.gameObject.name.Contains("button") || child.gameObject.name.Contains("Button"))
                    {
                        CreateNewButtons(groupTarget, child);
                    }
                }
            }
        }

        /// <summary>
        /// Copies the billboard groups with the whole contents and behaviors
        /// </summary>
        /// <param name="groupSource"></param>
        /// <param name="groupTarget"></param>
        protected void CopyBillboardGroup(Transform groupSource, Transform groupTarget)
        {
            CopyDeviceHeaderData(groupSource.Find("Canvas"), groupTarget.Find("Canvas"));
            CopyButtonsWithBehaviors(groupSource, groupTarget);
        }

        protected static string GetValuePrefix(UnitOfMeasure unitOfMeasure)
        {
            if (unitOfMeasure == null || string.IsNullOrEmpty(unitOfMeasure.PrefixSymbol))
            {
                return string.Empty;
            }
            return unitOfMeasure.PrefixSymbol;
        }

        /// <summary>
        /// Gets the grouped device functionalities, orderd by group box name.
        /// </summary>
        protected IEnumerable<IGrouping<string, DeviceFunctionality>> GetGroupedFunctionalities(List<DeviceFunctionality> funcs)
        {
            if (funcs == null) { return null; }
            return from func in funcs
                   group func by func.GroupBox.Uid into newGroup
                   orderby newGroup.Key
                   select newGroup;
        }

        /// <summary>
        /// gets the grouped device states, orderd by group box name.
        /// </summary>
        protected IEnumerable<IGrouping<string, DeviceState>> GetGroupedStates(List<DeviceState> states)
        {
            if (states == null) { return null; }
            return from state in states
                   group state by state.GroupBox.Uid into newGroup
                   orderby newGroup.Key
                   select newGroup;
        }

        private void CopyDeviceHeaderData(Transform sourceCanvas, Transform targetCanvas)
        {
            //TODO image source
            SpriteRenderer sourceImage = sourceCanvas.Find("Image").GetComponent<SpriteRenderer>();
            var sDesc = sourceCanvas.Find("Description").GetComponent<Text>();
            var sValue = sourceCanvas.Find("Value").GetComponent<Text>();

            SpriteRenderer targetImage = targetCanvas.Find("Image").GetComponent<SpriteRenderer>();
            var tDesc = targetCanvas.Find("Description").GetComponent<Text>();
            var tValue = targetCanvas.Find("Value").GetComponent<Text>();

            targetImage.sprite = sourceImage.sprite;
            tDesc.text = sDesc.text;
            tValue.text = sValue.text;
        }

        private void CreateNewButtons(Transform parent, Transform sourceChild)
        {
            GameObject newChild = Instantiate(sourceChild.gameObject);

            DefaultDeviceButtonBehavior[] sourceBehaviors = sourceChild.GetComponentsInChildren<DefaultDeviceButtonBehavior>();
            DefaultDeviceButtonBehavior[] targetBehaviors = newChild.GetComponentsInChildren<DefaultDeviceButtonBehavior>();

            if (sourceBehaviors.Length != targetBehaviors.Length)
            {
                Debug.LogError("defaultbuttonbehaviors count does not match for copied instance");
                return;
            }

            for (int i = 0; i < sourceBehaviors.Length; i++)
            {
                targetBehaviors[i].DeviceId = sourceBehaviors[i].DeviceId;
                targetBehaviors[i].RealCommandName = sourceBehaviors[i].RealCommandName;
                targetBehaviors[i].CommandDisplayName = sourceBehaviors[i].CommandDisplayName;
            }

            newChild.transform.SetParent(parent, false);
        }

        #region managed object

        protected void RegisterToHoloFlowSceneManager()
        {
            HoloFlowSceneManager.Instance.RegisterObject(this);
        }

        protected void UnregisterFromHoloFlowSceneManager()
        {
            //check is neccesary or at app exit a np is thrown
            if (HoloFlowSceneManager.IsInitialized) { HoloFlowSceneManager.Instance.UnregisterObject(this); }
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void EnablePlacingBox(bool enable)
        {
            TapToPlaceParent tapToPlace = gameObject.GetComponentInChildren<TapToPlaceParent>();
            if (tapToPlace == null)
            {
                Debug.LogErrorFormat("current gameobject '{0}' does not support the enabling/disabling of the placing box", gameObject.name);
                return;
            }

            if (enable) { tapToPlace.AllowPlacing(); }
            else { tapToPlace.DisallowPlacing(); }
        }

        public void StartPlacing()
        {
            TapToPlaceParent tapToPlace = gameObject.GetComponentInChildren<TapToPlaceParent>();
            if (tapToPlace == null)
            {
                Debug.LogErrorFormat("current gameobject '{0}' does not support the enabling/disabling of the placing box", gameObject.name);
                return;
            }
            //ensure placing is allowed
            tapToPlace.AllowPlacing();
            tapToPlace.TogglePlacing();
        }

        #endregion
    }

    public enum DeviceType { BASIC, TWO_PIECE, THREE_PIECE, MULTI }

    internal class DeviceStateHolder
    {
        internal Transform transform;
        internal DeviceState deviceState;
    }

}
