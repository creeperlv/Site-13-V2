using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Site13Kernel.Diagnostics
{
    [Serializable]
    public class DebuggerConfiguration
    {
        public bool PreserveLog=false;
        public int LogThreshold=500;
    }
    public class Debugger
    {
        public static Debugger CurrentDebugger=new Debugger();

        private const string Normal_Prefix = "[Normal]";
        private const string Warning_Prefix = "[Warning]";
        private const string Error_Prefix = "[Error]";

        DebuggerConfiguration CurrentConfiguration;


        List<Action<string,LogLevel>> Actions;
        
        List<string> Content=new List<string>();
        ConcurrentQueue<(object,LogLevel)> Logs=new ConcurrentQueue<(object,LogLevel)>();
        Debugger()
        {
            CurrentConfiguration = new DebuggerConfiguration();
            Actions = new List<Action<string, LogLevel>>();

        }
        public void SetConfiguration(DebuggerConfiguration configuration)
        {
            CurrentConfiguration = configuration;
        }
        public void LSync()
        {
            lock (Logs)
            {
                Execute();
                Logs.Clear();
            }
        }
        void Execute()
        {
            while (!Logs.IsEmpty)
            {
                if (Logs.TryDequeue(out var log))
                {
                    var obj=log.Item1;
                    var logLevel=log.Item2;
                    string str=obj.ToString();
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
                        StringBuilder stringBuilder=new StringBuilder();
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
        public void Log(object obj, LogLevel logLevel = LogLevel.Normal)
        {
            Logs.Enqueue((obj, logLevel));
        }
        public void CheckAndSaveLog()
        {
            if (Content.Count >= CurrentConfiguration.LogThreshold)
            {
                SaveLog();
            }
        }
        public void SaveLog()
        {

        }
        public void Register(Action<string, LogLevel> action)
        {
            Actions.Add(action);
        }
    }
    public enum LogLevel
    {
        Normal, Warning, Error
    }
}
