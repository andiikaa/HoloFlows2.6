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
            if (split.Length < 2) return null;

            QRCodeData newData = new QRCodeData();
            newData.BindingId = split[0];
            newData.ThingId = split[1];
            newData.IsValid = !string.IsNullOrEmpty(newData.BindingId) && !string.IsNullOrEmpty(newData.ThingId);

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
                    IsValid = true
                };
            }
            else if ("hue_bulb210_1".Equals(internalData.ThingId))
            {
                internalData = new QRCodeData
                {
                    BindingId = "tinkerforge1",
                    ThingId = "tinkerforge_irTemp_1",
                    IsValid = true
                };
            }
            else if ("tinkerforge_irTemp_1".Equals(internalData.ThingId))
            {
                internalData = new QRCodeData
                {
                    BindingId = "tinkerforge1",
                    ThingId = "tinkerforge_ambientLight_2",
                    IsValid = true
                };
            }
            else
            {
                internalData = new QRCodeData
                {
                    BindingId = "hue",
                    ThingId = "hue_bulb210_1",
                    IsValid = true
                };
            }
        }


    }
}
