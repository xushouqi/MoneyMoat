using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            int count = 1;
            if (args.Length > 0)
                count = int.Parse(args[0]);

            List<Task> tasklist = new List<Task>();
            for (int i = 0; i < count; i++)
            {
                int idx = i + 1300;
                Task work = new Task(new Work(idx).Initial);
                //Task work = new Task(new SingletonWork(idx).StartWithRegister);
                work.Start();
                tasklist.Add(work);
            }
            Task.WaitAll(tasklist.ToArray());
            Console.WriteLine("Finish!");
            Console.ReadKey();
        }
    }
}
