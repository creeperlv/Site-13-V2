using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using BTNodeEditor.Editors.Nodes;
using Site13Kernel.GameLogic.BT.Nodes.Generic;
using Site13Kernel.GameLogic.BT.Serialization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BTNodeEditor.Editors
{
    public partial class NodeGraphEditor : UserControl
    {
        public NodeGraphEditor()
        {
            InitializeComponent();

            Task.Run(async () =>
            {
                await Task.Delay(100);
                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    foreach (var item in ManagedConnections)
                    {
                        item.CalculatePath();
                    }
                });
            });
            AdornerCanvas.PointerPressed += (_, _) =>
            {
                if (__is_operating_NC)
                {
                    __is_operating_NC = false;
                    if (NC is not null)
                        RemoveConnection(NC);
                    NC = null;
                }
            };
            AdornerCanvas.PointerMoved += (_, b) =>
            {
                if (__is_operating_NC)
                {
                    {
                        if (i % 5 == 0)
                        {
                            var P = b.GetPosition(AdornerCanvas);
                            if (NC_R)
                            {
                                NC!.End = P;
                            }
                            else
                            {
                                NC!.Start = P;
                            }
                        }
                        i++;
                    }
                }
            };
        }
        public void AddStartNode()
        {
            AddNode(new SerializableNode()
            {
                X = 150,
                Y = 200,
                ID = Guid.NewGuid().ToString(),
                Contained = new Start(),
                HaveL = false,
                DisableDeletion = true,
                DisableDuplicate = true
            });
        }
        public Dictionary<string, GraphNode> ID_Node_Map = new();
        public List<GraphNode> Nodes = new();
        public void DeleteNode(GraphNode GN)
        {
            AdornerCanvas.Children.Remove(GN);
            for (int i = GN.L.Count - 1; i >= 0; i--)
            {
                var item = GN.L[i];
                RemoveConnection(item);
            }
            for (int i = GN.R.Count - 1; i >= 0; i--)
            {
                var item = GN.R[i];
                RemoveConnection(item);
            }
            //foreach (var item in GN.R)
            //{
            //    RemoveConnection(item);
            //}
            Nodes.Remove(GN);
        }
        public void HideAll()
        {
            foreach (var item in Nodes)
            {
                item.TitleButton.IsChecked = false;
            }
        }
        public void ShowAll()
        {
            foreach (var item in Nodes)
            {
                item.TitleButton.IsChecked = true;
            }
        }
        public void RemoveConnection(NodeConnection NC)
        {
            if (NC.L is not null) NC.L.RemoveR(NC);
            if (NC.R is not null) NC.R.RemoveL(NC);
            ManagedConnections.Remove(NC);
            AdornerCanvas.Children.Remove(NC.GetPath());
        }
        public GraphNode AddNode(SerializableNode SN)
        {
            var GN = GraphNode.FromSerializableNode(SN, this);
            Nodes.Add(GN);
            GN.Margin = new Thickness(SN.X, SN.Y);
            ID_Node_Map.Add(SN.ID, GN);
            AdornerCanvas.Children.Add(GN);
            foreach (var item in SN.L)
            {
                if (ID_Node_Map.TryGetValue(item, out var L))
                {
                    Connect(L, GN);
                }
            }
            foreach (var item in SN.R)
            {
                if (ID_Node_Map.TryGetValue(item, out var R))
                {
                    Connect(GN, R);
                }
            }
            return GN;
        }
        public void LoadSerializableGraph(SerializableGraph SG)
        {

            Dictionary<string, GraphNode> id_node_map = new Dictionary<string, GraphNode>();
            foreach (var item in SG.nodes)
            {
                var GN = AddNode(item);
                id_node_map.Add(item.ID, GN);
            }
            foreach (var item in SG.nodes)
            {
                if (id_node_map.TryGetValue(item.ID, out var op_node))
                {
                    foreach (var L in item.L)
                    {
                        if (id_node_map.TryGetValue(L, out var node_1))
                        {
                            Connect(node_1, op_node);
                        }
                    }
                    foreach (var R in item.R)
                    {
                        if (id_node_map.TryGetValue(R, out var node_1))
                        {
                            Connect(op_node, node_1);
                        }
                    }
                }
            }
        }
        public SerializableGraph ToSerializableGraph()
        {
            SerializableGraph SG = new SerializableGraph();
            foreach (var item in Nodes)
            {
                var SN = item.Serialize();
                {
                    SN.R.Clear();
                    foreach (var con in item.R)
                    {
                        if (con.R is not null)
                        {
                            SN.R.Add(con.R.Serialize().ID);
                        }
                    }
                }
                {
                    SN.L.Clear();
                    foreach (var con in item.L)
                    {
                        if (con.L is not null)
                        {
                            SN.L.Add(con.L.Serialize().ID);
                        }
                    }
                }
                SG.nodes.Add(SN);
            }
            return SG;
        }
        public void Connect(GraphNode L, GraphNode R)
        {
            foreach (var item in L.R)
            {
                if (item.R == R)
                {
                    return;
                }
            }
            foreach (var item in R.L)
            {
                if (item.L == L)
                {
                    return;
                }
            }
            NodeConnection nodeConnection = new NodeConnection(this);
            L.AddR(nodeConnection);
            R.AddL(nodeConnection);
            ManagedConnections.Add(nodeConnection);
            AdornerCanvas.Children.Insert(0, nodeConnection.GetPath());
        }
        int i = 0;
        bool __is_operating_NC;
        bool NC_R = false;
        NodeConnection? NC = null;
        List<NodeConnection> ManagedConnections = new();
        GraphNode? _LastGN;
        public void PressFromGraphNode(GraphNode n, bool isRightOfNode)
        {
            i = 0;
            if (NC == null)
            {
                NC = new NodeConnection(this);
                _LastGN = n;
                if (isRightOfNode)
                {
                    n.AddR(NC);
                    NC_R = true;
                }
                else
                {
                    n.AddL(NC);
                    NC_R = false;
                }
                __is_operating_NC = true;
                ManagedConnections.Add(NC);
                AdornerCanvas.Children.Insert(0, NC.GetPath());
            }
            else
            {
                __is_operating_NC = false;
                if (isRightOfNode)
                {
                    if (NC_R || _LastGN == n)
                    {
                        n.RemoveR(NC);
                        ManagedConnections.Remove(NC);
                        AdornerCanvas.Children.Remove(NC.GetPath());
                        NC = null;
                        return;
                    }
                    n.AddR(NC);
                }
                else
                {
                    if (!NC_R || _LastGN == n)
                    {
                        n.RemoveL(NC);
                        ManagedConnections.Remove(NC);
                        AdornerCanvas.Children.Remove(NC.GetPath());
                        NC = null;
                        return;
                    }
                    n.AddL(NC);
                }
                NC = null;
            }
        }
    }
}
