using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PowerArgs;

namespace TestClient
{
    // A class that describes the command line arguments for this program
    public class MyArgs
    {
        // This argument is required and if not specified the user will 
        // be prompted.
        [ArgRequired(PromptIfMissing = true)]
        [ArgPosition(0)]
        public string StringArg { get; set; }

        // This argument is not required, but if specified must be >= 0 and <= 60
        [ArgRange(0, 60)]
        [ArgPosition(1)]
        [ArgRequired(PromptIfMissing = true)]
        public int IntArg { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            try
            {
                var parsed = Args.Parse<MyArgs>(args);
                Console.WriteLine("You entered string '{0}' and int '{1}'", parsed.StringArg, parsed.IntArg);
            }
            catch (ArgException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ArgUsage.GenerateUsageFromTemplate<MyArgs>());
            }

            int count = 10;
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
