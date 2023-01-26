using Site13Kernel.UI.xUI.Abstraction;
using Site13Kernel.UI.xUI.Composition.Deserialization;
using Site13Kernel.UI.xUI.UIElements;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Site13Kernel.UI.xUI
{
    //public static class xUICore {
    //}
    public static class UIComposer
    {
        static Dictionary<string, IInstantiatable> Instantiators = new Dictionary<string, IInstantiatable>();
        public static void Init()
        {
            Instantiators.Add("Button", new xUIButtonInstantiator());
            Instantiators.Add("Window", new xUIWindowInstantiator());
            Instantiators.Add("Text", new xUITextInstantiator());

        }
        public static void Register(string name, IInstantiatable instantiator)
        {
            Instantiators.Add(name, instantiator);
        }
        public static UIElement Parse(string xml)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);
            var root = xmlDocument.DocumentElement;
            return ParseRecursively(root);
        }
        static UIElement ParseRecursively(XmlElement element)
        {
            var _element = Instantiators[element.Name].Instantiate();
            var attr_c = element.Attributes;
            foreach (XmlAttribute item in attr_c)
            {
                _element.SetProperty(item.Name, item.Value);
            }
            foreach (XmlElement item in element.ChildNodes)
            {
                if (item.Name.Contains('.'))
                {
                    //It's an attribute！
                    var attr_name = item.Name.Split('.')[1];
                    _element.SetProperty(attr_name, item.Value);
                    //item.Value
                }
                else
                {
                    if (_element is IxUIContainer c)
                    {
                        var _c = ParseRecursively(item);
                        _c.Parent = _element;
                        c.Add(c);
                    }
                    else
                    {
                        throw new Exception($"Expect a container, but {element.Name} does not implement IxUIContainer!");
                    }
                }
            }
            return _element;
        }
    }
    public class UIContext
    {
        public AbstractRenderEngine Engine;
    }
}
