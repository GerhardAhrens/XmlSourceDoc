namespace EasyPrototyping.XMLSourceDoc
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Xml.Linq;


    /// <summary>
    /// Methods for combining method information from reflection and XML documentation
    /// </summary>
    public class XmlSourceDocumentation : IDisposable
    {
        private bool classIsDisposed = false;
        private readonly Dictionary<string, MemberDocumentation> MemberSummaries = new Dictionary<string, MemberDocumentation>();


        /// <summary>
        /// Load method information from XML documentation file
        /// </summary>
        /// <param name="xmlFile">Dateiname</param>
        /// <param name="value">Ohne</param>
        public XmlSourceDocumentation(string xmlFile, string value = "")
        {
            if (File.Exists(xmlFile) == false)
            {
                throw new FileNotFoundException("XML Documenation File not found!", xmlFile);
            }

            XDocument doc = XDocument.Load(xmlFile);
            foreach (XElement element in doc.Element("doc").Element("members").Elements())
            {
                string asssembly = ((XElement)((XContainer)element.Document.Document.FirstNode).FirstNode).Value;
                string xmlFullnameWithTypes = element.FirstAttribute.Value;
                string xmlFullname = this.RemoveContent(xmlFullnameWithTypes);

                string typ = string.Empty;
                MemberTyp typName = MemberTyp.None;
                if (xmlFullname.Contains(":") == true)
                {
                    typ = xmlFullname.Substring(0, 1);
                    if (typ == "T")
                    {
                        typName = MemberTyp.Class;
                    }
                    else if (typ == "M" && xmlFullname.Contains("#ctor") == true)
                    {
                        typName = MemberTyp.Construktor;
                    }
                    else if (typ == "M" && xmlFullname.Contains("#ctor") == false)
                    {
                        typName = MemberTyp.Methode;
                    }
                    else if (typ == "P")
                    {
                        typName = MemberTyp.Property;
                    }
                    else if (typ == "F")
                    {
                        typName = MemberTyp.Enum;
                    }
                }

                Tuple<string, MemberTyp> xmlTyp = new Tuple<string, MemberTyp>(typ, typName);
                string xmlName = element.Attribute("name").Value.Trim().Substring(2);
                xmlName = this.RemoveContent(xmlName).Replace("``1", string.Empty);

                string xmlSummary = element.Element("summary").Value.Trim();
                string xmlExample = element.Elements("example").Count() == 0 ? string.Empty : element.Element("example").Value.Trim();
                string xmlRemarks = element.Elements("remarks").Count() == 0 ? string.Empty : element.Element("remarks").Value.Trim();

                Dictionary<string,Tuple<string,string,string>> xmlMemberParams = new Dictionary<string, Tuple<string, string, string>>();
                if (element.Elements("param") != null && element.Elements("param").Count() > 0)
                {
                    Dictionary<XElement, string> paramsFromTag = new Dictionary<XElement, string>();
                    string parameterFull = xmlFullnameWithTypes.ExtractFromString("(", ")").Where(x => x != null).ToList()[0];
                    int countParameters = element.Elements("param").Count();
                    List<string> parameterList = new List<string>();

                    foreach (XElement item in element.Elements("param"))
                    {
                        string paramName = item.FirstAttribute.Value;
                    }

                    List<string> tempParameters = xmlFullnameWithTypes.ExtractFromString("(", ")").Where(x => x != null).ToList();
                    if (tempParameters[0].Contains("{") == true && tempParameters[0].Contains("}") == true)
                    {
                        parameterList = new List<string>();
                        parameterList.Add(tempParameters[0]);
                    }
                    else
                    {
                        parameterList = tempParameters[0].Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
                    }

                    foreach (KeyValuePair<XElement, string> item in paramsFromTag)
                    {
                        string paramName = item.Key.FirstAttribute.Value;
                        string paramDescription = ((XText)item.Key.FirstNode)?.Value;
                        string paramType = string.Empty;
                        string paramItemTyp = string.Empty;
                        if (item.Value.Contains("{") == true && item.Value.Contains("}") == true)
                        {
                            int firstPos = item.Value.LastIndexOf('{');
                            string paramTypeTemp = item.Value.Substring(0, firstPos);
                            int lastPos = paramTypeTemp.LastIndexOf('.') + 1;
                            paramType = paramTypeTemp.Substring(lastPos);
                            paramItemTyp = item.Value.ExtractFromString("{", "}").Where(x => x != null).ToList()[0];
                        }
                        else
                        {
                            int lastPos = item.Value.LastIndexOf('.') + 1;
                            paramType = item.Value.Substring(lastPos);
                        }
                        xmlMemberParams.Add(paramName, new Tuple<string, string, string>(paramType, paramItemTyp, paramDescription));
                }
            }

                string xmlReturn = string.Empty;
                if (element.Element("returns") != null)
                {
                    xmlReturn = element.Element("returns").Value.Trim();
                }

                MemberDocumentation mdoc = new MemberDocumentation()
                {
                    Assembly = asssembly,
                    FullName = xmlFullname,
                    FullNameWithTyp = xmlFullnameWithTypes,
                    MemberTyp = xmlTyp,
                    Summary = xmlSummary,
                    MemberParams = xmlMemberParams,
                    Returns = xmlReturn,
                    Remark = xmlRemarks,
                    Example = xmlExample
                };

                this.MemberSummaries[xmlFullnameWithTypes.Substring(2)] = mdoc;
            }
        }

        public IEnumerable<SourceDocumentation> Get()
        {
            List<SourceDocumentation> result = null;

            if (this.MemberSummaries != null && this.MemberSummaries.Count > 0)
            {
                result = new List<SourceDocumentation>();
                int positionInDoc = 0;
                foreach (KeyValuePair< string, MemberDocumentation> item in this.MemberSummaries)
                {
                    positionInDoc++;
                    SourceDocumentation sdoc = new SourceDocumentation();
                    sdoc.PositionInDoc = positionInDoc;
                    sdoc.Assembly = item.Value.Assembly;
                    result.Add(sdoc);
                }

            }

            return result;
        }

        /*
        public IEnumerable<SourceDocumentation> Get(Type classType)
        {
            List<SourceDocumentation> result = null;
            bool nextStep = false;

            try
            {
                MethodInfo[] memberInfos = classType.GetMethods()
                                      .Where(x => x.DeclaringType.FullName != "System.Object" && (x.Name.StartsWith("set_") == false))
                                      .ToArray();

                if (memberInfos != null)
                {
                    result = new List<SourceDocumentation>();

                    if (classType.IsClass == true && classType.IsPublic == true)
                    {
                        if (((TypeInfo)classType).DeclaredConstructors.Count() == 0)
                        {
                            SourceDocumentation sdoc = new SourceDocumentation();
                            sdoc.PositionInDoc = 1;
                            sdoc.Assembly = Path.GetFileNameWithoutExtension(classType.Assembly.Location);
                            sdoc.Namespace = classType.Namespace;
                            sdoc.Class = classType.Name;

                            string xmlName = $"{sdoc.Namespace}.{sdoc.Class}.#ctor";
                            if (this.MemberSummaries.ContainsKey(xmlName) == true)
                            {
                                sdoc.MemberTyp = this.MemberSummaries[xmlName].MemberTyp;
                                sdoc.Constructor = $"{sdoc.Class}()";
                                sdoc.Summary = this.MemberSummaries[xmlName].Summary;
                                sdoc.Remark = this.MemberSummaries[xmlName].Remark;
                                sdoc.Example = this.MemberSummaries[xmlName].Example == null ? string.Empty : this.MemberSummaries[xmlName].Example;
                                result.Add(sdoc);
                            }

                            nextStep = true;
                        }
                        else if (((TypeInfo)classType).DeclaredConstructors.Count() > 0)
                        {
                            foreach (ConstructorInfo constructorInfo in classType.GetConstructors())
                            {
                                SourceDocumentation sdoc = new SourceDocumentation();
                                sdoc.PositionInDoc = 1;
                                sdoc.Assembly = Path.GetFileNameWithoutExtension(classType.Assembly.Location);
                                sdoc.Namespace = classType.Namespace;
                                sdoc.Class = classType.Name;

                                if (constructorInfo.GetParameters().Count() == 0)
                                {
                                    string xmlName = $"{sdoc.Namespace}.{sdoc.Class}.#ctor";
                                    if (this.MemberSummaries.ContainsKey(xmlName) == true)
                                    {
                                        sdoc.MemberTyp = this.MemberSummaries[xmlName].MemberTyp;
                                        sdoc.Constructor = $"{sdoc.Class}()";
                                        sdoc.Summary = this.MemberSummaries[xmlName].Summary;
                                        sdoc.Remark = this.MemberSummaries[xmlName].Remark;
                                        sdoc.Example = this.MemberSummaries[xmlName].Example == null ? string.Empty : this.MemberSummaries[xmlName].Example;
                                        result.Add(sdoc);
                                    }
                                }
                                else
                                {
                                    string xmlName = $"{sdoc.Namespace}.{sdoc.Class}.#ctor(";
                                    Tuple<int, string, string> ctorParam = null;
                                    List<Tuple<int, string, string>> ctorParams = new List<Tuple<int, string, string>>();
                                    List<string> ctorArgs = new List<string>();
                                    foreach (ParameterInfo parameterInfo in constructorInfo.GetParameters())
                                    {
                                        int pos = parameterInfo.Position;
                                        string name = parameterInfo.Name;
                                        Type parameterType = parameterInfo.ParameterType;
                                        ctorArgs.Add(parameterType.FullName);
                                        ctorParam = new Tuple<int, string, string>(pos, name, parameterType.FullName);
                                        ctorParams.Add(ctorParam);
                                    }

                                    xmlName = xmlName + string.Join(",", ctorArgs) + ")";

                                    if (this.MemberSummaries.ContainsKey(xmlName) == true)
                                    {
                                        sdoc.MemberTyp = this.MemberSummaries[xmlName].MemberTyp;
                                        sdoc.Constructor = $"{sdoc.Class}({string.Join(",", ctorParams.Select(s => $"{s.Item3} {s.Item2}"))})";
                                        sdoc.Summary = this.MemberSummaries[xmlName].Summary;
                                        sdoc.MemberParams = this.MemberSummaries[xmlName].MemberParams;
                                        sdoc.Parameter = ctorParams;
                                        sdoc.Remark = this.MemberSummaries[xmlName].Remark;
                                        sdoc.Example = this.MemberSummaries[xmlName].Example == null ? string.Empty : this.MemberSummaries[xmlName].Example;
                                        result.Add(sdoc);
                                    }
                                }
                            }

                            nextStep = true;
                        }
                    }
                    else if (classType.IsValueType == true && classType.IsPublic == true)
                    {
                        SourceDocumentation sdoc = new SourceDocumentation();
                        sdoc.PositionInDoc = 1;
                        sdoc.Assembly = Path.GetFileNameWithoutExtension(classType.Assembly.Location);
                        sdoc.Namespace = classType.Namespace;
                        sdoc.Class = classType.Name;

                        string xmlName = $"{sdoc.Namespace}.{sdoc.Class}";
                        if (this.MemberSummaries.ContainsKey(xmlName) == true)
                        {
                            sdoc.MemberTyp = this.MemberSummaries[xmlName].MemberTyp;
                        }

                        Dictionary<string, int> enumContent = classType.GetEnumValues().Cast<object>().ToDictionary(k => k.ToString(), v => (int)v);

                        xmlName = $"{sdoc.Namespace}.{sdoc.Class}";
                        if (this.MemberSummaries.ContainsKey(xmlName) == true)
                        {
                            sdoc.MemberTyp = this.MemberSummaries[xmlName].MemberTyp;
                            sdoc.Member = xmlName;
                            sdoc.Summary = this.MemberSummaries[xmlName].Summary;
                            sdoc.Remark = this.MemberSummaries[xmlName].Remark;
                            sdoc.Example = this.MemberSummaries[xmlName].Example == null ? string.Empty : this.MemberSummaries[xmlName].Example;
                            result.Add(sdoc);
                        }

                        if (enumContent != null && enumContent.Count > 0)
                        {
                            foreach (KeyValuePair<string, int> item in enumContent)
                            {
                                xmlName = $"{sdoc.Namespace}.{sdoc.Class}.{item.Key}";
                                if (this.MemberSummaries.ContainsKey(xmlName) == true)
                                {
                                    SourceDocumentation enumDoc = new SourceDocumentation();
                                    enumDoc.Namespace = sdoc.Namespace;
                                    enumDoc.Class = sdoc.Class;
                                    enumDoc.Member = xmlName;
                                    enumDoc.MemberTyp = this.MemberSummaries[xmlName].MemberTyp;
                                    enumDoc.Summary = this.MemberSummaries[xmlName].Summary;
                                    enumDoc.Remark = this.MemberSummaries[xmlName].Remark;
                                    enumDoc.Example = this.MemberSummaries[xmlName].Example == null ? string.Empty : this.MemberSummaries[xmlName].Example;
                                    result.Add(enumDoc);
                                }
                            }
                        }
                    }

                    if (nextStep == true)
                    {
                        int position = 1;
                        foreach (MethodInfo mi in memberInfos)
                        {
                            position++;
                            string xmlName = this.GetKeyName(mi);
                            SourceDocumentation sdoc = new SourceDocumentation();
                            sdoc.PositionInDoc = position;
                            sdoc.Assembly = Path.GetFileNameWithoutExtension(classType.Assembly.Location);
                            sdoc.Namespace = classType.Namespace;
                            sdoc.Class = classType.Name;
                            sdoc.MemberTyp = this.MemberSummaries[xmlName].MemberTyp;
                            sdoc.Member = this.XMLMethod(mi);

                            Tuple<int, string, string> ctorParam = null;
                            List<Tuple<int, string, string>> ctorParams = new List<Tuple<int, string, string>>();
                            foreach (ParameterInfo parameterInfo in mi.GetParameters())
                            {
                                int pos = parameterInfo.Position;
                                string name = parameterInfo.Name;
                                Type parameterType = parameterInfo.ParameterType;
                                ctorParam = new Tuple<int, string, string>(pos, name, parameterType.FullName);
                                ctorParams.Add(ctorParam);
                            }

                            sdoc.MemberParams = this.MemberSummaries[xmlName].MemberParams;
                            sdoc.Parameter = ctorParams;
                            sdoc.Summary = this.MemberSummaries[xmlName].Summary;
                            sdoc.Return = new Tuple<string, string>(mi.ReturnType.FullName, this.MemberSummaries[xmlName].Returns);
                            sdoc.Remark = this.MemberSummaries[xmlName].Remark;
                            sdoc.Example = this.MemberSummaries[xmlName].Example == null ? string.Empty : this.MemberSummaries[xmlName].Example;
                            result.Add(sdoc);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return result.OrderBy(o => o.PositionInDoc);
        }
        */

        /// <summary>
        /// Return a pretty description of the full method
        /// </summary>
        /// <returns></returns>
        public string XMLMethod(MethodInfo mi, bool withNamespace = false)
        {
            string xmlName = this.GetKeyName(mi);

            string name = mi.DeclaringType.FullName + "." + mi.Name;
            if (mi.IsGenericMethod == true)
            {
                name += "<T>";
            }

            MemberDocumentation memberDoc = this.MemberSummaries.FirstOrDefault(f => f.Key == xmlName).Value;
            if (memberDoc != null)
            {
                name = memberDoc.FullName;
            }
            else
            {
                return string.Empty;
            }

            string paramType = string.Empty;
            string genericParam = string.Empty;
            List<string> paramLabels = new List<string>();
            int parameterCount = mi.GetParameters().Count();
            foreach (var p in mi.GetParameters())
            {
                paramType = p.ParameterType.Name;
                if (p.ToString().Contains("Nullable"))
                {
                    paramType = p.ToString().Split("[")[1].Split("]")[0];
                }

                // TODO: add more replacements for language shortcuts
                paramType = paramType.Replace("System.", "");
                if (paramType == "String")
                {
                    paramType = "string";
                }
                else if (paramType == "Double")
                {
                    paramType = "double";
                }
                else if (paramType == "Single")
                {
                    paramType = "float";
                }
                else if (paramType == "Byte")
                {
                    paramType = "byte";
                }
                else if (paramType == "Int32")
                {
                    paramType = "int";
                }
                else if (paramType == "Int64")
                {
                    paramType = "long";
                }
                else
                {
                    genericParam = string.Join(",", mi.GetParameters().Select(o => string.Format("{0}", o.ParameterType)).ToArray());
                }

                if (p.ToString().Contains("Nullable"))
                {
                    paramType += "?";
                }

                paramLabels.Add($"{paramType} {p.Name}");
            }

            string sig = string.Empty;
            if (memberDoc.MemberTyp.Item1 == "P")
            {
                sig = name.Substring(2);
            }
            else
            {
                sig = $"{name.Substring(2)}{this.IsGenericValue(genericParam)}({string.Join(", ", paramLabels)})";
            }

            if (withNamespace == false)
            {
                sig = sig.Replace($"{this.XmlClassNamespace(mi)}.", string.Empty).Replace($"{this.XmlClass(mi)}.", string.Empty);
            }

            return sig.Replace($"``{parameterCount}", string.Empty);
        }

        /// <summary>
        /// Die Methode gibt den Namespace der Klasse zurück
        /// </summary>
        /// <returns>String</returns>
        public string XmlClassNamespace(MethodInfo mi)
        {
            string result = string.Empty;

            result = mi.DeclaringType.Namespace;

            return result;
        }

        /// <summary>
        /// Die Methode gibt den Namespace der Klasse zurück
        /// </summary>
        /// <returns>String</returns>
        public string XmlClass(MethodInfo mi)
        {
            string result = string.Empty;

            result = mi.DeclaringType.Name;

            return result;
        }

        /// <summary>
        /// MemberParams
        /// </summary>
        public Dictionary<string,Tuple<string, string, string>> XmlParams(MethodInfo mi)
        {
            string xmlName = this.GetKeyName(mi);

            if (this.MemberSummaries.ContainsKey(xmlName) == true)
            {
                return this.MemberSummaries[xmlName].MemberParams;

            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Return-Inhalt zu einem Member
        /// </summary>
        /// <returns>String</returns>
        public Tuple<string, string> XmlReturn(MethodInfo mi)
        {
            string xmlName = this.GetKeyName(mi);

            if (this.MemberSummaries.ContainsKey(xmlName) == true)
            {
                string resturnTyp = string.Empty;
                resturnTyp = mi.ReturnType.Name;
                string returnContent = string.Empty;

                return new Tuple<string, string>(resturnTyp, this.MemberSummaries[xmlName].Returns);
            }
            else
            {
                return new Tuple<string, string>(string.Empty, "RETURN NOT FOUND!");
            }
        }

        /// <summary>
        /// Gibt den Inhalt zu einer Bemerkung 'Remarks' zurück
        /// </summary>
        /// <param name="mi">MethodInfo</param>
        /// <returns>Gibt einen String zurück</returns>
        public string XmlRemark(MethodInfo mi)
        {
            string xmlName = this.GetKeyName(mi);

            if (this.MemberSummaries.ContainsKey(xmlName) == true)
            {
                return this.MemberSummaries[xmlName].Remark;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gibt den Inhalt zu eineem Beispiel 'example' zurück
        /// </summary>
        /// <param name="mi">MethodInfo</param>
        /// <returns>Gibt einen String zurück</returns>
        public string XmlExample(MethodInfo mi)
        {
            string xmlName = this.GetKeyName(mi);

            if (this.MemberSummaries.ContainsKey(xmlName) == true)
            {
                return this.MemberSummaries[xmlName].Example;
            }
            else
            {
                return string.Empty;
            }
        }

        private string RemoveContent(string value)
        {
            string result = string.Empty;

            int pos1 = value.IndexOf('(');
            if (pos1 == -1)
            {
                return value;
            }

            return value.Substring(0, pos1);
        }

        /// <summary>
        /// Get the name used in XML documentation for a MethodInfo found using Reflection.
        /// </summary>
        private string GetKeyName(MethodInfo mi)
        {
            MethodInfo method = mi;
            string fullName = string.Format("{0}.{1}", method.ReflectedType.FullName, method.Name);
            StringBuilder sb = new StringBuilder();
            sb.Append(fullName);
            if (method.GetParameters().Count() > 0)
            {
                if (method.IsGenericMethod == true)
                {
                    sb.Append($"``{method.GetParameters().Count()}");
                    sb.Append("(");
                    foreach (var param in method.GetParameters())
                    {
                        sb.Append($"``{param.Position},");
                    }

                    sb.RemoveLastChar();
                    sb.Append(")");
                }
                else
                {
                    sb.Append("(");
                    foreach (var param in method.GetParameters())
                    {
                        if (param.ParameterType.FullName.Contains("nullable", StringComparison.CurrentCultureIgnoreCase) == true)
                        {
                            sb.Append($"System.Nullable{{{param.ParameterType.GenericTypeArguments[0].FullName}}}");
                            sb.Append(",");
                        }
                        else
                        {
                            sb.Append(param.ParameterType.FullName);
                            sb.Append(",");
                        }
                    }

                    sb.RemoveLastChar();
                    sb.Append(")");
                }
            }

            return sb.ToString().Replace("get_", string.Empty);
        }

        private string IsGenericValue(string value)
        {
            string result = string.Empty;

            if (string.IsNullOrWhiteSpace(value) == false)
            {
                return $"<{value}>";
            }
            else
            {
                return result;
            }
        }

        public IEnumerable<string> SplitTerm(string paramInput)
        {
            IEnumerable<string> result = null;
            List<Tuple<int, int, string>> paramPos = null;

            try
            {
                if (paramInput.StartsWith("(") == true && paramInput.EndsWith(")") == true)
                {
                    paramInput = paramInput.Replace("(", string.Empty).Replace(")", string.Empty);
                }

                if (string.IsNullOrEmpty(paramInput) == false)
                {
                    int posStart = 0;
                    int posEnd = 0;
                    bool termGeneric = false;
                    string typ = string.Empty;
                    paramPos = new List<Tuple<int, int, string>>();
                    for (int i = 0; i < paramInput.Length; i++)
                    {
                        if (paramInput[i] == '{')
                        {
                            posStart = i;
                            termGeneric = true;
                            typ = string.Empty;
                        }

                        if (paramInput[i] == '}')
                        {
                            posEnd = i;
                            termGeneric = false;
                            typ = string.Empty;
                        }

                        if (termGeneric == false && paramInput[i] == ',')
                        {
                            posStart = i;
                            posEnd = i;
                            typ = "C";
                        }

                        if (posStart > 0 && posEnd > 0)
                        {
                            Tuple<int, int, string> pos = new Tuple<int, int, string>(posStart, posEnd, typ);
                            paramPos.Add(pos);
                            posStart = 0;
                            posEnd = 0;
                            termGeneric = false;
                            typ = string.Empty;
                        }
                    }

                    int[] posIndex = paramPos.Where(w => w.Item3 == "C").Select(s => s.Item1).ToArray();
                    result = paramInput.SplitAt(',',posIndex);

                }
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        #region Dispose
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool classDisposing = false)
        {
            if (this.classIsDisposed == false)
            {
                if (classDisposing)
                {
                }
            }

            this.classIsDisposed = true;
        }

        #endregion Dispose
    }
}
