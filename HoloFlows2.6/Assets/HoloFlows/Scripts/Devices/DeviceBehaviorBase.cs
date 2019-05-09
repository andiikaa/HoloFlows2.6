using HoloFlows.ButtonScripts;
using HoloFlows.Manager;
using HoloFlows.Model;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HoloFlows.Devices
{
    public abstract class DeviceBehaviorBase : MonoBehaviour, IManagedObject
    {
        protected const string TYPE_ON_COMMAND = "dogont:OnCommand";
        protected const string TYPE_OFF_COMMAND = "dogont:OffCommand";
        protected const string TYPE_DOWN_COMMAND = "dogont:DownCommand";
        protected const string TYPE_UP_COMMAND = "dogont:UpCommand";
        protected const string FUNC_TYPE_COLOR_CONTROL = "dogont:ColorControlFunctionality";

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
        public abstract DeviceType GetDeviceType();

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

        protected void AddColorButtons(DeviceFunctionality colorFunc, Transform target)
        {
            GameObject colorPickerPrefab = PrefabHolder.Instance.devices.poorColorPicker;
            GameObject colorPicker = Instantiate(colorPickerPrefab);
            colorPicker.transform.SetParent(target, false);
            colorPicker.SetActive(true);
            DefaultColorPickerButton btn = colorPicker.GetComponent<DefaultColorPickerButton>();
            btn.SetFunctionality(colorFunc);
        }

        protected GameObject SpawnButton(DeviceFunctionality functionality, DeviceCommand command, Transform parent)
        {
            GameObject buttonPrefab = GetTypeSpecificButtonPrefab(command);
            GameObject buttonInstance = Instantiate(buttonPrefab);
            buttonInstance.transform.SetParent(parent, false);
            DefaultDeviceButtonBehavior behavior = buttonInstance.GetComponent<DefaultDeviceButtonBehavior>();
            behavior.CommandDisplayName = command.Name;
            behavior.RealCommandName = command.RealCommandName;
            behavior.DeviceId = functionality.ItemId;
            return buttonInstance;
        }

        private GameObject GetTypeSpecificButtonPrefab(DeviceCommand cmd)
        {
            //TODO add button for color changing
            if (TYPE_DOWN_COMMAND.Equals(cmd.CommandType)) return PrefabHolder.Instance.devices.downButton;
            if (TYPE_UP_COMMAND.Equals(cmd.CommandType)) return PrefabHolder.Instance.devices.upButton;
            return PrefabHolder.Instance.devices.defaultDeviceButton;
        }

        protected void AddOnOffUpDownCombination(Transform layoutGroup, List<DeviceFunctionality> onOffUpDownFunc)
        {
            DeviceFunctionality onCommandFunc = onOffUpDownFunc
                .Where(f => f.Commands.Any(c => TYPE_ON_COMMAND == c.CommandType))
                .First();
            DeviceFunctionality offCommandFunc = onOffUpDownFunc
                .Where(f => f.Commands.Any(c => TYPE_OFF_COMMAND == c.CommandType))
                .First();
            DeviceFunctionality upCommandFunc = onOffUpDownFunc
                .Where(f => f.Commands.Any(c => TYPE_UP_COMMAND == c.CommandType))
                .First();
            DeviceFunctionality downCommandFunc = onOffUpDownFunc
                .Where(f => f.Commands.Any(c => TYPE_DOWN_COMMAND == c.CommandType))
                .First();

            //buttons are visible in this order from top to bottom
            GameObject btnObject = Instantiate(PrefabHolder.Instance.devices.onOffUpDownButton);
            OnOffUpDownButton btnScript = btnObject.GetComponent<OnOffUpDownButton>();

            btnScript.SetDownButtonData(downCommandFunc.ItemId, downCommandFunc.Commands.First(c => TYPE_DOWN_COMMAND == c.CommandType).RealCommandName);
            btnScript.SetUpButtonData(upCommandFunc.ItemId, upCommandFunc.Commands.First(c => TYPE_UP_COMMAND == c.CommandType).RealCommandName);
            btnScript.SetOnButtonData(onCommandFunc.ItemId, onCommandFunc.Commands.First(c => TYPE_ON_COMMAND == c.CommandType).RealCommandName);
            btnScript.SetOffButtonData(offCommandFunc.ItemId, offCommandFunc.Commands.First(c => TYPE_OFF_COMMAND == c.CommandType).RealCommandName);

            btnObject.transform.SetParent(layoutGroup, false);
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

    internal class GroupBoxHolder
    {
        internal string groupBoxId;
        internal Transform transform;
        internal DeviceState deviceState;
    }

}
