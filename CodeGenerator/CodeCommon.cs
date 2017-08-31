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
        public static void WriteFileRegisterModule(string project_name, string template_path, string server_path, string server_registers)
        {
            string server_regmodule = GetTemplate(template_path, "RegisterModule.txt");
            server_regmodule = server_regmodule.Replace("#Action#", server_registers);
            server_regmodule = server_regmodule.Replace("#ProjectName#", project_name);
            string regFileName = server_path + "RegisterModule.cs";
            WriteFile(regFileName, server_regmodule);
        }

        public static void WriteFilePushManager(string project_name, string template_path, string server_path, string server_regActionidByType)
        {
            string server_regmodule = GetTemplate(template_path, "TeamPushManager.txt");
            server_regmodule = server_regmodule.Replace("#RegActionIdByType#", server_regActionidByType);
            string regFileName = server_path + "TeamPushManager.cs";
            WriteFile(regFileName, server_regmodule);
        }

        public static void WriteNetworkClient(string template_path, string client_path, string defineClient, string declareClient)
        {
            string server_regmodule = GetTemplate(template_path, "NetworkClient.txt");
            server_regmodule = server_regmodule.Replace("#DefineClient#", defineClient);
            server_regmodule = server_regmodule.Replace("#DeclareClient#", declareClient);
            string regFileName = client_path + "NetworkClient.cs";
            WriteFile(regFileName, server_regmodule);
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

        public static void WriteFile(string filepath, string content)
        {
            if (File.Exists(filepath))
                File.Delete(filepath);
            using (var sw = File.CreateText(filepath))
            {
                sw.Write(content);
            }
            Console.WriteLine("Write ClientFile: " + filepath);
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
                    Match mc = Regex.Match(stype, @"ZeroModels.[A-Za-z]*");
                    if (mc != null && mc.Length > 0)
                        clid_type = mc.Value.Substring(0, mc.Value.Length - 0);
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
            string returnDataPrefix = "GodModels.ReturnData`1";
            string collectionsPrefix = "System.Collections.Generic.List`1";
            string taskPrefix = "System.Threading.Tasks.Task`1";

            bool isCollections = false;

            if (methodReturnTypeName.Contains(taskPrefix))
                methodReturnTypeName = methodReturnTypeName.Substring((taskPrefix + @"[[").Length);

            if (methodReturnTypeName.Contains(returnDataPrefix))
                methodReturnTypeName = methodReturnTypeName.Substring((returnDataPrefix + @"[[").Length);

            //是List<>
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
            if (!GetSubTypeName(ref methodReturnTypeName, @"GodModels\.[A-Za-z]*"))
            {
                if (!GetSubTypeName(ref methodReturnTypeName, @"System\.[A-Za-z]*"))
                {
                    GetSubTypeName(ref methodReturnTypeName, "");
                }
            }
            return methodReturnTypeName;
        }
        static bool GetSubTypeName(ref string typeName, string pattern)
        {
            bool ret = false;
            var mc1 = Regex.Match(typeName, pattern);
            if (mc1 != null && mc1.Length > 0)
            {
                ret = true;
                typeName = mc1.Value;
            }
            return ret;
        }

    }
}
