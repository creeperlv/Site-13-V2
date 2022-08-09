using CLUNL.ConsoleAppHelper;
using Site13Kernel.GameLogic.BT.Serialization;
using Site13Kernel.Utilities;

namespace BTNodeCompiler
{
    [DependentFeature("BTNode", "Build", Description = "Build the behavior tree nodes made by editor to Site-13 Kernel compatible JSON/binary",
        Options = new string[] { "O,OUT", "T,TYPE" } , OptionDescriptions =new string[] {"Output file.","Target compile type, available types: JSON, BINARY. JSON is the default type." })]
    public class Build : IFeature
    {
        public void Execute(ParameterList Parameters, string MainParameter)
        {
            string? T = "";
            string? O = "";
            if ((O = Parameters.Query<string>("O")) != null)
            {
            }
            else O = "";
            if (MainParameter == null)
            {
                Output.OutLine(new ErrorMsg { ID = "BT.ERR.0", Fallback = "A graph file must be specified." });
                return;
            }
            if (MainParameter == "")
            {
                Output.OutLine(new ErrorMsg { ID = "BT.ERR.0", Fallback = "A graph file must be specified." });
                return;
            }
            int Type = 0;
            if ((T = Parameters.Query<string>("T")) != null)
            {
            }
            else
            {
                Output.OutLine(new WarnMsg { ID = "BT.WARN.1", Fallback = "Type not specified, using JSON." });
                T = "JSON";
            }
            switch (T.ToUpper())
            {
                case "B":
                case "BIN":
                case "BINARY":
                    {
                        Type = 1;
                    }
                    break;
                default:
                    break;
            }
            if (O == "")
            {
                if (Type == 0)
                {
                    Output.OutLine(new WarnMsg { ID = "BT.WARN.0", Fallback = "Output not specified, using original file name + .site13.json instead." });
                    O = MainParameter + ".site13.json";
                }
                else
                if (Type == 1)
                {

                    Output.OutLine(new WarnMsg { ID = "BT.WARN.1", Fallback = "Output not specified, using original file name + .site13.bytes instead." });
                    O = MainParameter + ".site13.bytes";
                }
            }
            if (!File.Exists(MainParameter))
            {
                Output.OutLine(new ErrorMsg { ID = "BT.ERR.1", Fallback = "Specified graph file does not exist!" });
                return;
            }
            if (File.Exists(O))
            {
                Output.OutLine(new WarnMsg { ID = "BT.WARN.2", Fallback = "Target output file has already existed, deleting old one to overwrite." });
                File.Delete(O);
            }
            Output.OutLine("BT.INFO.0", "Loading graph...");
            var SG = JsonUtilities.Deserialize<SerializableGraph>(File.ReadAllText(MainParameter));
            Output.OutLine("BT.INFO.1", "Building Node...");
            var __node = SG.Build();
            switch (Type)
            {
                case 0:
                    Output.OutLine("BT.INFO.1", "Convert to JSON...");
                    File.WriteAllText(O, JsonUtilities.Serialize(__node));
                    break;
                case 1:
                    Output.OutLine("BT.INFO.2", "Convert to binary file...");
                    File.WriteAllBytes(O, BinaryUtilities.Serialize(__node));
                    break;
                default:
                    break;
            }
            Output.OutLine("BT.INFO.3", "Done.");
        }
    }

}