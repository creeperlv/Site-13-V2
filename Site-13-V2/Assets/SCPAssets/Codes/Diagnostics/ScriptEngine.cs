using CLUNL.Utilities;
using Site13Kernel.Diagnostics.Functions;

namespace Site13Kernel.Diagnostics
{
    public class ScriptEngine
    {
        public static void Execute(string cmd)
        {
            var result = CommandLineTool.Analyze(cmd);
            var a = result.RealParameter;
            Debugger.CurrentDebugger.Log($">{cmd}", LogLevel.Normal);
            var CMD = a[0].EntireArgument;
            if (FunctionCollection._func.ContainsKey(CMD))
            {
                var func = FunctionCollection._func[CMD];
                a.RemoveAt(0);
                func.Execute(a);
            }
            else
            {
                foreach (var item in FunctionCollection.Aliases)
                {
                    if (item.Value.Contains(CMD))
                    {
                        var func = FunctionCollection._func[item.Key];
                        a.RemoveAt(0);
                        func.Execute(a);
                        return;
                    }
                }
                if (a[0].EntireArgument != "?" && a[0].EntireArgument != "h" && a[0].EntireArgument != "help")
                    Debugger.CurrentDebugger.Log($"\"{a[0].EntireArgument}\" not found!", LogLevel.Warning);
                Debugger.CurrentDebugger.Log($"Available functinos:", LogLevel.Normal);
                foreach (var item in FunctionCollection._func.Keys)
                {
                    Debugger.CurrentDebugger.Log($"{item}", LogLevel.Normal);
                    Debugger.CurrentDebugger.Log($"Alias:", LogLevel.Normal);
                    foreach (var alia in FunctionCollection.Aliases[item])
                    {
                        Debugger.CurrentDebugger.Log($"\t{alia}", LogLevel.Normal);
                    }
                }
            }
        }
    }
}
