using CLUNL.Localization;
using Site13Kernel.Data;
using Site13Kernel.Diagnostics;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Debug = Site13Kernel.Diagnostics.Debug;

namespace Site13Kernel.UI.Documents.PLN
{
    public class PLNEngineCore
    {
        static GameObject _TextTemplate;
        static GameObject _ImageTemplate;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Init(GameObject TextTemplate, GameObject ImageTemplate)
        {
            _TextTemplate = TextTemplate;
            _ImageTemplate = ImageTemplate;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ViewLanguage(Transform Container, IEnumerable<string> contents, Color DefaultColor, int BaseSize = 14)
        {
            View(Container, contents, DefaultColor, BaseSize);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void View(Transform Container, IEnumerable<string> contents, Color DefaultColor, int BaseSize = 14)
        {

            foreach (var item in contents)
            {
                Process(item, Container, DefaultColor, BaseSize);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SequentialProcessLine(string Line, Text t, int BaseSize)
        {
            if (Line.StartsWith("[C]"))
            {
                var item = Line.Substring(3).Trim();
                t.alignment = TextAnchor.MiddleCenter;
                SequentialProcessLine(item, t, BaseSize);
            }
            else if (Line.StartsWith("[R]"))
            {
                var item = Line.Substring(3).Trim();
                t.alignment = TextAnchor.MiddleRight;
                SequentialProcessLine(item, t, BaseSize);
            }
            else if (Line.StartsWith("[L]"))
            {
                var item = Line.Substring(3).Trim();
                t.alignment = TextAnchor.MiddleRight;
                SequentialProcessLine(item, t, BaseSize);
            }
            else if (Line.StartsWith("[B]"))
            {
                var item = Line.Substring(3).Trim();
                t.fontStyle = FontStyle.Bold;
                SequentialProcessLine(item, t, BaseSize);
            }
            else if (Line.StartsWith("[I]"))
            {
                var item = Line.Substring(3).Trim();
                t.fontStyle = FontStyle.Italic;
                SequentialProcessLine(item, t, BaseSize);
            }
            else if (Line.StartsWith("[IB]"))
            {
                var item = Line.Substring(4).Trim();
                t.fontStyle = FontStyle.BoldAndItalic;
                SequentialProcessLine(item, t, BaseSize);
            }
            else if (Line.StartsWith("[H1]"))
            {
                var item = Line.Substring(4).Trim();
                t.fontSize = BaseSize + 2;
                SequentialProcessLine(item, t, BaseSize);
            }
            else if (Line.StartsWith("[H2]"))
            {
                var item = Line.Substring(4).Trim();
                t.fontSize = BaseSize + 4;
                SequentialProcessLine(item, t, BaseSize);
            }
            else if (Line.StartsWith("[H3]"))
            {
                var item = Line.Substring(4).Trim();
                t.fontSize = BaseSize + 6;
                SequentialProcessLine(item, t, BaseSize);
            }
            else if (Line.StartsWith("[H4]"))
            {
                var item = Line.Substring(4).Trim();
                t.fontSize = BaseSize + 8;
                SequentialProcessLine(item, t, BaseSize);
            }
            else if (Line.StartsWith("[H5]"))
            {
                var item = Line.Substring(4).Trim();
                t.fontSize = BaseSize + 10;
                SequentialProcessLine(item, t, BaseSize);
            }
            else if (Line.StartsWith("[H4]"))
            {
                var item = Line.Substring(4).Trim();
                t.fontSize = BaseSize + 8;
                SequentialProcessLine(item, t, BaseSize);
            }
            else if (Line.StartsWith("[N1]"))
            {
                var item = Line.Substring(4).Trim();
                t.fontSize = BaseSize - 2;
                SequentialProcessLine(item, t, BaseSize);
            }
            else if (Line.StartsWith("[N2]"))
            {
                var item = Line.Substring(4).Trim();
                t.fontSize = BaseSize - 4;
                SequentialProcessLine(item, t, BaseSize);
            }
            else if (Line.StartsWith("[N3]"))
            {
                var item = Line.Substring(4).Trim();
                t.fontSize = BaseSize - 6;
                SequentialProcessLine(item, t, BaseSize);
            }
            else if (Line.StartsWith("[N4]"))
            {
                var item = Line.Substring(4).Trim();
                t.fontSize = BaseSize - 8;
                SequentialProcessLine(item, t, BaseSize);
            }
            else if (Line.StartsWith("[C/B]"))
            {
                var item = Line.Substring(5).Trim();
                t.color = Color.black;
                SequentialProcessLine(item, t, BaseSize);
            }
            else if (Line.StartsWith("[C/W]"))
            {
                var item = Line.Substring(5).Trim();
                t.color = Color.white;
                SequentialProcessLine(item, t, BaseSize);
            }
            else if (Line.StartsWith("[C/G]"))
            {
                var item = Line.Substring(5).Trim();
                t.color = Color.green;
                SequentialProcessLine(item, t, BaseSize);
            }
            else if (Line.StartsWith("[C/R]"))
            {
                var item = Line.Substring(5).Trim();
                t.color = Color.red;
                SequentialProcessLine(item, t, BaseSize);
            }
            else if (Line.StartsWith("[C/Y]"))
            {
                var item = Line.Substring(5).Trim();
                t.color = Color.yellow;
                SequentialProcessLine(item, t, BaseSize);
            }
            else if (Line.StartsWith("[C:"))
            {
                var item = Line.Substring(3).Trim();
                var COLOR = item.Substring(0, 6);
                if (ColorUtility.TryParseHtmlString(COLOR, out var c))
                {
                    t.color = c;
                }
                item = item.Substring(7).Trim();
                SequentialProcessLine(item, t, BaseSize);
            }
            else
            {
                t.text = Line;
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Process(string item, Transform Container, Color DefaultColor, int BaseSize)
        {
            if (item.StartsWith("[Img]"))
            {
                var URL = item.Substring(5).Trim();
                Uri uri = new Uri(URL);

                {
                    var HOST = uri.GetComponents(UriComponents.Host, UriFormat.UriEscaped);
                    var Name = uri.GetComponents(UriComponents.Path, UriFormat.UriEscaped);

                    var o = GameObject.Instantiate(_ImageTemplate, Container);
                    var t = o.GetComponent<Image>();
                    var rt = o.transform as RectTransform;
                    if (HOST.ToUpper() == "LOCAL.CREEPERLV.GITHUB.IO")
                    {

                        t.sprite = ImageStorage.FindSprite(Name).LoadedSprite;
                    }
                    var para = uri.GetComponents(UriComponents.Query, UriFormat.UriEscaped);
                    if (para != null)
                    {
                        var paras = para.Split('&');
                        foreach (var p in paras)
                        {
                            var kv = p.Split('=');
                            switch ((kv[0]).ToUpper())
                            {
                                case "W":
                                case "WIDTH":
                                    {
                                        float w = float.Parse(kv[1]);
                                        var v = rt.sizeDelta;
                                        v.x = w;
                                        rt.sizeDelta = v;
                                    }
                                    break;
                                case "H":
                                case "HEIGHT":
                                    {
                                        float h = float.Parse(kv[1]);
                                        var v = rt.sizeDelta;
                                        v.y = h;
                                        rt.sizeDelta = v;
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
            else
            {

                var t = GameObject.Instantiate(_TextTemplate, Container).GetComponent<Text>();
                if (t == null)
                {
                    Debugger.CurrentDebugger.LogError("Text is null, is TextTemplate inited?");
                    return;
                }
                t.fontSize = BaseSize;
                t.color = DefaultColor;
                SequentialProcessLine(item, t,BaseSize);
            }
        }
    }
}
