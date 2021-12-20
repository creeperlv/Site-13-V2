using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Site13Kernel.Core;
using Site13Kernel.Core.Controllers;
using Site13Kernel.Diagnostics;
using System;
using Site13Kernel.GameLogic.AI;
using System.Runtime.CompilerServices;

namespace Site13Kernel
{
    public class AIController : BehaviorController
    {
        public static AIController CurrentController;
        void Start()
        {
            CurrentController = this;
            if (CrossScene)
            {
                DontDestroyOnLoad(base.gameObject);
            }

            SerializeAll();
            foreach (IControllable item in _OnInit)
            {
                try
                {
                    InitBehavior(item);
                }
                catch (Exception obj)
                {
                    Debugger.CurrentDebugger.Log(obj, LogLevel.Error);
                }
            }

            foreach (IControllable item2 in _OnInit)
            {
                if (!INTERRUPT_INIT)
                {
                    try
                    {
                        item2.Init();
                    }
                    catch (Exception obj2)
                    {
                        Debugger.CurrentDebugger.Log(obj2, LogLevel.Error);
                    }
                }
            }
        }
        public AICharacter Spawn(string ID, Vector3 position, Vector3 Rotation)
        {
            var OBJ = GlobalBioController.CurrentGlobalBioController.Spawn(ID, position, Rotation);
            var ai = OBJ.GetComponent<AICharacter>();
            ai.Parent = this;
            ai.Init();
            this.RegisterRefresh(ai);
            return ai;
        }
        public void DestoryAllCharacters()
        {
            for (int i = this._OnRefresh.Count - 1; i >= 0; i--)
            {
                var item = this._OnRefresh[i] as AICharacter;
                DestoryAICharacter(item);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DestoryAICharacter(AICharacter character)
        {
            UnregisterRefresh(character);
            character.BioEntity.Controller.DestroyEntity(character.BioEntity);
        }
    }
}
