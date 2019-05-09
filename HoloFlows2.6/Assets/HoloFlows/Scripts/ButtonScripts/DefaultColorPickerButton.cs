using HoloFlows.Model;
using UnityEngine;

namespace HoloFlows.ButtonScripts
{
    public class DefaultColorPickerButton : MonoBehaviour
    {
        private SingleColorButton redBtn;
        private SingleColorButton purpleBtn;
        private SingleColorButton blueBtn;
        private SingleColorButton greenBtn;
        private SingleColorButton yellowBtn;
        private SingleColorButton whiteBtn;

        private void InitButtons()
        {
            redBtn = transform.Find("Red").GetComponent<SingleColorButton>();
            purpleBtn = transform.Find("Purple").GetComponent<SingleColorButton>();
            blueBtn = transform.Find("Blue").GetComponent<SingleColorButton>();
            greenBtn = transform.Find("Green").GetComponent<SingleColorButton>();
            yellowBtn = transform.Find("Yellow").GetComponent<SingleColorButton>();
            whiteBtn = transform.Find("White").GetComponent<SingleColorButton>();
        }

        public void SetFunctionality(DeviceFunctionality func)
        {
            if (redBtn == null) { InitButtons(); }

            redBtn.DeviceId = func.ItemId;
            redBtn.RealCommandName = "359,100,100";

            purpleBtn.DeviceId = func.ItemId;
            purpleBtn.RealCommandName = "302,96,100";

            blueBtn.DeviceId = func.ItemId;
            blueBtn.RealCommandName = "240,100,100";

            greenBtn.DeviceId = func.ItemId;
            greenBtn.RealCommandName = "125,100,100";

            yellowBtn.DeviceId = func.ItemId;
            yellowBtn.RealCommandName = "60,100,100";

            whiteBtn.DeviceId = func.ItemId;
            whiteBtn.RealCommandName = "0,0,100";
        }
    }
}
