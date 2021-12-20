using CLUNL.Utilities;
using Site13Kernel.Core.Controllers;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.Diagnostics.Functions
{
    public class Spawn : IDiagnosticsFunction
    {
        public void Execute(List<Argument> arguments)
        {
            if (arguments == null)
            {
                Help();
                return;
            }
            if (arguments.Count != 7)
            {
                Help();
                return;
            }
            if (arguments[0].EntireArgument.ToUpper() == "--HELP")
            {
                Help();
                return;
            }
            else
            {
                string ID = arguments[0].EntireArgument;
                float X;
                float Y;
                float Z;
                float R_X;
                float R_Y;
                float R_Z;
                if (!float.TryParse(arguments[1].EntireArgument, out X))
                {
                    Debugger.CurrentDebugger.LogError($"Invalid Argument: At parameter 1 ({arguments[1].EntireArgument}");
                    return;
                }
                if (!float.TryParse(arguments[2].EntireArgument, out Y))
                {
                    Debugger.CurrentDebugger.LogError($"Invalid Argument: At parameter 2 ({arguments[2].EntireArgument}");
                    return;
                }
                if (!float.TryParse(arguments[3].EntireArgument, out Z))
                {
                    Debugger.CurrentDebugger.LogError($"Invalid Argument: At parameter 3 ({arguments[3].EntireArgument}");
                    return;
                }
                if (!float.TryParse(arguments[4].EntireArgument, out R_X))
                {
                    Debugger.CurrentDebugger.LogError($"Invalid Argument: At parameter 4 ({arguments[4].EntireArgument}");
                    return;
                }
                if (!float.TryParse(arguments[5].EntireArgument, out R_Y))
                {
                    Debugger.CurrentDebugger.LogError($"Invalid Argument: At parameter 5 ({arguments[5].EntireArgument}");
                    return;
                }
                if (!float.TryParse(arguments[6].EntireArgument, out R_Z))
                {
                    Debugger.CurrentDebugger.LogError($"Invalid Argument: At parameter 6 ({arguments[6].EntireArgument}");
                    return;
                }
                Vector3 POS;
                POS.x = X;
                POS.y = Y;
                POS.z = Z;
                Vector3 ROT;
                ROT.x = R_X;
                ROT.y = R_Y;
                ROT.z = R_Z;
                _ = AIController.CurrentController.Spawn(ID, POS, ROT);
            }
        }

        public string GetCommandName()
        {
            return "spawn";
        }

        public void Help()
        {
            Debugger.CurrentDebugger.Log("spawn <BioEntityID:string> <x:float> <y:float> <z:float> <r_x:float> <r_y:float> <r_z:float> <identity:stirng>");
        }
    }

    public class ListBioDef : IDiagnosticsFunction
    {

        public void Execute(List<Argument> arguments)
        {
            foreach (var item in GlobalBioController.CurrentGlobalBioController.BioDefinitions)
            {
                Debugger.CurrentDebugger.Log($"{item.Key} -> {item.Value}");
            }
        }
        public string GetCommandName()
        {
            return "listbiodef";
        }
        public List<string> GetAlias()
        {
            return new List<string> { GetCommandName(), "ls-bio-def" };
        }

        public void Help()
        {
            Debugger.CurrentDebugger.Log("listbiodef");
        }
    }
}
