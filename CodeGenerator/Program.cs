using System;
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

                for (int i = 0; i < types.Length; i++)
                {
                    var myType = types[i];
                    if (myType != null)
                    {
                        var tName = myType.FullName;

                        //找到服务
                        if (tName.ToLower().Contains(serviceName.ToLower()))
                        {
                            //恢复正确的大小写
                            project_name = tName.Split('.')[0];

                            //Console.WriteLine("CodeGenerator tName={0}", tName);
                            
                            if (myType.GetTypeInfo().IsDefined(commonAssembly.GetType(typeof(WebApiAttribute).FullName), false))
                            {
                                server_path = solutionPath + @"\" + project_name + @"\Controllers\";
                                client_path = "";

                                Console.WriteLine("CodeGenerator server_path={0}", server_path);

                                GenerateWebApiConnector.InitPath(template_path, project_name, server_path, client_path);
                                GenerateWebApiConnector.GenerateFromService(myType);
                            }
                            if (myType.GetTypeInfo().IsDefined(typeof(WebSocketAttribute), false))
                            {
                                server_path = solutionPath + @"\" + project_name + @"\Controllers\";
                                client_path = "";

                                Console.WriteLine("CodeGenerator server_path={0}", server_path);

                                GenerateWebSocketClient.InitPath(template_path, project_name, server_path, client_path);
                                //GenerateWebSocketClient.GenerateFromService(myType);
                            }
                        }
                    }
                }
            }
        }

    }
}