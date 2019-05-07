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
                Name = "IR Temp",
                IconName = "temp"
            };

            GroupBox boxAmb = new GroupBox()
            {
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

        //private DeviceInfo CreateTinkerforgeIRTemp2()
        //{
        //    DeviceInfo tinkerforgeIrTemp = new DeviceInfo()
        //    {
        //        Uid = "tinkerforge_irTemp_2",
        //        DisplayName = "Tinkerforge IR Temp"
        //    };

        //    UnitOfMeasure degree = new UnitOfMeasure()
        //    {
        //        UnitName = "degree-Celsius",
        //        UnitType = "temperature",
        //        PrefixSymbol = "°C"
        //    };

        //    DeviceState irTempState = new DeviceState()
        //    {
        //        ItemId = "tinkerforge_irTemp_irTemp_2",
        //        Label = "Object Temp1",
        //        UnitOfMeasure = degree,
        //        RealStateValue = "20",
        //        StateType = "dogont:TemperatureState"
        //    };

        //    DeviceState ambTempState = new DeviceState()
        //    {
        //        ItemId = "tinkerforge_irTemp_ambTemp_2",
        //        Label = "Ambiente Temp1",
        //        UnitOfMeasure = degree,
        //        RealStateValue = "15",
        //        StateType = "dogont:TemperatureState"
        //    };

        //    DeviceState ambTempState2 = new DeviceState()
        //    {
        //        ItemId = "tinkerforge_irTemp_ambTemp_3",
        //        Label = "Ambiente Temp2",
        //        UnitOfMeasure = degree,
        //        RealStateValue = "28",
        //        StateType = "dogont:TemperatureState"
        //    };


        //    tinkerforgeIrTemp.States = new[] { irTempState, ambTempState, ambTempState2 };
        //    return tinkerforgeIrTemp;
        //}

        private DeviceInfo CreateHomematicDimmer()
        {
            DeviceInfo homematicDimmer = new DeviceInfo()
            {
                Uid = "homematic_dimmer_1",
                DisplayName = "Homematic Dimmer"
            };

            GroupBox box = new GroupBox()
            {
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
