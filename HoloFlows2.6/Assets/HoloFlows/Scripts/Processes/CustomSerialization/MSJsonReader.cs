using Newtonsoft.Json;
using System.IO;

namespace HoloFlows.Processes.CustomSerialization
{
    public class MSJsonReader : JsonTextReader
    {
        public MSJsonReader(TextReader reader) : base(reader) { }

        public override bool Read()
        {
            var hasToken = base.Read();

            if (hasToken && base.TokenType == JsonToken.PropertyName && base.Value != null && base.Value.Equals("__type"))
                base.SetToken(JsonToken.PropertyName, "$type");

            return hasToken;
        }
    }
}
