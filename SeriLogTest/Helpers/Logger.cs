using System;
using System.Diagnostics;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;

namespace SeriLogTest.Helpers
{
    class Logger
    {

        /// <summary>
        /// 
        /// Logger helper
        /// 
        /// Install packages in Package Manager Console:
        ///     Install-Package Serilog
        ///     Install-Package Serilog.Sinks.Console
        ///     Install-Package Serilog.Sinks.File
        ///     Install-Package Serilog.Enrichers.Environment
        ///     Install-Package Serilog.Exceptions
        ///     Install-Package Serilog.Filters.Expressions
        /// 
        /// In Program.cs include:
        ///     using Serilog;
        ///     using Serilog.Events;
        ///     using SeriLogTest.Helpers;
        /// 
        /// Than init logger:
        ///     Logger.InitLogger(separatefiles: true, separatedebugfile: true, filemaxlevel: LogEventLevel.Error, consolemaxlevel: LogEventLevel.Debug);
        ///     
        /// </summary>

        private const string logfile = "app_.log";                        // Path to store log files
        private const string debuglogfile = "dbg_.log";                   // Path to store debug files
        private const string errorlogfile = "err_.log";                   // Path to store error files
        private const string warninglogfile = "wrn_.log";                 // Path to store warning files
        private const string fatallogfile = "ftl_.log";                   // Path to store fatal files
        private const string informationlogfile = "inf_.log";             // Path to store information files
        private const string verboselogfile = "vrb_.log";                 // Path to store verbose files
        private const int filecountlimit = 10;                            // Maximum number of files
        private const int filesizelimit = 52428800;                       // File size limit in Bytes

        public static void InitLogger(string logdir = "", bool logtofile = false, bool separatefiles = false, bool separatedebugfile = false, LogEventLevel filemaxlevel = LogEventLevel.Error, LogEventLevel consolemaxlevel = LogEventLevel.Information)
        {
            logdir = System.IO.Directory.Exists(@logdir) ? System.IO.Path.GetFullPath(logdir) : "logs\\";
            Serilog.Debugging.SelfLog.Enable(msg => Debug.WriteLine(msg));
            Serilog.Debugging.SelfLog.Enable(Console.Error);
            var loggerConfiguration = new LoggerConfiguration()
                //.Enrich.WithMachineName()
                .MinimumLevel.Verbose()
                .Enrich.WithExceptionDetails()
                .WriteTo.Console(restrictedToMinimumLevel: consolemaxlevel);
                
            if (separatedebugfile)
            {
                loggerConfiguration.WriteTo.Logger(lc => lc.Filter.ByIncludingOnly("@Level = 'Debug'").WriteTo.File(System.IO.Path.Combine(logdir, debuglogfile), retainedFileCountLimit: filecountlimit, fileSizeLimitBytes: filesizelimit, rollOnFileSizeLimit: true, rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: LogEventLevel.Debug));
            }
            if (logtofile)
            {
                if (separatefiles)
                {
                    loggerConfiguration.WriteTo.Logger(lc => lc.Filter.ByIncludingOnly("@Level = 'Verbose'").WriteTo.File(System.IO.Path.Combine(logdir, verboselogfile), retainedFileCountLimit: filecountlimit, fileSizeLimitBytes: filesizelimit, rollOnFileSizeLimit: true, rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: filemaxlevel));
                    loggerConfiguration.WriteTo.Logger(lc => lc.Filter.ByIncludingOnly("@Level = 'Information'").WriteTo.File(System.IO.Path.Combine(logdir, informationlogfile), retainedFileCountLimit: filecountlimit, fileSizeLimitBytes: filesizelimit, rollOnFileSizeLimit: true, rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: filemaxlevel));
                    if (!separatedebugfile) loggerConfiguration.WriteTo.Logger(lc => lc.Filter.ByIncludingOnly("@Level = 'Debug'").WriteTo.File(System.IO.Path.Combine(logdir, debuglogfile), retainedFileCountLimit: filecountlimit, fileSizeLimitBytes: filesizelimit, rollOnFileSizeLimit: true, rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: LogEventLevel.Debug));
                    loggerConfiguration.WriteTo.Logger(lc => lc.Filter.ByIncludingOnly("@Level = 'Warning'").WriteTo.File(System.IO.Path.Combine(logdir, warninglogfile), retainedFileCountLimit: filecountlimit, fileSizeLimitBytes: filesizelimit, rollOnFileSizeLimit: true, rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: filemaxlevel));
                    loggerConfiguration.WriteTo.Logger(lc => lc.Filter.ByIncludingOnly("@Level = 'Error'").WriteTo.File(System.IO.Path.Combine(logdir, errorlogfile), retainedFileCountLimit: filecountlimit, fileSizeLimitBytes: filesizelimit, rollOnFileSizeLimit: true, rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: filemaxlevel));
                    loggerConfiguration.WriteTo.Logger(lc => lc.Filter.ByIncludingOnly("@Level = 'Fatal'").WriteTo.File(System.IO.Path.Combine(logdir, fatallogfile), retainedFileCountLimit: filecountlimit, fileSizeLimitBytes: filesizelimit, rollOnFileSizeLimit: true, rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: filemaxlevel));
                }
                else
                {
                    loggerConfiguration.WriteTo.File(System.IO.Path.Combine(logdir, logfile), retainedFileCountLimit: filecountlimit, fileSizeLimitBytes: filesizelimit, rollOnFileSizeLimit: true, rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: filemaxlevel);
                }
            }
            Log.Logger = loggerConfiguration.CreateLogger();
            if (logtofile) Log.Debug("Path to store logs: {0}", logdir);
        }
    }
}
