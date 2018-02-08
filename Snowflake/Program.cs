#region Test1
//using System;

//namespace Twitter_Snowflake
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            for (int i = 0; i < 1000; i++)
//            {
//                Console.WriteLine($"开始执行 {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffffff")}------{Snowflake.Instance().GetId()} \n");
//            }
//            Console.ReadKey();
//        }
//    }
//} 
#endregion

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Snowflake
{
    class Program
    {
        private static int N = 10000;
        private static readonly HashSet<long> Set = new HashSet<long>();
        private static readonly Snowflake Worker = new Snowflake(1, 1);
        private static int _taskCount;

        static void Main(string[] args)
        {
            Task.Run(() => GetId());
            Task.Run(() => GetId());
            Task.Run(() => GetId());

            Task.Run(() => Printf());
            Console.ReadKey();
        }

        private static void Printf()
        {
            while (_taskCount != 3)
            {
                Console.WriteLine("...");
                Thread.Sleep(1000);
            }
            lock (O)
            {
                Console.WriteLine(Set.Count == N * _taskCount);
            }
        }

        private static readonly object O = new object();
        private static void GetId()
        {
            for (var i = 0; i < N; i++)
            {
                var id = Worker.GetId();

                lock (O)
                {
                    if (Set.Contains(id))
                    {
                        Console.WriteLine(_taskCount + "发现重复项 : {0}", id);
                    }
                    else
                    {
                        Set.Add(id);
                    }
                }

            }
            lock (O)
            {
                Console.WriteLine($"任务{++_taskCount}完成");
            }
        }
    }
}