using System;
using System.Diagnostics;
using Serilog;
using Serilog.Events;
using SeriLogTest.Helpers;

namespace SeriLogTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.InitLogger(logtofile: true, separatefiles: true, separatedebugfile: true, filemaxlevel: LogEventLevel.Verbose, consolemaxlevel: LogEventLevel.Verbose); ;
            Console.WriteLine("Hello World!");
            Log.Information("Hello, Serilog!");
            Log.Warning("Goodbye, Serilog.");
            test();
            Console.ReadKey();
        }

        static void test()
        {
            Log.Debug("----Debug level---------");
            Debug.WriteLine("------TEST-------------");
            Log.Warning("Warning level");
            Log.Fatal("Fatal level");
            Log.Error("Error level");
            Log.Information("Test");
            Log.Verbose("Test");
            try
            {
                int i = 0;
                var f = 23 / i;
            } catch (Exception ex)
            {
                Log.Error(ex, "Exception test");
            }
        }
    }
}
