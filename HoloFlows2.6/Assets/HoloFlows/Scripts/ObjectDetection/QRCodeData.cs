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


        private QRCodeData() { }

        /// <summary>
        /// Create the <see cref="QRCodeData"/> from plain text.
        /// </summary>
        public static QRCodeData FromQrCodeData(string data)
        {
            //TODO remove hardcoded stuff
            QRCodeData qrData = new QRCodeData
            {
                BindingId = "tinkerforge1",
                ThingId = "tinkerforge_irTemp_1",
                IsValid = true
            };
            return qrData;
        }


    }
}
