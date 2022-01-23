using Site13Kernel.Diagnostics.Errors;
using Site13Kernel.Diagnostics.Warns;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Site13Kernel.Diagnostics
{
    [Serializable]
    public class DebuggerConfiguration
    {
        public bool PreserveLog = false;
        public int LogThreshold = 500;
    }
    public class Debugger
    {
        public static Debugger CurrentDebugger = new Debugger();

        private const string Normal_Prefix = "[Normal]";
        private const string Warning_Prefix = "[Warning]";
        private const string Error_Prefix = "[Error]";

        DebuggerConfiguration CurrentConfiguration;


        List<Action<string, LogLevel>> Actions;
        List<Action> ClearBuffers;

        List<string> Content = new List<string>();
        ConcurrentQueue<(object, LogLevel)> Logs = new ConcurrentQueue<(object, LogLevel)>();
        Debugger()
        {
            CurrentConfiguration = new DebuggerConfiguration();
            Actions = new List<Action<string, LogLevel>>();
            ClearBuffers = new List<Action>();
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetConfiguration(DebuggerConfiguration configuration)
        {
            CurrentConfiguration = configuration;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void LSync()
        {
            lock (Logs)
            {
                Execute();
                Logs.Clear();
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void Execute()
        {
            while (!Logs.IsEmpty)
            {
                if (Logs.TryDequeue(out var log))
                {
                    var obj = log.Item1;
                    var logLevel = log.Item2;
                    string str = obj.ToString();
                    foreach (var item in Actions)
                    {
                        try
                        {
                            item(str, logLevel);
                        }
                        catch (Exception)
                        {
                        }
                    }
                    if (CurrentConfiguration.PreserveLog)
                    {
                        StringBuilder stringBuilder = new StringBuilder();
                        switch (logLevel)
                        {
                            case LogLevel.Normal:
                                stringBuilder.Append(Normal_Prefix);
                                break;
                            case LogLevel.Warning:
                                stringBuilder.Append(Warning_Prefix);
                                break;
                            case LogLevel.Error:
                                stringBuilder.Append(Error_Prefix);
                                break;
                            default:
                                break;
                        }
                        stringBuilder.Append(str);
                        Content.Add(stringBuilder.ToString());
                    }
                }

            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Log(object obj, LogLevel logLevel = LogLevel.Normal)
        {
            Logs.Enqueue((obj, logLevel));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Log(ISite13Error obj)
        {
            Logs.Enqueue((obj.ToString(), LogLevel.Error));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void LogError(object message, object Context)
        {
            Logs.Enqueue(($"Error on {{{Context}}}\r\n{message}", LogLevel.Error));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void LogError(object message)
        {
            Logs.Enqueue(($"Error: {message}", LogLevel.Error));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void LogWarning(object message, object Context)
        {
            Logs.Enqueue(($"Warning on {{{Context}}}\r\n{message}", LogLevel.Warning));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void LogWarning(object message)
        {
            Logs.Enqueue(($"Warning: {message}", LogLevel.Warning));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Log(ISite13Warn obj)
        {
            Logs.Enqueue((obj.ToString(), LogLevel.Warning));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CheckAndSaveLog()
        {
            if (Content.Count >= CurrentConfiguration.LogThreshold)
            {
                SaveLog();
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SaveLog()
        {

        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            if (ClearBuffers != null)
                foreach (var item in ClearBuffers)
                {
                    item();
                }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Register(Action<string, LogLevel> action)
        {
            Actions.Add(action);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Register(Action ClearBufferAction)
        {
            if (ClearBuffers != null)
                ClearBuffers.Add(ClearBufferAction);
        }
    }
    public enum LogLevel
    {
        Normal, Warning, Error
    }
}
