using Site13Kernel.Core;
using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace Site13Kernel.Diagnostics
{
    public static class DebugInfoFileWriter
    {
        /// <summary>
        /// Only call after Debugger.CurrentDebugger is inited.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Init()
        {
            CreateLogFile();
            Register();
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Register()
        {
            Debugger.CurrentDebugger.Register((C, L) =>
            {
                LogWriter.WriteLine(C);
            });
        }
        static FileInfo LogFile;
        static StreamWriter LogWriter;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CreateLogFile()
        {
            var NOW = DateTime.Now;
            var DP = Path.Combine(GameEnv.DataPath, "Site13Logs");
            var FP = Path.Combine(DP,$"{NOW.Year}-{NOW.Month}-{NOW.Day}--{NOW.Hour}-{NOW.Minute}-{NOW.Second}.log");
            if (!Directory.Exists(DP))
            {
                Directory.CreateDirectory(DP);
            }
            LogFile = new FileInfo(FP);
            if (LogFile.Exists)
            {
                LogFile.Delete();
            }
            LogWriter=LogFile.CreateText();
        }
    }
}
