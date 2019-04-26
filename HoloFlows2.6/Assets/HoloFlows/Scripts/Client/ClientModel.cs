using System.Collections.Generic;

namespace HoloFlows.Client
{

    public class ItemDataShort
    {
        public string state;
        public string name;
    }

    public class ItemData
    {
        public string state;
        public string type;
        public string name;
        public string label;
        public StateDescription stateDescription;
    }

    public class StateDescription
    {
        //pattern pattern: "datatype unit", e.g. "%.1f mbar"
        public string pattern;
    }

    public class ItemStates
    {
        public static readonly List<string> IgnoredStates = new List<string>() { "NULL", "UNDEF" };
    }
}
