using System.Collections.Generic;


namespace HoloFlows.Processes.Sofia
{

    ///
    /// This Class was autogenerated with genmymodel.com
    ///
    public abstract class ProcessStep : Identifiable, INameable, CpsStep
    {
#pragma warning disable IDE1006
        public string type { get; set; }
        public string description { get; set; }
        public string resource { get; set; }


        public CompositeStep parentstep { get; set; }
        public List<Port> ports { get; set; }


        ///Implements for CpsStep
        public bool cyberPhysical { get; set; }
        public string goal { get; set; }
        public string eplQuery { get; set; }
        public string controlProcessId { get; set; }
        public string context { get; set; }

        public Process controlProcess { get; set; }

        public string name { get; set; }

#pragma warning restore IDE1006
    }

}