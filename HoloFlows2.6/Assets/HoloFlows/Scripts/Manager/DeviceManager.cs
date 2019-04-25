﻿
using HoloFlows.Model;
using HoloToolkit.Unity;
using System.Collections.Generic;


namespace HoloFlows.Manager
{
    public class DeviceManager : Singleton<DeviceManager>
    {

        public Dictionary<string, DeviceInfo> DeviceInfos { get; private set; } = new Dictionary<string, DeviceInfo>();

        void Start()
        {
            AddDemoDevices();
        }

        #region demo devices

        private void AddDemoDevices()
        {
            var homematicDimmer = CreateHomematicDimmer();
            var tinkerforgeAmbTemp = CreateTinkerforgeIRTemp();
            var tinkerforgeAmbTemp2 = CreateTinkerforgeIRTemp2();
            DeviceInfos.Add(homematicDimmer.Uid, homematicDimmer);
            DeviceInfos.Add(tinkerforgeAmbTemp.Uid, tinkerforgeAmbTemp);
            DeviceInfos.Add(tinkerforgeAmbTemp2.Uid, tinkerforgeAmbTemp2);
        }

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

            DeviceState irTempState = new DeviceState()
            {
                ItemId = "tinkerforge_irTemp_irTemp_1",
                Label = "Object Temp",
                UnitOfMeasure = degree,
                RealStateValue = "20",
                StateType = "dogont:TemperatureState"
            };

            DeviceState ambTempState = new DeviceState()
            {
                ItemId = "tinkerforge_irTemp_ambTemp_1",
                Label = "Ambiente Temp",
                UnitOfMeasure = degree,
                RealStateValue = "15",
                StateType = "dogont:TemperatureState"
            };


            tinkerforgeIrTemp.States = new[] { irTempState, ambTempState };
            return tinkerforgeIrTemp;
        }

        private DeviceInfo CreateTinkerforgeIRTemp2()
        {
            DeviceInfo tinkerforgeIrTemp = new DeviceInfo()
            {
                Uid = "tinkerforge_irTemp_2",
                DisplayName = "Tinkerforge IR Temp"
            };

            UnitOfMeasure degree = new UnitOfMeasure()
            {
                UnitName = "degree-Celsius",
                UnitType = "temperature",
                PrefixSymbol = "°C"
            };

            DeviceState irTempState = new DeviceState()
            {
                ItemId = "tinkerforge_irTemp_irTemp_2",
                Label = "Object Temp1",
                UnitOfMeasure = degree,
                RealStateValue = "20",
                StateType = "dogont:TemperatureState"
            };

            DeviceState ambTempState = new DeviceState()
            {
                ItemId = "tinkerforge_irTemp_ambTemp_2",
                Label = "Ambiente Temp1",
                UnitOfMeasure = degree,
                RealStateValue = "15",
                StateType = "dogont:TemperatureState"
            };

            DeviceState ambTempState2 = new DeviceState()
            {
                ItemId = "tinkerforge_irTemp_ambTemp_3",
                Label = "Ambiente Temp2",
                UnitOfMeasure = degree,
                RealStateValue = "28",
                StateType = "dogont:TemperatureState"
            };


            tinkerforgeIrTemp.States = new[] { irTempState, ambTempState, ambTempState2 };
            return tinkerforgeIrTemp;
        }

        private DeviceInfo CreateHomematicDimmer()
        {
            DeviceInfo homematicDimmer = new DeviceInfo()
            {
                Uid = "homematic_dimmer_1",
                DisplayName = "Homematic Dimmer"
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
                Commands = new[] { onCommand, offCommand, upCommand, downCommand }
            };

            homematicDimmer.Functionalities = new[] { f1 };
            return homematicDimmer;
        }

        #endregion
    }
}

