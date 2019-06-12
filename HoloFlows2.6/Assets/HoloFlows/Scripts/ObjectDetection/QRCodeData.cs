namespace HoloFlows.ObjectDetection
{
    public class QRCodeData
    {
        /// <summary>
        /// The Binding id for openhab.
        /// </summary>
        public string BindingId { get; private set; }

        /// <summary>
        /// The unique thing id for openhab.
        /// </summary>
        public string ThingId { get; private set; }

        /// <summary>
        /// Unique Workflow id
        /// </summary>
        public string WorkflowId { get; private set; }

        /// <summary>
        /// Workflow name human readably for easy demo managment
        /// </summary>
        public string WorkflowName { get; private set; }

        /// <summary>
        /// If false, the qr code has not the correct data stored
        /// </summary>
        public bool IsValid { get; private set; } = false;

        //used for debugging and dev
        private static QRCodeData internalData = null;

        private QRCodeData() { }

        /// <summary>
        /// Create the <see cref="QRCodeData"/> from plain text.
        /// </summary>
        public static QRCodeData FromQrCodeData(string data)
        {
            if (string.IsNullOrEmpty(data)) return null;
            if (!data.Contains("\n") && !data.Contains("\r")) return null;

            string[] split = data.Split(new[] { "\r\n", "\r", "\n" }, System.StringSplitOptions.None);
            if (split.Length < 4) return null;

            QRCodeData newData = new QRCodeData();
            newData.BindingId = split[0];
            newData.ThingId = split[1];
            newData.WorkflowId = split[2];
            newData.WorkflowName = split[3];
            newData.IsValid = !string.IsNullOrEmpty(newData.BindingId)
                && !string.IsNullOrEmpty(newData.ThingId)
                && !string.IsNullOrEmpty(newData.WorkflowId)
                && !string.IsNullOrEmpty(newData.WorkflowName);
            return newData;
        }

        public static QRCodeData ForDebug()
        {
            SetDebuggingData();
            return internalData;
        }


        //for debugging and dev
        private static void SetDebuggingData()
        {
            if (internalData == null)
            {
                internalData = new QRCodeData
                {
                    BindingId = "hue",
                    ThingId = "hue_bulb210_1",
                    WorkflowId = "_kzOggI0TEemo-tbczAUMtw",
                    WorkflowName = "hue_bulb210",
                    IsValid = true
                };
            }
            else if ("hue_bulb210_1".Equals(internalData.ThingId))
            {
                internalData = new QRCodeData
                {
                    BindingId = "tinkerforge1",
                    ThingId = "tinkerforge_irTemp_1",
                    WorkflowId = "_Afr5II0XEemo-tbczAUMtw",
                    WorkflowName = "tinkerforge_irTemp",
                    IsValid = true
                };
            }
            else if ("tinkerforge_irTemp_1".Equals(internalData.ThingId))
            {
                internalData = new QRCodeData
                {
                    BindingId = "tinkerforge1",
                    ThingId = "tinkerforge_ambientLight_2",
                    WorkflowId = "_IjP9YI0LEemo-tbczAUMtw",
                    WorkflowName = "tinkerforge_ambientLight",
                    IsValid = true
                };
            }
            else
            {
                internalData = new QRCodeData
                {
                    BindingId = "hue",
                    ThingId = "hue_bulb210_1",
                    WorkflowId = "_kzOggI0TEemo-tbczAUMtw",
                    WorkflowName = "hue_bulb210",
                    IsValid = true
                };
            }
        }


    }
}
