using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;

namespace CodeGenerator
{
    class CodeCommon
    {
        public static void ParseReturnType(Type methodReturnType, Type attriReturnType, string modelPrject, 
            ref string methodReturnTypeName, ref string returnTypeName, ref string mapperReturn, ref string innerType)
        {

            methodReturnTypeName = methodReturnType.FullName;
            bool isReturnData = methodReturnTypeName.Contains("ReturnData");
            bool needMapper = methodReturnTypeName.Contains(modelPrject);

            //使用返回值结构
            methodReturnTypeName = CodeCommon.GetReturnTypeName(methodReturnTypeName);
            methodReturnTypeName = CodeCommon.GetSimpleTypeName(methodReturnTypeName);
            //提取实际返回值类型
            returnTypeName = methodReturnTypeName;
            //获取<>前的类型
            innerType = returnTypeName;

            //api设定的返回类型
            if (attriReturnType != null)
            {
                returnTypeName = attriReturnType.FullName;
                returnTypeName = CodeCommon.GetReturnTypeName(returnTypeName);
                returnTypeName = CodeCommon.GetSimpleTypeName(returnTypeName);
            }
            //todo: 总是mapper类型
            else if (needMapper)
            {
                //获取List中的类型
                var mc = Regex.Match(returnTypeName, "[A-Za-z]*(?=>)");
                if (mc != null && mc.Length > 0)
                {
                    innerType = mc.Value + "Data";
                    //获取<>前的类型
                    mc = Regex.Match(returnTypeName, "[A-Za-z]*(?=<)");
                    if (mc != null && mc.Length > 0)
                        returnTypeName = mc.Value + "<" + innerType + ">";
                }
                else
                    returnTypeName = returnTypeName + "Data";
            }

            mapperReturn = "";
            if (needMapper)
            {
                if (isReturnData)
                {
                    var mc = Regex.Match(returnTypeName, "[A-Za-z]*(?=>)");
                    if (mc != null && mc.Length > 0)
                    {
                        mapperReturn += "var dataList = new " + returnTypeName + "();\n";
                        mapperReturn += "                    for (int i = 0; i < retData.Data.Count; i++)\n";
                        mapperReturn += "                        dataList.Add(Mapper.Map<" + innerType + ">(retData.Data[i]));\n";

                        mapperReturn += "                    var data = new ReturnData<" + returnTypeName + ">{\n";
                        mapperReturn += "                        ErrorCode = retData.ErrorCode,\n";
                        mapperReturn += "                        Data = dataList,\n";
                        mapperReturn += "                    };\n";
                    }
                    else
                    {
                        mapperReturn += "var data = new ReturnData<" + returnTypeName + ">{\n";
                        mapperReturn += "                    ErrorCode = retData.ErrorCode,\n";
                        mapperReturn += "                    Data = Mapper.Map<" + returnTypeName + ">(retData.Data),\n";
                        mapperReturn += "                };\n";
                    }
                }
                else
                {
                    var mc = Regex.Match(returnTypeName, "[A-Za-z]*(?=>)");
                    if (mc != null && mc.Length > 0)
                    {
                        mapperReturn += "var data = new " + returnTypeName + "();\n";
                        mapperReturn += "                    for (int i = 0; i < retData.Count; i++)\n";
                        mapperReturn += "                        data.Add(Mapper.Map<" + innerType + ">(retData[i]));\n";
                    }
                    else
                    {
                        mapperReturn += "var data = Mapper.Map<" + returnTypeName + ">(retData);\n";
                    }
                }
            }
            else
                mapperReturn += "var data = retData;\n";
        }

        public static string GetTemplate(string temp_path, string filename)
        {
            string temp = "";
            using (var stream = new FileStream(temp_path + filename, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(stream, Encoding.UTF8))
                {
                    temp = sr.ReadToEnd();
                }
            }
            return temp;
        }

        public static void WriteFile(string filename, string content)
        {
            var path = filename.Substring(0, filename.LastIndexOf(@"\"));
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            else
            {
                if (File.Exists(filename))
                    File.Delete(filename);
            }
            using (var sw = File.CreateText(filename))
            {
                sw.Write(content);
            }
            Console.WriteLine("Write ClientFile: " + filename);
        }

        public static bool CheckParamSocket(ParameterInfo paramInfo)
        {
            return paramInfo.Name.Equals("socket");
        }

        public static string GetReadString(string stype)
        {
            string ret = "";
            if (stype.Contains("System.Int32"))
                ret = "ReadInt";
            else if (stype.Contains("System.Int64"))
                ret = "ReadLong";
            else if (stype.Contains("System.String"))
                ret = "ReadString";
            else if (stype.Contains("System.Single"))
                ret = "ReadFloat";
            else if (stype.Contains("System.Double"))
                ret = "ReadDouble";
            else if (stype.Contains("System.Bool"))
                ret = "ReadBool";
            else
                ret = "ReadString";
            return ret;
        }

        public static string GetSimpleTypeName(string stype)
        {
            string ret = "";
            if (stype.Contains("Dictionary"))
            {
                string att_str = stype.Substring(stype.LastIndexOf("["));
                string[] atts = att_str.Split(',');
                if (atts != null && atts.Length >= 2)
                    //todo: 第二个参数为什么不对？
                    ret = "Dictionary<" + GetSimpleTypeName(atts[0]) + ", " + GetSimpleTypeName(atts[0]) + ">";
            }
            else if (stype.Contains("System.Collections.Generic.List`1"))
            {
                string clid_type = "int";
                if (stype.Contains("DateTime"))
                {
                    clid_type = "System.DateTime";
                }
                else
                {
                    Match mc = Regex.Match(stype, @"Models.[A-Za-z]*");
                    if (mc != null && mc.Length > 0)
                        clid_type = mc.Value.Substring("Models.".Length, mc.Value.Length - 0);
                }
                ret = "List<" + clid_type + "> ";
            }
            else if (stype.Contains("System.Int32[]"))
                ret = "int[]";
            else if (stype.Contains("System.Int32"))
                ret = "int";
            else if (stype.Contains("System.Int64"))
                ret = "long";
            else if (stype.Contains("System.Int"))
                ret = "int";
            else if (stype.Contains("System.String"))
                ret = "string";
            else if (stype.Contains("System.Single"))
                ret = "float";
            else if (stype.Contains("System.Double"))
                ret = "double";
            else if (stype.Contains("System.Bool"))
                ret = "bool";
            else
            {
                ret = stype.Replace("+", ".");
            }
            return ret;
        }

        public static string GetReturnTypeName(string methodReturnTypeName)
        {
            bool isCollections = false;

            string taskPrefix = "System.Threading.Tasks.Task`1";
            if (methodReturnTypeName.Contains(taskPrefix))
                methodReturnTypeName = methodReturnTypeName.Substring((taskPrefix + @"[[").Length);

            string returnDataPrefix = "[A-Za-z]*.ReturnData`1";
            var mc1 = Regex.Match(methodReturnTypeName, returnDataPrefix);
            if (mc1 != null && mc1.Length > 0)
                methodReturnTypeName = methodReturnTypeName.Substring((mc1.Value + @"[[").Length);

            returnDataPrefix = "[A-Za-z]*.ReturnMessage`1";
            mc1 = Regex.Match(methodReturnTypeName, returnDataPrefix);
            if (mc1 != null && mc1.Length > 0)
                methodReturnTypeName = methodReturnTypeName.Substring((mc1.Value + @"[[").Length);


            //是List<>
            string collectionsPrefix = "System.Collections.Generic.List`1";
            if (methodReturnTypeName.Contains(collectionsPrefix))
            {
                methodReturnTypeName = methodReturnTypeName.Substring((collectionsPrefix + @"[[").Length);
                isCollections = true;
            }

            methodReturnTypeName = TryParseTypeName(methodReturnTypeName);
            if (isCollections)
                methodReturnTypeName = string.Format("List<{0}>", methodReturnTypeName);
            return methodReturnTypeName;
        }

        static string TryParseTypeName(string methodReturnTypeName)
        {
            var mc = GetSubTypeName(ref methodReturnTypeName, @"Models\.[A-Za-z]*");
            if (mc == null || mc.Length == 0)
            {
                mc = GetSubTypeName(ref methodReturnTypeName, @"System\.[A-Za-z]*");
                if (mc == null || mc.Length == 0)
                {
                    GetSubTypeName(ref methodReturnTypeName, "");
                }
            }
            else
            {
                int start = "Models.".Length;
                methodReturnTypeName = mc.Value.Substring(start, mc.Value.Length - start);
            }
            return methodReturnTypeName;
        }
        static Match GetSubTypeName(ref string typeName, string pattern)
        {
            bool ret = false;
            var mc1 = Regex.Match(typeName, pattern);
            if (mc1 != null && mc1.Length > 0)
            {
                ret = true;
                typeName = mc1.Value;
            }
            return mc1;
        }

    }
}
