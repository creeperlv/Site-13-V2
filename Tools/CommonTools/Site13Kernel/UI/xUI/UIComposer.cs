using Site13Kernel.UI.xUI.Composition.Deserialization;
using Site13Kernel.UI.xUI.UIElements;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Site13Kernel.UI.xUI
{
    public static class UIComposer
    {
        static Dictionary<string, IInstantiatable> Instantiators = new Dictionary<string, IInstantiatable>();
        public static void Init()
        {
            Instantiators.Add("Button", new xUIButtonInstantiator());
            Instantiators.Add("Window", new xUIWindowInstantiator());
            Instantiators.Add("Text", new xUITextInstantiator());

        }
        public static void Register(string name,IInstantiatable instantiator)
        {
            Instantiators.Add(name, instantiator);
        }
        public static UIElement Parse(string xml)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);
            var root=xmlDocument.DocumentElement;
            return ParseRecursively(root);
        }
        static UIElement ParseRecursively(XmlElement element)
        {
            var _element=Instantiators[element.Name].Instantiate();
            return _element;
        }
    }
}
