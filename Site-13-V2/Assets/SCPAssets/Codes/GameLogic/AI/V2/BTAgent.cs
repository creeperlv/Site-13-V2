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
        internal int ListSlot;
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
        public override void Refresh(float DeltaTime, float UnscaledDeltaTime)
        {
            if (!isPrimitive)
                tree.Tick();
            agent.OnFrame(DeltaTime);
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
                            agent.CheckEnemies();
                            return agent.isAwareOfEnemy;
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
                case WaitTime c:
                    {
                        builder = builder.WaitTime("Wait Time",c.Time);
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
                            agent.ControlledAnimatedCharacter.PlayMotion(c.Trigger, c.Layer);
                                return CleverCrow.Fluid.BTs.Tasks.TaskStatus.Success;
                        });
                    }
                    break;
                case SelfDestruct c:
                    {
                        builder = builder.Do("Self Destruct", () =>
                        {
                            agent.ControlledEntity.Die();
                                return CleverCrow.Fluid.BTs.Tasks.TaskStatus.Success;
                        });
                    }
                    break;
                case StartAction c:
                    {
                        builder = builder.Do("StartAction", () =>
                        {
                            agent.BlockActionCountDown = c.ActionLength;
                            return CleverCrow.Fluid.BTs.Tasks.TaskStatus.Success;
                        });
                    }
                    break;
                case FoeInAttackRange c:
                    {
                        builder = builder.Condition("FoeInAttackRange", () =>
                        {
                            if (agent.Collector.LastClosestFoe == null)
                                return false;
                            if ((agent.Collector.LastClosestFoe.transform.position - agent.transform.position).magnitude < agent.AttackRange)
                            {
                                return true;
                            }
                            return false;
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
