using CleverCrow.Fluid.BTs.Trees;
using Site13Kernel.Core;
using Site13Kernel.GameLogic.BT.Nodes;
using Site13Kernel.GameLogic.BT.Nodes.Generic;
using Site13Kernel.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.GameLogic.AI.V2
{
    public class BTAgent : ControlledBehavior
    {
        public TextAsset TreeSource;
        public AIAgent agent;        
        [SerializeField]
        public BehaviorTree tree;
        public bool isPrimitive=true;
        private void Awake()
        {
            var useBinaryData=TreeSource.name.ToUpper().EndsWith(".BYTES");
            BTBaseNode root = null;
            if (useBinaryData)
            {
                root = BinaryUtilities.Deserialize<BTBaseNode>(TreeSource.bytes);
            }
            else
            {
                root = JsonUtilities.Deserialize<BTBaseNode>(TreeSource.text);
            }
            var builder=new BehaviorTreeBuilder(gameObject);
            SetupNode(root,ref builder);
        }
        public void SetupNode(BTBaseNode node,ref BehaviorTreeBuilder builder)
        {
            switch (node)
            {
                case Selector _:
                    builder=builder.Selector();
                    break;
                case Sequence _:
                    builder = builder.Sequence();
                    break;
                case End _:
                    builder = builder.End();
                    break;
                default:
                    break;
            }
            if (node.NextNode != null)
            {
                SetupNode(node.NextNode,ref builder);
            }
        }
    }
}
