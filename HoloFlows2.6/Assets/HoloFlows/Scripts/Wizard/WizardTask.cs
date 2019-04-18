using System;

namespace HoloFlows.Wizard
{

    public class WizardTask
    {
        public string Name { get; set; }
        public string Instruction { get; set; }
        public string AudioUri { get; set; }
        public string ImageUri { get; set; }

        /// <summary>
        /// Timeout for static functionality evaluation.
        /// Not to be used in production!
        /// </summary>
        [Obsolete("Should not be used for production")]
        public int Timeout { get; set; }

    }
}

