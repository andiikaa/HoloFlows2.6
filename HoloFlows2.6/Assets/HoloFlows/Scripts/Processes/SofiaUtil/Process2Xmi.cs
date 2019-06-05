using Processes.Sofia;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace HoloFlows.Processes.SofiaUtil
{
    /// <summary>
    /// <para>
    /// Converter class, to convert a Sofia <see cref="Process"/> to an *.sofia file.
    /// Use the <see cref="Transform(Process)"/> method to write a process to string.
    /// </para>
    /// <para>
    /// Be sure you have the latest sofia.ecore file located in <see cref="ECORE_PATH"/>.
    /// This file is needed cause it contains the structural info, how to write the file.
    /// </para>
    /// </summary>
    public class Process2Xmi
    {
        private readonly XNamespace NS_SOFIA = "http://vicci.eu/sofia/1.0";
        private const string NS_SOFIA_NAME = "sofia";
        private readonly XNamespace NS_XMI = "http://www.omg.org/XMI";
        private const string NS_XMI_NAME = "xmi";
        private readonly XNamespace NS_XSI = "http://www.w3.org/2001/XMLSchema-instance";
        private const string NS_XSI_NAME = "xsi";
        private readonly XNamespace NS_ECORE = "http://www.eclipse.org/emf/2002/Ecore";
        private const string NS_ECORE_NAME = "ecore";
        private const string XMI_VERSION = "2.0";
        private const string XML_VERSION = "1.0";
        private const string ENCODING = "UTF-8";
        private const string ECORE_PATH = "sofia.ecore";

        private readonly XDocument sofiaModel;
        //type name, feature (e.g. 'ProcessStep', feature)
        private readonly IDictionary<string, IList<StructuralFeature>> typeDefinitions;
        //object, reference as string (e.g. /1/@ports.5/@outTransitions.0/)
        private readonly IDictionary<object, string> references;

        private readonly ISet<object> doneNodes;

        public Process2Xmi()
        {
            //we need the sofia.ecore cause we need to know the structure of the resulting document
            sofiaModel = XDocument.Load(ECORE_PATH);
            typeDefinitions = new Dictionary<string, IList<StructuralFeature>>();
            references = new Dictionary<object, string>();
            doneNodes = new HashSet<object>();
            PopulateTypeMap();
        }

        /// <summary>
        /// This methods takes a <see cref="Process"/> and writes it to a *.sofia file.
        /// <see cref="ArgumentException"/>s are thrown, if something is wrong with the sofia.ecore file or the model itself.
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public string Transform(Process root)
        {
            references.Clear();
            doneNodes.Clear();
            PopulateReferenceMap(root);
            XDocument doc = new XDocument();
            XElement xmiElement = new XElement(NS_XMI + "XMI",
                    new XAttribute(XNamespace.Xmlns + NS_XMI_NAME, NS_XMI),
                    new XAttribute(XNamespace.Xmlns + NS_SOFIA_NAME, NS_SOFIA),
                    new XAttribute(XNamespace.Xmlns + NS_XSI_NAME, NS_XSI),
                    new XAttribute(NS_XMI + "version", XMI_VERSION)
            );

            //root process
            XElement sofiaProcess = new XElement(NS_SOFIA + "Process",
                        new XAttribute("id", root.id),
                        new XAttribute("name", root.name)
            );

            sofiaProcess.Add(GenAllElements(root));
            xmiElement.Add(sofiaProcess);
            doc.Add(xmiElement);

            doc.Declaration = new XDeclaration(XML_VERSION, ENCODING, null);
            return doc.Declaration + "\n" + doc.ToString();
        }

        #region reference creation

        /// <summary>
        /// We need to generate all reference strings. 
        /// A reference is a string (e.g. '//@subSteps.1/@ports.0'), which defines the position of a referenced object, within a *.sofia file
        /// </summary>
        private void PopulateReferenceMap(object someObject, string reference = null)
        {
            if (doneNodes.Contains(someObject)) return;
            if (reference == null) reference = "/";

            Type someObjectType = someObject.GetType();
            IList<StructuralFeature> features;
            if (!typeDefinitions.TryGetValue(someObjectType.Name, out features))
                return;

            //if (!references.ContainsKey(someObject))
            //    references.Add(someObject, reference);

            doneNodes.Add(someObject);
            AddCurrentPropertyReferences(features, someObject, reference);
            AddPropertyReferencesOfChilds(features, someObject, reference);
        }


        private void AddCurrentPropertyReferences(IList<StructuralFeature> features, object someObject, string reference)
        {
            LoopOverFeatures(features, someObject, reference, AddPropertyListToReferences, AddPropertyToReferences);
        }

        private void AddPropertyReferencesOfChilds(IList<StructuralFeature> features, object someObject, string reference)
        {
            LoopOverFeatures(features, someObject, reference, PopulateReferencesForListProperty, PopulateReferencesForProperty);
        }

        private void LoopOverFeatures(IList<StructuralFeature> features, object someObject, string reference,
            Action<StructuralFeature, PropertyInfo, object, string> doForList,
            Action<StructuralFeature, PropertyInfo, object, string> doForElement)
        {
            Type someObjectType = someObject.GetType();
            foreach (StructuralFeature feature in features)
            {
                PropertyInfo info = someObjectType.GetProperty(feature.Name);
                if (feature.IsContainment && IsSofiaType(info.PropertyType))
                {
                    if (feature.IsList) doForList(feature, info, someObject, reference);
                    else doForElement(feature, info, someObject, reference);
                }
            }
        }

        /// <summary>
        /// This will pick up all references from the objects within a property which is a list
        /// </summary>
        private void PopulateReferencesForListProperty(StructuralFeature feature, PropertyInfo info, object someObject, string reference)
        {
            object tmpValue = info.GetValue(someObject);
            if (tmpValue == null)
                return;
            IEnumerable tmpList = tmpValue as IEnumerable;
            if (tmpList != null)
            {
                int count = 0;
                foreach (object so in tmpList)
                {
                    PopulateReferenceMap(so, reference + "/@" + feature.Name + "." + count);
                    count++;
                }
            }
        }

        /// <summary>
        /// This will pick up all references for a property
        /// </summary>
        private void PopulateReferencesForProperty(StructuralFeature feature, PropertyInfo info, object someObject, string reference)
        {
            object tmpValue = info.GetValue(someObject);
            if (tmpValue == null) return;
            PopulateReferenceMap(tmpValue, reference + "/@" + feature.Name);
        }

        /// <summary>
        /// Adds all object references from a property which is a list
        /// </summary>
        private void AddPropertyListToReferences(StructuralFeature feature, PropertyInfo info, object someObject, string reference)
        {
            object tmpValue = info.GetValue(someObject);
            if (tmpValue == null)
                return;
            IEnumerable subs = tmpValue as IEnumerable;
            if (subs == null)
                throw new ArgumentException("property is no list? {0}", info.Name);

            int count = 0;
            foreach (object so in subs)
            {
                string tmpRef = reference + "/@" + feature.Name + "." + count;
                if (!references.ContainsKey(so))
                    references.Add(so, tmpRef);
                count++;
            }
        }

        /// <summary>
        /// Adds the reference of an object
        /// </summary>
        private void AddPropertyToReferences(StructuralFeature feature, PropertyInfo info, object someObject, string reference)
        {
            string tmpRef = reference + "/@" + feature.Name;
            object tmpValue = info.GetValue(someObject);
            if (tmpValue == null)
                return;
            if (!references.ContainsKey(tmpValue))
                references.Add(tmpValue, tmpRef);
        }

        #endregion

        #region XElement creation

        /// <summary>
        /// Creates all XElements for the given object.
        /// Creates also all XElements for child objects of the given sofiaObject (recursive call).
        /// </summary>
        private IEnumerable<XElement> GenAllElements(object sofiaObject, string reference = null)
        {
            if (reference == null) reference = "/";

            Type sofiaType = sofiaObject.GetType();
            IList<StructuralFeature> features;
            if (!typeDefinitions.TryGetValue(sofiaType.Name, out features))
                throw new ArgumentException(string.Format("is the given type a sofiaType? '{0}'", sofiaType.Name));

            var list = new List<XElement>();
            foreach (StructuralFeature feature in features)
            {
                CreateAndAddXElementsToList(feature, sofiaType, sofiaObject, reference, list);
            }

            return list;
        }

        private void CreateAndAddXElementsToList(StructuralFeature feature, Type sofiaType, object sofiaObject, string reference, IList<XElement> targetList)
        {
            //only containments create new XElement
            if (!feature.IsContainment) return;

            PropertyInfo info = sofiaType.GetProperty(feature.Name);

            //objects which are no sofia model objects are ignored
            if (!IsSofiaType(info.PropertyType)) return;

            EOpposite opposite = new EOpposite()
            {
                TypeName = sofiaType.Name,
                PropertyName = info.Name
            };

            if (feature.IsList)
            {
                AddXElementForListPropertyToList(feature, info, opposite, sofiaObject, reference, targetList);
            }
            else
            {
                object propertyValue = info.GetValue(sofiaObject);
                targetList.Add(CreateSimpleElement(feature, propertyValue, opposite, reference));
            }
        }

        private void AddXElementForListPropertyToList(StructuralFeature feature, PropertyInfo info, EOpposite opposite, object sofiaObject, string reference, IList<XElement> targetList)
        {
            object tmpValue = info.GetValue(sofiaObject);
            if (tmpValue == null)
                return;
            IEnumerable subs = tmpValue as IEnumerable;
            if (subs == null)
                throw new ArgumentException("property is no list? {0}", info.Name);

            int count = 0;
            foreach (object so in subs)
            {
                var ele = CreateSimpleElement(feature, so, opposite, reference + "/" + feature.Name + "." + count);
                targetList.Add(ele);
                count++;
            }
        }

        private XElement CreateSimpleElement(StructuralFeature feature, object propertyValue, EOpposite opposite, string reference)
        {
            if (!references.ContainsKey(propertyValue))
                references.Add(propertyValue, reference);

            var ele = new XElement(feature.Name,
                new XAttribute(NS_XSI + "type", NS_SOFIA_NAME + ":" + propertyValue.GetType().Name)
                );

            ele.Add(CreateAttributes(propertyValue, opposite));
            ele.Add(GenAllElements(propertyValue, reference));
            return ele;
        }

        #endregion

        #region XAttribute creation

        private XAttribute SofiaTypeToAttribute(PropertyInfo info, object obj, EOpposite opposite, bool isList = false)
        {
            Type objType = obj.GetType();

            IList<StructuralFeature> features = null;
            if (!typeDefinitions.TryGetValue(objType.Name, out features))
                throw new ArgumentException(string.Format("is the given type a sofiaType? '{0}'", objType.Name));
            StructuralFeature feature = features.Where(f => f.Name == info.Name).FirstOrDefault();
            if (feature == null)
                throw new ArgumentException(string.Format("the property '{0}' could not be found in the structuralFeatures", info.Name));

            //containments are not added as attributes
            if (feature.IsContainment)
                return null;

            if (feature.Opposite != null)
            {
                //check if current type + propertyname are the opposite
                //opposites are backreferences and are not added
                //TODO also check the type for more safety
                if (feature.Opposite.PropertyName == opposite.PropertyName)
                    return null;

                if (isList)
                    return CreateAttributeForOpposite(feature, info, opposite, obj);
            }

            object attrValue = info.GetValue(obj);
            if (attrValue == null)
                return null;

            if (feature.IsReference)
                return CreateAttributeWithReference(info, attrValue);

            return new XAttribute(info.Name, attrValue.ToString());
        }

        private XAttribute CreateAttributeWithReference(PropertyInfo info, object attributeValue)
        {
            string reference = null;
            if (!references.TryGetValue(attributeValue, out reference))
                throw new ArgumentException(string.Format("reference not found for object in {0}", info.Name));
            return new XAttribute(info.Name, reference);
        }

        private XAttribute CreateAttributeForOpposite(StructuralFeature feature, PropertyInfo info, EOpposite opposite, object obj)
        {
            //attribute values are split by " "
            IEnumerable tmpList = (IEnumerable)info.GetValue(obj);
            if (tmpList == null) return null;
            string valueString = null;
            foreach (object tmpObject in tmpList)
            {
                if (valueString != null) valueString += " ";
                if (valueString == null) valueString += "";

                string reference = null;
                if (!references.TryGetValue(tmpObject, out reference))
                    throw new ArgumentException(string.Format("reference not found for object in {0}", info.Name));

                valueString += reference;
            }
            return new XAttribute(info.Name, valueString);
        }

        private IEnumerable<XAttribute> CreateAttributes(object obj, EOpposite opposite)
        {
            var list = new List<XAttribute>();
            foreach (PropertyInfo info in obj.GetType().GetProperties())
            {
                var attr = PropertyToAttribute(info, obj, opposite);
                if (attr != null) list.Add(attr);
            }
            return list;
        }

        private XAttribute PropertyToAttribute(PropertyInfo info, object obj, EOpposite opposite)
        {
            var tmpValue = info.GetValue(obj);
            if (tmpValue == null) return null;

            if (IsKnownSofiaType(info.PropertyType.Name)) return SofiaTypeToAttribute(info, obj, opposite);
            if (IsListWithSofiaType(info)) return SofiaTypeToAttribute(info, obj, opposite, true);

            if (!IsSimpleType(info.PropertyType)) return null;
            //for some default values, we dont need the attribute
            if (info.PropertyType == typeof(bool) && !(bool)tmpValue) return null;
            if (info.PropertyType == typeof(string) && string.IsNullOrEmpty((string)tmpValue)) return null;
            return new XAttribute(info.Name, tmpValue);
        }

        #endregion

        #region helper methods

        private bool IsListWithSofiaType(PropertyInfo info)
        {
            return info.PropertyType.GetTypeInfo().IsGenericType
                && typeof(IList).IsAssignableFrom(info.PropertyType)
                && CheckGenericArgsLength(info.PropertyType)
                && IsKnownSofiaType(info.PropertyType.GetGenericArguments()[0].Name);
        }

        private bool IsKnownSofiaType(string typeName) { return typeDefinitions.ContainsKey(typeName); }

        private bool CheckGenericArgsLength(Type type)
        {
            //our model should only contain 1 generic type
            if (type.GetGenericArguments().Length != 1)
                throw new ArgumentException("the generic type contains more or less than one type parameter. this is currently not supported.");
            return true;
        }

        private bool IsSimpleType(Type type)
        {
            if (type == typeof(string)) return true;
            if (type == typeof(int)) return true;
            if (type == typeof(bool)) return true;
            return false;
        }

        private bool IsSofiaType(Type type)
        {
            if (!type.GetTypeInfo().IsGenericType)
                return type.Namespace == typeof(Process).Namespace;
            CheckGenericArgsLength(type);
            return type.GetGenericArguments()[0].Namespace == typeof(Process).Namespace;
        }

        #endregion

        #region structural methods
        private void PopulateTypeMap()
        {
            IEnumerable<XElement> classifiers = sofiaModel.Element(NS_ECORE + "EPackage").Elements("eClassifiers");

            var t = from e in classifiers
                    let feats = e.Elements("eStructuralFeatures")
                    select new Classifier()
                    {
                        Name = e.Attribute("name").Value,
                        Element = e,
                        XFeatures = feats
                    };

            foreach (Classifier classifier in t)
            {
                var list = new List<StructuralFeature>();
                list.AddRange(CreateFeatures(classifier.XFeatures));
                list.AddRange(SuperTypeFeatures(classifier.Element));
                typeDefinitions.Add(classifier.Name, list);
            }
        }

        private IEnumerable<StructuralFeature> CreateFeatures(IEnumerable<XElement> xFeatures)
        {
            return from xFeature in xFeatures
                   select new StructuralFeature()
                   {
                       EType = GetMemberType(xFeature),
                       IsReference = IsReference(xFeature),
                       IsContainment = IsContainment(xFeature),
                       Name = xFeature.Attribute("name").Value,
                       Opposite = CreateOpposite(xFeature),
                       IsList = IsList(xFeature)
                   };
        }

        private IEnumerable<StructuralFeature> SuperTypeFeatures(XElement classifier)
        {
            var st = classifier.Attribute("eSuperTypes");
            string stVal = st?.Value;
            if (stVal == null)
                return new List<StructuralFeature>();
            string[] supertypes = stVal.Split(' ');
            var superClassifiers = sofiaModel.Element(NS_ECORE + "EPackage")
                .Elements("eClassifiers")
                .Where(e => supertypes.Contains("#//" + e.Attribute("name").Value))
                .ToList();

            if (superClassifiers.Count != supertypes.Length)
                throw new ArgumentException("supertypes in eStructuralFeature and found supertypes does not match");

            var list = new List<StructuralFeature>();

            foreach (XElement superClassifier in superClassifiers)
            {
                var xFeatures = superClassifier.Elements("eStructuralFeatures");
                list.AddRange(CreateFeatures(xFeatures));
                list.AddRange(SuperTypeFeatures(superClassifier));
            }
            return list;
        }

        private bool IsContainment(XElement e)
        {
            var attr = e.Attribute("containment");
            return attr != null && attr.Value == "true";
        }

        private bool IsReference(XElement e)
        {
            var attr = e.Attribute(NS_XSI + "type");
            if (attr == null)
                throw new ArgumentException(string.Format("type Attribute not found for {0}", e.Name));
            return attr.Value == "ecore:EReference";
        }

        private static string GetMemberType(XElement e)
        {
            var attr = e.Attribute("eType");
            if (attr == null)
                throw new ArgumentException(string.Format("eType Attribute not found for {0}", e.Name));

            if (attr.Value.StartsWith("ecore:"))
            {
                return MapEcoreType(attr.Value);
            }

            return attr.Value.Replace("#//", string.Empty);
        }

        private static bool IsList(XElement xFeature)
        {
            var upper = xFeature.Attribute("upperBound");
            if (upper == null) return false;
            return !string.IsNullOrEmpty(upper.Value) && upper.Value == "-1";
        }

        private static EOpposite CreateOpposite(XElement xFeature)
        {
            var attr = xFeature.Attribute("eOpposite");
            if (attr == null)
                return null;
            string val = attr.Value;
            val = val.Replace("#//", string.Empty);
            string[] split = val.Split('/');
            if (split.Length != 2)
                throw new ArgumentException(string.Format("expected eOpposite attribute like 'TypeName/propertyName' but was '{0}'", val));

            return new EOpposite()
            {
                TypeName = split[0],
                PropertyName = split[1]
            };
        }

        private static string MapEcoreType(string typeString)
        {
            int pos = typeString.IndexOf("#//");
            string tmpString = typeString.Substring(pos + 3);
            switch (tmpString)
            {
                case "EBoolean": return "bool";
                case "EInt": return "int";
                case "EString": return "string";
                case "ELong": return "long";
                default: throw new ArgumentException(string.Format("type not implemented yet: {0}", tmpString));
            }
        }
        #endregion

        #region HelperClasses

        public class Classifier
        {
            public string Name { get; set; }
            public XElement Element { get; set; }
            public IEnumerable<XElement> XFeatures { get; set; }
        }

        public class StructuralFeature
        {
            /// <summary>
            /// name of the property
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// if this is true, we must reference the object (e.g /1/@subSteps.0), otherwise it should be possible to add the value directly
            /// </summary>
            public bool IsReference { get; set; }
            /// <summary>
            /// if this is true, the object is listet as XElement under the parent
            /// </summary>
            public bool IsContainment { get; set; }
            /// <summary>
            /// type name of the property
            /// </summary>
            public string EType { get; set; }
            /// <summary>
            /// Backreference. If this is not empty, we must check, if the current type really needs to include this type under the given property name
            /// </summary>  
            public EOpposite Opposite { get; set; }
            /// <summary>
            /// if the StructuralFeature has a upper bound=-1 this should mean, we need a list for this type
            /// </summary>
            public bool IsList { get; set; }
        }

        public class EOpposite
        {
            public string TypeName { get; set; }
            public string PropertyName { get; set; }
        }

        #endregion

    }
}