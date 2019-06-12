using UnityEngine;

namespace HoloFlows.Processes
{
    public class ProcessLoadUtil
    {
        public const string LOCAL_PROCESS_MODEL_PATH = "ProcessModels/";

        private ProcessLoadUtil() { }

        /// <summary>
        /// Loads a process definition from the resources.
        /// 
        /// NOTE: process files have the txt file extension. Something like 'processName.txt'. Otherwise unity wont read the files as textasset.
        /// </summary>
        /// <param name="processPath">the path of the process file within 
        /// the <see cref="ProcessLoadUtil.LOCAL_PROCESS_MODEL_PATH"/> e.g. 'someAdditionalFolderName/tinkerforge_irTemp'
        /// </param>
        /// <returns>the process definition as string or null if process file was not found</returns>
        public static string LoadLocalProcessDefinition(string processPath)
        {
            TextAsset text = Resources.Load<TextAsset>(LOCAL_PROCESS_MODEL_PATH + processPath);
            if (text == null) return null;
            return text.text;
        }
    }
}
