﻿using Site13Kernel.Core;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.UI;

namespace Site13Kernel.UI.Elements
{
    public class GenericInteractivable : ControlledBehavior
    {
        public List<Button> Controlled_uUIBUtton;
        public List<TextButton> Controlled_TextBUtton;
        public List<Text> Controlled_uGUI_Text;
        private Site13Event _onClick = new Site13Event();
        private string content;
        public string Content
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
        }
        public void Awake()
        {
            if (isInited) return;
            UpdateOnClickEvents();
            UpdateContent();
        }

        public override void Init()
        {
            if (isInited) return;
            UpdateOnClickEvents();
            UpdateContent();
        }
        bool isInited = false;
        void InvokeOnClick()
        {
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
