using Newtonsoft.Json;
using System.IO;

namespace HoloFlows.Processes.CustomSerialization
{
    public class CustomJsonWriter : JsonTextWriter
    {
        public CustomJsonWriter(TextWriter writer) : base(writer)
        {
        }

        public override void WritePropertyName(string name, bool escape)
        {
            if (name == "$type") name = "__type";
            base.WritePropertyName(name, escape);
        }
    }
}
