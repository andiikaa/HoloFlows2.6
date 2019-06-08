using System;

namespace HoloFlows.Processes.CustomSerialization
{
    public class MSRestToJsonDotNetSerializationBinder : System.Runtime.Serialization.SerializationBinder
    {
        public string ServiceNamespace { get; set; }
        public string LocalNamespace { get; set; }

        public MSRestToJsonDotNetSerializationBinder(string serviceNamespace, string localNamespace)
        {
            if (serviceNamespace.EndsWith("."))
                serviceNamespace = serviceNamespace.Substring(0, -1);

            if (localNamespace.EndsWith("."))
                localNamespace = localNamespace.Substring(0, -1);

            ServiceNamespace = serviceNamespace;
            LocalNamespace = localNamespace;
        }

        public override void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            assemblyName = null;
            typeName = string.Format("{0}:#{1}", serializedType.Name, ServiceNamespace); // MS format
        }

        public override Type BindToType(string assemblyName, string typeName)
        {
            string jsonDotNetType = string.Format("{0}.{1}", LocalNamespace, typeName.Substring(0, typeName.IndexOf(":#")));
            return Type.GetType(jsonDotNetType);
        }
    }
}
