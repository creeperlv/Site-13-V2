using CleverCrow.Fluid.BTs.Trees;
using Site13Kernel.Core;
using Site13Kernel.GameLogic.BT.Nodes;
using Site13Kernel.GameLogic.BT.Nodes.Actions;
using Site13Kernel.GameLogic.BT.Nodes.Conditions;
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
        public bool isPrimitive = true;
        private void Awake()
        {
            var useBinaryData = TreeSource.name.ToUpper().EndsWith(".BYTES");
            BTBaseNode root = null;
            if (useBinaryData)
            {
                root = BinaryUtilities.Deserialize<BTBaseNode>(TreeSource.bytes);
            }
            else
            {
                root = JsonUtilities.Deserialize<BTBaseNode>(TreeSource.text);
            }
            var builder = new BehaviorTreeBuilder(gameObject);
            SetupNode(root, ref builder);
            tree = builder.Build();
        }
        private void Update()
        {
            //tree.Tick();
        }
        public void SetupNode(BTBaseNode node, ref BehaviorTreeBuilder builder)
        {
            switch (node)
            {
                case Selector _:
                    builder = builder.Selector();
                    break;
                case Sequence _:
                    builder = builder.Sequence();
                    break;
                case End _:
                    builder = builder.End();
                    break;
                case IsActionComplete c:
                    {
                        builder = builder.Condition("IsActionComplete", () =>
                        {
                            if (c.RevertBool)
                                return (agent.BlockActionCountDown > 0);
                            return agent.BlockActionCountDown <= 0;
                        });
                    }
                    break;
                case IsLowHealth c:
                    {
                        builder = builder.Condition("IsLowHealth", () =>
                        {
                            if (c.RevertBool)
                                return (agent.ControlledEntity.CurrentHP > agent.LowHealth);
                            return agent.ControlledEntity.CurrentHP <= agent.LowHealth;
                        });
                    }
                    break;
                case CheckBehaviorMode c:
                    {
                        builder = builder.Condition("CheckBehaviorMode", () =>
                        {
                            if (c.RevertBool)
                                return agent.CurrentMode != c.TargetMode;
                            return agent.CurrentMode == c.TargetMode;
                        });
                    }
                    break;
                case IsSpottedEnemy c:
                    {
                        builder = builder.Condition("IsSpottedEnemy", () =>
                        {
                            return false;
                        });
                    }
                    break;
                case IsHit c:
                    {
                        builder = builder.Condition("IsHit", () =>
                        {
                            var b = agent.isHit;
                            agent.isHit = false;
                            return b;
                        });
                    }
                    break;
                case SetBehaviorMode c:
                    {
                        builder = builder.Do("SetBehaviorMode", () =>
                        {
                            agent.CurrentMode = c.TargetMode;
                            return CleverCrow.Fluid.BTs.Tasks.TaskStatus.Success;
                        });
                    }
                    break;
                case PlayMotion c:
                    {
                        builder = builder.Do("PlayMotion", () =>
                        {
                            //agent.ControlledAnimatedCharacter.Pla
                            return CleverCrow.Fluid.BTs.Tasks.TaskStatus.Success;
                        });
                    }
                    break;
                default:
                    break;
            }
            if (node.NextNode != null)
            {
                SetupNode(node.NextNode, ref builder);
            }
            else
            {
            }
        }
    }
}
