using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Processes.Proteus.Rest.Model;
using System;

namespace HoloFlows.Processes.ProteusJsonConverter
{
    /// <summary>
    /// Custom converter for converting <see cref="IHumanTaskRequest"/> because of the different type handling.
    /// (@class in java vs. $type in newotonsoft.json)
    /// </summary>
    public class HumanTaskRequestConverter : JsonCreationConverter<IHumanTaskRequest>
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            //no writing to json needed
            //@class information is handled in the specific model objects
            //see IJsonType.ClassInfo
            throw new NotImplementedException();
        }


        protected override IHumanTaskRequest Create(Type objectType, JObject jObject)
        {
            IHumanTaskRequest request = new IHumanTaskRequest();


            return request;
        }
    }
}
