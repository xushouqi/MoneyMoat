using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Loader;
using System.Reflection;
using CommonLibs;

namespace CodeGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Process pInfo = Process.GetCurrentProcess();
            string filePath = pInfo.MainModule.FileName;
            if (!filePath.Contains("CodeGenerator"))
                filePath = Directory.GetCurrentDirectory();
            else
                filePath = filePath.Substring(0, filePath.LastIndexOf(@"\"));

            string basePath = filePath;
            int lastOfPath = filePath.LastIndexOf(@"\CodeGenerator");
            if (lastOfPath > 0)
                basePath = filePath.Substring(0, lastOfPath);

            Console.WriteLine("CodeGenerator.Start, args={0}, basePath={1}", args.Length, basePath);

            string template_path = basePath + @"\CodeGenerator\Template\";

            if (args.Length > 0)
            {
                string dllfile = args[0];

                //取项目名称
                int lastOfP = dllfile.LastIndexOf(@"\");
                string project_name = dllfile.Substring(lastOfP + 1, dllfile.LastIndexOf(".") - lastOfP - 1);

                string solutionPath = basePath;

                string server_path = "";
                string client_path = "";
                //string api_client_path = solutionPath + @"\ClientApiConnector\WebApi\";

                Assembly myAssembly = null;
                Assembly commonAssembly = null;
                Assembly modelsAssembly = null;

                var dllPath = dllfile.Substring(0, dllfile.LastIndexOf(@"\"));
                //加载同一目录下的所有dll
                var files = Directory.GetFiles(dllPath, "*.dll");
                foreach (var file in files)
                {
                    var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(file);
                    Console.WriteLine("AssemblyLoadContext dllfile={0}", file);

                    var filename = file.Substring(file.LastIndexOf(@"\") + 1);

                    //特别指出CommonLibs
                    if (filename.Contains("CommonLibs"))
                        commonAssembly = assembly;
                    //主程序
                    else if (filename.Contains("Models"))
                        modelsAssembly = assembly;
                    else if (filename.ToLower().Contains(project_name.ToLower()))
                        myAssembly = assembly;
                }

                Type[] types;
                try
                {
                    types = myAssembly.GetTypes();
                }
                catch (ReflectionTypeLoadException e)
                {
                    types = e.Types;
                }

                string serviceName = project_name + ".Services";
                Console.WriteLine("CodeGenerator types={0}, serviceName={1}", types.Length, serviceName);

                List<Type> socketTypeList = new List<Type>();
                List<Type> dataTypeList = new List<Type>();
                for (int i = 0; i < types.Length; i++)
                {
                    var myType = types[i];
                    if (myType != null)
                    {
                        var tName = myType.FullName;

                        //找到服务
                        //if (tName.ToLower().Contains(serviceName.ToLower()))
                        {
                            //恢复正确的大小写
                            project_name = tName.Split('.')[0];
                            server_path = solutionPath + @"\" + project_name;

                            if (myType.GetTypeInfo().IsDefined(commonAssembly.GetType(typeof(WebApiAttribute).FullName), false))
                            {
                                client_path = solutionPath + @"\ClientApiConnector\WebApi\";

                                GenerateWebApiConnector.InitPath(modelsAssembly, template_path, project_name, server_path, client_path);
                                GenerateWebApiConnector.GenerateFromService(myType);
                            }
                            if (myType.GetTypeInfo().IsDefined(typeof(WebSocketAttribute), false))
                            {
                                client_path = solutionPath + @"\ClientApiConnector\WebSocket\";

                                GenerateWebSocketClient.InitPath(template_path, project_name, server_path, client_path);
                                //GenerateWebSocketClient.GenerateFromService(myType);

                                socketTypeList.Add(myType);
                            }
                        }
                    }
                }

                if (socketTypeList.Count > 0)
                {
                    client_path = "";

                    GenerateWebSocketClasses.InitPath(template_path, project_name, server_path, client_path);
                    GenerateWebSocketClasses.GenerateFromService(socketTypeList.ToArray());
                }


                Type[] modelTypes;
                try
                {
                    modelTypes = modelsAssembly.GetTypes();
                }
                catch (ReflectionTypeLoadException e)
                {
                    modelTypes = e.Types;
                }
                for (int i = 0; i < modelTypes.Length; i++)
                {
                    var myType = modelTypes[i];
                    if (myType != null)
                    {
                        var tName = myType.FullName;

                        project_name = tName.Split('.')[0];

                        if (myType.GetTypeInfo().IsDefined(commonAssembly.GetType(typeof(DataModelsAttribute).FullName), false))
                        {
                            dataTypeList.Add(myType);
                        }
                    }
                }

                if (dataTypeList.Count > 0)
                {
                    string models_path = basePath + @"\"+ project_name + @"\ViewModels\";

                    GenerateDataModel.InitPath(template_path, project_name, server_path, models_path);
                    GenerateDataModel.GenerateFromData(dataTypeList.ToArray());
                }

            }
        }
    }
}