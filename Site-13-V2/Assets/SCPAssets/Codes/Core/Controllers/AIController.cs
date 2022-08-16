using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Site13Kernel.Core;
using Site13Kernel.Core.Controllers;
using Site13Kernel.Diagnostics;
using System;
using Site13Kernel.GameLogic.AI;
using System.Runtime.CompilerServices;
using Site13Kernel.GameLogic.AI.V2;
using UnityEngine.UIElements;

namespace Site13Kernel
{
    public class AIController : BehaviorController
    {
        public List<List<BTAgent>> Agents;
        public int Slices = 2;
        int AddIndicator=0;
        int RuntimeIndicator = 0;
        public static AIController CurrentController;
        void Start()
        {
            for (int i = 0; i < Slices; i++)
            {
                Agents.Add(new List<BTAgent>());
            }
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

        private void Update()
        {
            float DeltaTime = Time.deltaTime;
            float UnscaledDeltaTime = Time.unscaledDeltaTime;
            int ___i = RuntimeIndicator % (Slices + 1);
            var L = Agents[___i];
            for (int num = L.Count-1; num >=0; num--)
            {
                L[num].Refresh(DeltaTime, UnscaledDeltaTime);
            }
            RuntimeIndicator++;
            if (RuntimeIndicator == Slices)
            {
                RuntimeIndicator = 0;
            }
            for (int num = _OnRefresh.Count - 1; num >= 0; num--)
            {
                if (!INTERRUPT_REFRESH)
                {
                    AICharacter controllable = _OnRefresh[num] as AICharacter;
                    if (controllable != null)
                        try
                        {
                            controllable.Refresh(DeltaTime, UnscaledDeltaTime);
                        }
                        catch (Exception obj)
                        {
                            Debugger.CurrentDebugger.Log(obj, LogLevel.Error);
                        }
                }
            }
        }
        //public override void Refresh(float DeltaTime, float UnscaledDeltaTime)
        //{
        //}
        public AICharacter Spawn(string ID, Vector3 position, Vector3 Rotation)
        {
            var OBJ = GlobalBioController.CurrentGlobalBioController.Spawn(ID, position, Rotation);
            var ai = OBJ.GetComponent<AICharacter>();
            ai.Parent = this;
            ai.Init();
            this.RegisterRefresh(ai);
            return ai;
        }
        public BTAgent SpawnV2(string ID,Vector3 pos,Vector3 rot)
        {
            var OBJ = GlobalBioController.CurrentGlobalBioController.Spawn(ID, pos, rot);
            var ai = OBJ.GetComponent<BTAgent>();
            ai.isPrimitive = false;
            int ___i = AddIndicator% (Slices + 1);
            Agents[___i].Add(ai);
            AddIndicator++;
            return ai;
        }
        public void DestoryAllCharacters()
        {
            for (int i = this._OnRefresh.Count - 1; i >= 0; i--)
            {
                var item = this._OnRefresh[i] as AICharacter;
                if (item == null)
                {
                    _OnRefresh.RemoveAt(i);
                }
                else
                {
                    DestoryAICharacter(item);
                }
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
