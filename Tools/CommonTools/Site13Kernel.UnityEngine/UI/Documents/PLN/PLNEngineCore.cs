using CLUNL.Localization;
using Site13Kernel.Data;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
namespace Site13Kernel.UI.Documents.PLN
{
    public class PLNEngineCore
    {
        static GameObject _TextTemplate;
        static GameObject _ImageTemplate;
        public static void Init(GameObject TextTemplate, GameObject ImageTemplate)
        {
            _TextTemplate = TextTemplate;
            _ImageTemplate = ImageTemplate;
        }
        public static void ViewLanguage(Transform Container, IEnumerator<string> contents, Color DefaultColor, int BaseSize = 14)
        {
            View(Container, EnumStr(contents), DefaultColor, BaseSize);
        }
        static IEnumerator<string> EnumStr(IEnumerator<string> contents)
        {
            string current = contents.Current;
            while (contents.MoveNext())
            {
                yield return Language.Find(current);
            }
        }
        public static void View(Transform Container, IEnumerator<string> contents, Color DefaultColor, int BaseSize = 14)
        {
            string current = contents.Current;
            while (contents.MoveNext())
            {
                Process(current);
                current = contents.Current;
            }
            void Process(string item)
            {
                if (item.StartsWith("[Img]"))
                {
                    var URL = item.Substring(5).Trim();
                    if (URL.StartsWith("Site-13://"))
                    {
                        var Name = URL.Substring("Site-13://".Length);
                        var t = GameObject.Instantiate(_ImageTemplate, Container).GetComponent<Image>();
                        t.sprite = ImageStorage.FindSprite(Name);
                    }
                }
                else
                {

                    var t = GameObject.Instantiate(_TextTemplate, Container).GetComponent<Text>();
                    //t.text = item;
                    t.fontSize = BaseSize;
                    t.color = DefaultColor;
                    SequentialProcessLine(item, t);
                }
            }
            void SequentialProcessLine(string Line, Text t)
            {
                if (Line.StartsWith("[C]"))
                {
                    var item = Line.Substring(3).Trim();
                    t.alignment = TextAnchor.MiddleCenter;
                    SequentialProcessLine(item, t);
                }
                else if (Line.StartsWith("[R]"))
                {
                    var item = Line.Substring(3).Trim();
                    t.alignment = TextAnchor.MiddleRight;
                    SequentialProcessLine(item, t);
                }
                else if (Line.StartsWith("[L]"))
                {
                    var item = Line.Substring(3).Trim();
                    t.alignment = TextAnchor.MiddleRight;
                    SequentialProcessLine(item, t);
                }
                else if (Line.StartsWith("[B]"))
                {
                    var item = Line.Substring(3).Trim();
                    t.fontStyle = FontStyle.Bold;
                    SequentialProcessLine(item, t);
                }
                else if (Line.StartsWith("[I]"))
                {
                    var item = Line.Substring(3).Trim();
                    t.fontStyle = FontStyle.Italic;
                    SequentialProcessLine(item, t);
                }
                else if (Line.StartsWith("[IB]"))
                {
                    var item = Line.Substring(4).Trim();
                    t.fontStyle = FontStyle.BoldAndItalic;
                    SequentialProcessLine(item, t);
                }
                else if (Line.StartsWith("[H1]"))
                {
                    var item = Line.Substring(4).Trim();
                    t.fontSize = BaseSize + 2;
                    SequentialProcessLine(item, t);
                }
                else if (Line.StartsWith("[H2]"))
                {
                    var item = Line.Substring(4).Trim();
                    t.fontSize = BaseSize + 4;
                    SequentialProcessLine(item, t);
                }
                else if (Line.StartsWith("[H3]"))
                {
                    var item = Line.Substring(4).Trim();
                    t.fontSize = BaseSize + 6;
                    SequentialProcessLine(item, t);
                }
                else if (Line.StartsWith("[H4]"))
                {
                    var item = Line.Substring(4).Trim();
                    t.fontSize = BaseSize + 8;
                    SequentialProcessLine(item, t);
                }
                else if (Line.StartsWith("[N1]"))
                {
                    var item = Line.Substring(4).Trim();
                    t.fontSize = BaseSize - 2;
                    SequentialProcessLine(item, t);
                }
                else if (Line.StartsWith("[N2]"))
                {
                    var item = Line.Substring(4).Trim();
                    t.fontSize = BaseSize - 4;
                    SequentialProcessLine(item, t);
                }
                else if (Line.StartsWith("[N3]"))
                {
                    var item = Line.Substring(4).Trim();
                    t.fontSize = BaseSize - 6;
                    SequentialProcessLine(item, t);
                }
                else if (Line.StartsWith("[N4]"))
                {
                    var item = Line.Substring(4).Trim();
                    t.fontSize = BaseSize - 8;
                    SequentialProcessLine(item, t);
                }
                else if (Line.StartsWith("[C/B]"))
                {
                    var item = Line.Substring(5).Trim();
                    t.color = Color.black;
                    SequentialProcessLine(item, t);
                }
                else if (Line.StartsWith("[C/W]"))
                {
                    var item = Line.Substring(5).Trim();
                    t.color = Color.white;
                    SequentialProcessLine(item, t);
                }
                else if (Line.StartsWith("[C/G]"))
                {
                    var item = Line.Substring(5).Trim();
                    t.color = Color.green;
                    SequentialProcessLine(item, t);
                }
                else if (Line.StartsWith("[C/R]"))
                {
                    var item = Line.Substring(5).Trim();
                    t.color = Color.red;
                    SequentialProcessLine(item, t);
                }
                else if (Line.StartsWith("[C/Y]"))
                {
                    var item = Line.Substring(5).Trim();
                    t.color = Color.yellow;
                    SequentialProcessLine(item, t);
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
                    SequentialProcessLine(item, t);
                }
                else
                {
                    t.text = Line;
                }
            }
        }
    }
}
