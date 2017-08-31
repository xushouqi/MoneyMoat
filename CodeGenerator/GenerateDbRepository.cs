using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.IO;
using CommonLibs;

namespace CodeGenerator
{
    class GenerateDbRepository
    {
        static string m_template_path = string.Empty;
        static string m_server_path = string.Empty;
        static string m_model_path = string.Empty;
        static string m_type_name = string.Empty;
        static string m_project_name = string.Empty;

        public static void InitPath(string template, string projectname, string server_path, string model_path)
        {
            m_project_name = projectname;
            m_template_path = template;
            m_server_path = server_path;
            m_model_path = model_path;
        }

        public static void GenerateFromDbContext<T>()
        {
            System.Type vType = typeof(T);
            m_type_name = vType.Name;

            string classTemplate = CodeCommon.GetTemplate(m_template_path, "ServerRepositoryExtension.txt");
            string funcTemplate = CodeCommon.GetTemplate(m_template_path, "ServerRepositoryFunc.txt");
            string vmTemplate = CodeCommon.GetTemplate(m_template_path, "ViewModel.txt");
            string mapperTemplate = CodeCommon.GetTemplate(m_template_path, "ServerMapperModelsExtension.txt");

            string funcStr = "";
            string modelStr = "";

            //方法生成请求接口
            var vPropertiesInfos = vType.GetProperties(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public);
            for (int i = 0; i < vPropertiesInfos.Length; i++)
            {
                var vPropertyInfo = vPropertiesInfos[i];
                if (vPropertyInfo.PropertyType.Name.Contains("DbSet"))
                {
                    string propertyName = vPropertyInfo.Name;
                    var proTypeName = vPropertyInfo.PropertyType.FullName;
                    proTypeName = Common.GetReturnTypeName(proTypeName);
                    proTypeName = Common.GetSimpleTypeName(proTypeName);
                    string className = proTypeName;
                    if (className.StartsWith("GodModels."))
                        className = className.Substring("GodModels.".Length);

                    modelStr += "                cfg.CreateMap<"+ className + ", "+ className + "Data>();\n";

                    //注册数据仓库
                    string funcfile = funcTemplate;
                    funcfile = funcfile.Replace("#Type#", proTypeName);
                    funcfile = funcfile.Replace("#PropertyName#", propertyName);
                    funcStr += funcfile;

                    string dataStr = "";
                    int idx = 0;
                    //Model里的字段
                    var an = new AssemblyName("GodModels");
                    var ptype = Assembly.Load(an).GetType(proTypeName);
                    var childMembers = ptype.GetProperties(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public);
                    for (int j = 0; j < childMembers.Length; j++)
                    {
                        var pMember = childMembers[j];
                        string mName = pMember.Name;
                        //字段类型
                        var mpType = pMember.PropertyType.FullName;
                        mpType = Common.GetReturnTypeName(mpType);
                        mpType = Common.GetSimpleTypeName(mpType);

                        if (pMember.IsDefined(typeof(GodDataAttribute)))
                        {
                            var attributes = (GodDataAttribute)pMember.GetCustomAttribute(typeof(GodDataAttribute), false);
                            string tips = attributes.Tips;
                            if (!string.IsNullOrEmpty(tips))
                            {
                                dataStr += "        /// <summary>\n";
                                dataStr += "        /// " + tips + "\n";
                                dataStr += "        /// </summary>\n";
                            }
                        }
                        idx++;
                        if (pMember.CanWrite)
                        {
                            dataStr += "        [ProtoMember(" + idx + ")]\n";
                            dataStr += "        public " + mpType + " " + mName + " { get; set; }\n";
                        }
                        else
                        {
                            string subName = mName;
                            if (subName.Contains("My"))
                                subName = subName.Substring(2);
                            else if (subName.Contains("Cur"))
                                subName = subName.Substring(3);
                            dataStr += "        public " + mpType + " " + mName + " { get { return ("+mpType+")"+ subName + "; } }\n";
                        }
                    }

                    //ViewModels
                    string vm_class = vmTemplate;
                    vm_class = vm_class.Replace("#TypeName#", className);
                    vm_class = vm_class.Replace("#Datas#", dataStr);
                    string vfileName = m_model_path + @"\ViewModels\"+ className + "Data.cs";
                    CodeCommon.WriteFile(vfileName, vm_class);
                }
            }

            //注册数据仓库
            string server_class = classTemplate;
            server_class = server_class.Replace("#RepositoryFunc#", funcStr);
            string fileName = m_server_path + @"\Repositories\RepositoryServiceExtensions.cs";
            CodeCommon.WriteFile(fileName, server_class);

            //automapper
            server_class = mapperTemplate;
            server_class = server_class.Replace("#MapperData#", modelStr);
            fileName = m_server_path + @"\Repositories\AutoMapperExtensions.cs";
            CodeCommon.WriteFile(fileName, server_class);

        }

    }
}
