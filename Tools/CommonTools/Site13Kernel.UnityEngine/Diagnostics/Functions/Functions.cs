
using CLUNL.Utilities;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Site13Kernel.Diagnostics.Functions
{
    /// <summary>
    /// Debugger will find functions according to this 
    /// </summary>
    public interface IDiagnosticsFunction
    {
        List<string> GetAlias()
        {
            return new List<string> { GetCommandName() };
        }
        string GetCommandName();
        void Help();
        void Execute(List<Argument> arguments);
    }
    public class FunctionCollection
    {
        public static Dictionary<string, IDiagnosticsFunction> _func=new Dictionary<string, IDiagnosticsFunction>();
        public static Dictionary<string,List<string>> Aliases=new Dictionary<string, List<string>>();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GatherFunctions()
        {
            {
                //Init functions.
                var DLLS=AppDomain.CurrentDomain.GetAssemblies();
                foreach (var DLL in DLLS)
                {
                    foreach (var t in DLL.GetTypes())
                    {
                        if (t.GetInterface("IDiagnosticsFunction") != null)
                        {
                            var func=(IDiagnosticsFunction) Activator.CreateInstance(t);
                            var _NAME=func.GetCommandName();
                            {
                                if (!_func.ContainsKey(_NAME))
                                    _func.Add(_NAME, func);
                                else
                                    _func[_NAME] = func;
                            }
                            var ALIAS=func.GetAlias();
                            {
                                if (!Aliases.ContainsKey(_NAME))
                                    Aliases.Add(_NAME, ALIAS);
                                else
                                    Aliases[_NAME].AddRange(ALIAS);
                            }
                        }
                    }
                }
            }
        }
    }
    public class OutPut : IDiagnosticsFunction
    {
        private const string BLANK = " ";

        public void Execute(List<Argument> arguments)
        {
            StringBuilder builder=new StringBuilder();
            for (int i = 0; i < arguments.Count; i++)
            {
                builder.Append(arguments[i].EntireArgument);
                if (i != arguments.Count - 1)
                {
                    //Last One
                }
                else
                {
                    builder.Append(BLANK);
                }
            }
            Debugger.CurrentDebugger.Log(builder.ToString());
        }

        public string GetCommandName()
        {
            return "Write-Output";
        }

        public void Help()
        {
            Debugger.CurrentDebugger.Log("Write-Output \"Content...\"");
        }
        public List<string> GetAlias()
        {
            return new List<string> { "echo", "Console.WriteLine" };
        }
    }
}
