using System;
using System.Collections.Generic;
using System.Reflection;
using CommonLibs;

namespace CodeGenerator
{
    class GenerateDataModel
    {
        static string m_template_path = string.Empty;
        static string m_server_path = string.Empty;
        static string m_models_path = string.Empty;
        static string m_project_name = string.Empty;

        public static void InitPath(string template, string projectName, string server_path, string models_path)
        {
            m_template_path = template;
            m_server_path = server_path;
            m_models_path = models_path;
            m_project_name = projectName;
        }

        public static void GenerateFromData(Type[] types)
        {
            string repoStr = "";
            string modelStr = "";
            string modelPrject = string.Empty;

            for (int i = 0; i < types.Length; i++)
            {
                Type ptype = types[i];

                var proTypeName = ptype.FullName;

                modelPrject = ptype.FullName.Split(",")[0];

                proTypeName = Common.GetReturnTypeName(proTypeName);
                proTypeName = Common.GetSimpleTypeName(proTypeName);
                string className = proTypeName;

                string prefix = m_project_name + ".";
                if (className.StartsWith(prefix))
                    className = className.Substring(prefix.Length);

                modelStr += "                cfg.CreateMap<" + className + ", " + className + "Data>();\n";
                repoStr += "            services.AddTransient<IRepository<"+ className + ">, Repository<" + className + ", MainDbContext>>();\n";

                string dataStr = "";
                int idx = 0;

                var childMembers = ptype.GetProperties(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public);
                for (int j = 0; j < childMembers.Length; j++)
                {
                    var pMember = childMembers[j];
                    if (pMember.IsDefined(typeof(DataViewAttribute)))
                    {
                        var attributes = (DataViewAttribute)pMember.GetCustomAttribute(typeof(DataViewAttribute), false);

                        string mName = pMember.Name;
                        //字段类型
                        var mpType = pMember.PropertyType.FullName;
                        mpType = Common.GetReturnTypeName(mpType);
                        mpType = Common.GetSimpleTypeName(mpType);

                        string tips = attributes.Tips;
                        if (!string.IsNullOrEmpty(tips))
                        {
                            dataStr += "        /// <summary>\n";
                            dataStr += "        /// " + tips + "\n";
                            dataStr += "        /// </summary>\n";
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
                            dataStr += "        public " + mpType + " " + mName + " { get { return (" + mpType + ")" + subName + "; } }\n";
                        }
                    }
                }

                //ViewModels
                if (!string.IsNullOrEmpty(m_models_path))
                {
                    string vm_class = CodeCommon.GetTemplate(m_template_path, "ViewModel.txt");
                    vm_class = vm_class.Replace("#TypeName#", className);
                    vm_class = vm_class.Replace("#Datas#", dataStr);
                    vm_class = vm_class.Replace("#ProjectName#", m_project_name);

                    string vfileName = m_models_path + className + "Data.cs";
                    CodeCommon.WriteFile(vfileName, vm_class);
                }
            }

            //automapper
            string server_class = CodeCommon.GetTemplate(m_template_path, "ServerMapperModelsExtension.txt");
            server_class = server_class.Replace("#MapperData#", modelStr);
            server_class = server_class.Replace("#ProjectName#", m_project_name);

            string fileName = m_server_path + @"\Data\AutoMapperExtensions.cs";
            CodeCommon.WriteFile(fileName, server_class);

            //repository
            string repo_class = CodeCommon.GetTemplate(m_template_path, "RepositoryServiceExtensions.txt");
            repo_class = repo_class.Replace("#ModelProject#", modelPrject);
            repo_class = repo_class.Replace("#ProjectName#", m_project_name);
            repo_class = repo_class.Replace("#AddRepository#", repoStr);

            fileName = m_server_path + @"\Data\RepositoryServiceExtensions.cs";
            CodeCommon.WriteFile(fileName, repo_class);
        }
    }
}
