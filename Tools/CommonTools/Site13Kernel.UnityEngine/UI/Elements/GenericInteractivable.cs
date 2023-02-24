using CLUNL.Localization;
using Site13Kernel.Core;
using Site13Kernel.Core.Controllers;
using Site13Kernel.Data;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;

namespace Site13Kernel.UI.Elements
{
    public class GenericInteractivable : ControlledBehavior
    {
        public List<Button> Controlled_uUIBUtton;
        public List<TextButton> Controlled_TextBUtton;
        public List<Text> Controlled_uGUI_Text;
        public bool initEffect = false;
        public bool GlobalEffect = false;
        public PrefabReference Effect;
        private Site13Event _onClick = new Site13Event();
        [UnityEngine.SerializeField]
        private LocalizedString content;
        [UnityEngine.SerializeField]
        public LocalizedString Content
        {
            get => content; set
            {
                content = value;
                UpdateContent();
            }
        }
        public void UpdateContent()
        {

            foreach (var item in Controlled_uGUI_Text)
            {
                item.text = content;
            }
        }
        public void UpdateOnClickEvents()
        {
            foreach (var item in Controlled_uUIBUtton)
            {
                item.onClick.RemoveAllListeners();
                item.onClick.AddListener(InvokeOnClick);
            }
            foreach (var item in Controlled_TextBUtton)
            {
                item.onClick.RemoveAllListeners();
                item.onClick.AddListener(InvokeOnClick);
            }
            isInited = true;
        }
        public void Start()
        {
            if (isInited) return;
            UpdateOnClickEvents();
            UpdateContent();
            SideInit();
        }
        public virtual void SideInit()
        {
            SideInit();
        }
        public void Awake()
        {
            if (isInited) return;
            UpdateOnClickEvents();
            UpdateContent();
        }
        public void OnEnable()
        {
            UpdateContent();
        }
        public override void Init()
        {
            if (isInited) return;
            UpdateOnClickEvents();
            UpdateContent();
        }
        bool isInited = false;
        internal virtual void InvokeOnClick()
        {
            if (initEffect)
            {
                if (GlobalEffect)
                    EffectController.CurrentEffectController.Spawn(Effect, Vector3.zero, Quaternion.identity, Vector3.one, null);
                else EffectController.CurrentEffectController.Spawn(Effect, Vector3.zero, Quaternion.identity, Vector3.one, this.transform);
            }
            _onClick.Invoke();
        }
        public Site13Event OnClick
        {
            get => _onClick; set
            {
                _onClick = value; UpdateOnClickEvents();
            }
        }

    }
}
