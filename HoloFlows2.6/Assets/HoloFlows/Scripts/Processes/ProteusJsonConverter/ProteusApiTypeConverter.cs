using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Processes.Proteus.Rest.Model;
using System;
using UnityEngine;

namespace HoloFlows.Processes.ProteusJsonConverter
{

    /// <summary>
    /// Custom converter for proteus api model. Some model will contain a @class info about the type.
    /// We need this to create the correct type.
    /// <see cref="IJSONType"/>, <see cref="IJSONPort"/>, <see cref="IJSONPortInstance"/>, <see cref="IJSONTypeInstance"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ProteusApiTypeConverter<T> : JsonCreationConverter<T>
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            //no writing to json needed
            //@class information is handled in the specific model objects
            //see IJsonType.ClassInfo
            throw new NotImplementedException();
        }

        protected override T Create(Type objectType, JObject jObject)
        {
            if (jObject["@class"] == null)
            {
                throw new JsonReaderException(string.Format("object does not contain @class info: {0}", jObject.ToString()));
            }

            string typeName = jObject["@class"].Value<string>();
            return GetTypeForDeserilization(typeName, jObject);
        }

        private static string ExtractTypeName(string typeName)
        {
            //typename should be something like this
            //"@class: eu.vicci.process.model.util.serialization.jsonprocesssteps.JSONDataPort";
            int lastIndex = typeName.LastIndexOf(".") + 1;
            //because swagger names all types I... due to proteus rest config
            string directTypeName = "I" + typeName.Substring(lastIndex);
            Debug.LogFormat("Type: {0}", directTypeName);
            return directTypeName;
        }

        //FIXME this is super ugly - find better solution
        private T GetTypeForDeserilization(string typeName, JObject jObject)
        {
            string directTypeName = ExtractTypeName(typeName);
            object result = null;
            switch (directTypeName)
            {
                case "IJSONType":
                    result = jObject.ToObject<IJSONType>();
                    break;
                case "IJSONComplexType":
                    result = jObject.ToObject<IJSONComplexType>();
                    break;
                case "IJSONStringType":
                    result = jObject.ToObject<IJSONStringType>();
                    break;
                case "IJSONBooleanType":
                    result = jObject.ToObject<IJSONBooleanType>();
                    break;
                case "IJSONDoubleType":
                    result = jObject.ToObject<IJSONDoubleType>();
                    break;
                case "IJSONSetType":
                    result = jObject.ToObject<IJSONSetType>();
                    break;
                case "IJSONIntegerType":
                    result = jObject.ToObject<IJSONIntegerType>();
                    break;
                case "IJSONListType":
                    result = jObject.ToObject<IJSONListType>();
                    break;

                case "IJSONPort":
                    result = jObject.ToObject<IJSONPort>();
                    break;
                case "IJSONDataPort":
                    result = jObject.ToObject<IJSONDataPort>();
                    break;
                case "IJSONEscalationPort":
                    result = jObject.ToObject<IJSONEscalationPort>();
                    break;

                case "IJSONPortInstance":
                    result = jObject.ToObject<IJSONPortInstance>();
                    break;
                case "IJSONDataPortInstance":
                    result = jObject.ToObject<IJSONDataPortInstance>();
                    break;
                case "IJSONEscalationPortInstance":
                    result = jObject.ToObject<IJSONEscalationPortInstance>();
                    break;


                case "IJSONTypeInstance":
                    result = jObject.ToObject<IJSONTypeInstance>();
                    break;
                case "IJSONStringTypeInstance":
                    result = jObject.ToObject<IJSONStringTypeInstance>();
                    break;
                case "IJSONSetTypeInstance":
                    result = jObject.ToObject<IJSONSetTypeInstance>();
                    break;
                case "IJSONListTypeInstance":
                    result = jObject.ToObject<IJSONListTypeInstance>();
                    break;
                case "IJSONIntegerTypeInstance":
                    result = jObject.ToObject<IJSONIntegerTypeInstance>();
                    break;
                case "IJSONComplexTypeInstance":
                    result = jObject.ToObject<IJSONComplexTypeInstance>();
                    break;
                case "IJSONBooleanTypeInstance":
                    result = jObject.ToObject<IJSONBooleanTypeInstance>();
                    break;
                case "IJSONDoubleTypeInstance":
                    result = jObject.ToObject<IJSONDoubleTypeInstance>();
                    break;

            }

            return (T)result;
        }
    }
}
