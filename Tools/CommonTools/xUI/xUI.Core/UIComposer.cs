using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using xUI.Core.Abstraction;
using xUI.Core.Composition.Deserialization;
using xUI.Core.Data;
using xUI.Core.UIElements;

namespace xUI.Core
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
            Instantiators.Add("Grid", new xUIGridInstantiator());
            Instantiators.Add("SolidColorRectangle", new xUISolidColorRectangleInstantiator());
            Instantiators.Add("Menu", new xUIMenuInstantiator());
            Instantiators.Add("MenuItem", new xUIMenuItemInstantiator());

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
        static void ParseStyleProperty(StyleDefinition def, XmlElement xe)
        {
            Property property = new Property();
            property.Key = xe.Attributes["Key"].Value;
            def.Properties.Add(property);
            if (xe.HasAttribute("Value"))
            {
                property.Value = xe.GetAttribute("Value");
            }
            if (xe.ChildNodes.Count > 0)
            {
                foreach (var item in xe.ChildNodes)
                {
                    if (item is XmlText t)
                    {
                        property.Value = t.Value;
                    }
                    else if (item is XmlElement __xe)
                    {
                        property.Value = ParseRecursively(__xe);
                    }
                }
            }
        }
        static void ParseSingleStyle(UIElement uie, XmlElement xe)
        {
            StyleDefinition def = new StyleDefinition();
            def.Name = xe.GetAttribute("Name");
            uie.StyleResources.Definitions.Add(def);
            foreach (var item in xe.ChildNodes)
            {
                if (item is XmlElement childN)
                {
                    if (childN.Name == "Property")
                    {
                        ParseStyleProperty(def, childN);
                    }
                }
            }
        }
        static void ParseStyles(UIElement uie, XmlElement xe)
        {
            uie.StyleResources = new Style();
            foreach (var item in xe.ChildNodes)
            {
                if (item is XmlElement childN)
                {
                    if (childN.Name == "Style")
                    {
                        ParseSingleStyle(uie, childN);
                    }
                }
                else
                {

                }
            }
        }
        static UIElement ParseRecursively(XmlElement element)
        {
            var _element = Instantiators[element.Name].Instantiate();
            var attr_c = element.Attributes;
            foreach (XmlAttribute item in attr_c)
            {
                _element.SetProperty(item.Name, item.Value);
            }
            bool isContent = false;
            foreach (var item in element.ChildNodes)
            {

                if (item is XmlElement childElement)
                {

                    if (childElement.Name.Contains('.'))
                    {
                        //It's an attribute！
                        var attr_name = childElement.Name.Split('.')[1];
                        _element.SetProperty(attr_name, childElement.ChildNodes[0].Value);
                        //item.Value
                    }
                    else
                    {
                        if (childElement.Name == "Styles")
                        {
                            ParseStyles(_element, childElement);
                        }
                        else
                        if (_element is IxUIContainer c)
                        {
                            var _c = ParseRecursively(childElement);
                            _c.Parent = _element;
                            c.Add(_c);
                        }
                        else if (_element is IContent ic)
                        {
                            if (isContent == true)
                            {
                                throw new Exception($"Expect a container, but {childElement.Name}({_element.GetType().Name}) does not implement IxUIContainer!");
                            }
                            else
                            {
                                isContent = true;
                                var _c = ParseRecursively(childElement);
                                _c.Parent = _element;
                                ic.Content = _c;
                            }
                        }
                        else
                        {
                            throw new Exception($"Expect a container, but {childElement.Name}({_element.GetType().Name}) does not implement IxUIContainer!");
                        }

                    }
                }
                else if (item is XmlText t)
                {
                    switch (_element)
                    {
                        case IContent _ic:
                            _ic.Content = t.Value;
                            break;
                        default:
                            break;
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
