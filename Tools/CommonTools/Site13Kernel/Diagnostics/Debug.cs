using Site13Kernel.Diagnostics.Errors;
using Site13Kernel.Diagnostics.Warns;
using System.Runtime.CompilerServices;

namespace Site13Kernel.Diagnostics
{
    public static class Debug
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LSync()
        {
            Debugger.CurrentDebugger.LSync();
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Log(object obj, LogLevel logLevel = LogLevel.Normal)
        {
            Debugger.CurrentDebugger.Log(obj, logLevel);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Log(ISite13Error obj)
        {
            Debugger.CurrentDebugger.Log(obj);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LogError(object message, object Context)
        {
            Debugger.CurrentDebugger.LogError(message, Context);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LogError(object message)
        {
            Debugger.CurrentDebugger.LogError(message);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LogWarning(object message, object Context)
        {
            Debugger.CurrentDebugger.LogWarning(message, Context);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LogWarning(object message)
        {
            Debugger.CurrentDebugger.LogWarning(message);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Log(ISite13Warn obj)
        {
            Debugger.CurrentDebugger.Log(obj);
        }
    }
}
