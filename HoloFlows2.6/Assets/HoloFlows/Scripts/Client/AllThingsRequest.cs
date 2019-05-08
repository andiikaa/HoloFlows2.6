using HoloFlows.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace HoloFlows.Client
{
    /// <summary>
    /// Gets all things from openhab
    /// </summary>
    public class AllThingsRequest : SimpleGetRequestBase
    {
        private const string URI_TARGET = "rest/semantic/things";
        private readonly Action<List<DeviceInfo>> responseReadyAction;

        public AllThingsRequest(string requestURL, Action<List<DeviceInfo>> responseReadyAction = null) : base(requestURL, URI_TARGET)
        {
            this.responseReadyAction = responseReadyAction;
        }

        protected override void HandleResponse(UnityWebRequest www)
        {
            //TODO implement the web request in openhab
            List<DeviceInfo> infos = new List<DeviceInfo>();
            infos.Add(CreateTinkerforgeIRTemp());
            infos.Add(CreateHomematicDimmer());
            infos.Add(CreateTinkerforgeLuminance());
            infos.Add(CreateHue());
            responseReadyAction?.Invoke(infos);
        }

        //TODO remove this override
        new public IEnumerator ExecuteRequest()
        {
            HandleResponse(null);
            yield break;
        }

        #region static demo devices

        /// <summary>
        /// hard coded device information for ir temp bricklet
        /// </summary>
        private DeviceInfo CreateTinkerforgeIRTemp()
        {
            DeviceInfo tinkerforgeIrTemp = new DeviceInfo()
            {
                Uid = "tinkerforge_irTemp_1",
                DisplayName = "Tinkerforge IR Temp"
            };

            UnitOfMeasure degree = new UnitOfMeasure()
            {
                UnitName = "degree-Celsius",
                UnitType = "temperature",
                PrefixSymbol = "°C"
            };

            GroupBox boxIr = new GroupBox()
            {
                Uid = "box_ir_1",
                Name = "IR Temp",
                IconName = "temp"
            };

            GroupBox boxAmb = new GroupBox()
            {
                Uid = "box_amb_1",
                Name = "Ambiente Temp",
                IconName = "temp"
            };

            DeviceState irTempState = new DeviceState()
            {
                ItemId = "tinkerforge_irTemp_irTemp_1",
                Label = "Object Temp",
                UnitOfMeasure = degree,
                RealStateValue = "20",
                StateType = "dogont:TemperatureState",
                GroupBox = boxIr
            };

            DeviceState ambTempState = new DeviceState()
            {
                ItemId = "tinkerforge_irTemp_ambTemp_1",
                Label = "Ambiente Temp",
                UnitOfMeasure = degree,
                RealStateValue = "15",
                StateType = "dogont:TemperatureState",
                GroupBox = boxAmb
            };

            tinkerforgeIrTemp.States = new[] { irTempState, ambTempState };
            tinkerforgeIrTemp.GroupBoxes = new[] { boxAmb, boxIr };
            return tinkerforgeIrTemp;
        }

        private DeviceInfo CreateTinkerforgeLuminance()
        {
            DeviceInfo info = new DeviceInfo()
            {
                Uid = "tinkerforge_ambientLight_ambientLight_2",
                DisplayName = "Ambient Light Sensor"
            };

            GroupBox boxLum = new GroupBox()
            {
                Uid = "box_lum_1",
                Name = "Luminance",
                IconName = "light"
            };

            UnitOfMeasure lux = new UnitOfMeasure()
            {
                UnitName = "lux",
                UnitType = "light",
                PrefixSymbol = "Lux"
            };

            DeviceState ambienteLight = new DeviceState()
            {
                ItemId = "tinkerforge_ambientLight_ambientLight_2",
                Label = "Luminance",
                UnitOfMeasure = lux,
                GroupBox = boxLum,
                RealStateValue = "150"
            };

            info.States = new[] { ambienteLight };
            info.GroupBoxes = new[] { boxLum };
            return info;
        }

        //TODO create Hue stuff
        private DeviceInfo CreateHue()
        {
            DeviceInfo info = new DeviceInfo()
            {
                DisplayName = "Hue Bulb 1",
                Uid = "hue_bulb210_1"
            };

            GroupBox box = new GroupBox()
            {
                Uid = "box_hue_1",
                Name = "Hue Bulb 1",
                IconName = "color_light"
            };

            DeviceState state = new DeviceState()
            {
                ItemId = "hue_bulb210_light_1",
                Label = "State",
                RealStateValue = "OFF",
                GroupBox = box
            };

            DeviceCommand onCommand = new DeviceCommand()
            {
                Name = "DEFAULT_ON_COMMAND",
                RealCommandName = "ON",
                CommandType = "dogont:OnCommand"
            };

            DeviceCommand offCommand = new DeviceCommand()
            {
                Name = "DEFAULT_OFF_COMMAND",
                RealCommandName = "OFF",
                CommandType = "dogont:OffCommand"
            };

            DeviceCommand upCommand = new DeviceCommand()
            {
                Name = "DEFAULT_UP_COMMAND",
                RealCommandName = "INCREASE",
                CommandType = "dogont:UpCommand"
            };

            DeviceCommand downCommand = new DeviceCommand()
            {
                Name = "DEFAULT_DOWN_COMMAND",
                RealCommandName = "DECREASE",
                CommandType = "dogont:DownCommand"
            };

            DeviceFunctionality lightOnOff = new DeviceFunctionality()
            {
                FunctionalityType = "dogont:OnOffFunctionality",
                Commands = new[] { onCommand, offCommand },
                ItemId = "hue_bulb210_light_1",
                GroupBox = box
            };

            DeviceFunctionality lightDimm = new DeviceFunctionality()
            {
                FunctionalityType = "dogont:LevelControlFunctionality",
                Commands = new[] { upCommand, downCommand },
                ItemId = "hue_bulb210_dimmer_1",
                GroupBox = box
            };

            DeviceFunctionality color = new DeviceFunctionality()
            {
                FunctionalityType = "dogont:ColorControlFunctionality",
                Commands = new[] { upCommand, downCommand },
                ItemId = "hue_bulb210_color_1",
                GroupBox = box
            };

            info.Functionalities = new[] { lightOnOff, lightDimm, color };
            info.States = new[] { state };
            info.GroupBoxes = new[] { box };
            return info;
        }


        private DeviceInfo CreateHomematicDimmer()
        {
            DeviceInfo homematicDimmer = new DeviceInfo()
            {
                Uid = "homematic_dimmer_1",
                DisplayName = "Homematic Dimmer"
            };

            GroupBox box = new GroupBox()
            {
                Uid = "box_dimmer_1",
                Name = "Dimmer 1",
                IconName = "dimmer"
            };

            DeviceCommand onCommand = new DeviceCommand()
            {
                Name = "DEFAULT_ON_COMMAND",
                RealCommandName = "ON",
                CommandType = "dogont:OnCommand"
            };

            DeviceCommand offCommand = new DeviceCommand()
            {
                Name = "DEFAULT_OFF_COMMAND",
                RealCommandName = "OFF",
                CommandType = "dogont:OffCommand"
            };

            DeviceCommand upCommand = new DeviceCommand()
            {
                Name = "DEFAULT_UP_COMMAND",
                RealCommandName = "UP",
                CommandType = "dogont:UpCommand"
            };

            DeviceCommand downCommand = new DeviceCommand()
            {
                Name = "DEFAULT_DOWN_COMMAND",
                RealCommandName = "DOWN",
                CommandType = "dogont:DownCommand"
            };

            DeviceFunctionality f1 = new DeviceFunctionality()
            {
                FunctionalityType = "dogont:LevelControlFunctionality",
                ItemId = "homematic_dimmer_dimmer_1",
                Commands = new[] { onCommand, offCommand, upCommand, downCommand },
                GroupBox = box
            };

            homematicDimmer.Functionalities = new[] { f1 };
            homematicDimmer.GroupBoxes = new[] { box };
            return homematicDimmer;
        }

        #endregion
    }
}
