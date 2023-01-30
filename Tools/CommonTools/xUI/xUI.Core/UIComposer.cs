using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Xml;
using xUI.Core.Abstraction;
using xUI.Core.Composition.Deserialization;
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
#if DEBUG
            Trace.WriteLine("Element:" + element.Name);
#endif
            var attr_c = element.Attributes;
            foreach (XmlAttribute item in attr_c)
            {
                _element.SetProperty(item.Name, item.Value);
            }
            bool isContent = false;
#if DEBUG
            Trace.WriteLine("XML Child Count:" + element.ChildNodes.Count);
#endif
            foreach (var item in element.ChildNodes)
            {
#if DEBUG
                Trace.WriteLine("XML T:" + item.GetType().Name);
#endif

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
#if DEBUG
                        Trace.WriteLine($"Find an element:{childElement.Name}");
#endif
                        if (_element is IxUIContainer c)
                        {
                            var _c = ParseRecursively(childElement);
                            _c.Parent = _element;
                            c.Add(_c);
#if DEBUG
                            Trace.WriteLine("IxUIContainer adding item on:" + element.Name);
#endif
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
#if DEBUG
                                Trace.WriteLine("IContent setting item on:" + element.Name);
#endif
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
                    //if(_element is IContent _ic)
                    //{
                    //    _ic.Content = t.Value;
                    //}else if(_element)

                    switch (_element)
                    {
                        case IContent _ic:
#if DEBUG
                            Trace.WriteLine("IContent is a string:" + element.Name);
#endif
                            _ic.Content = t.Value;
                            break;
                        default:
                            break;
                    }
                }
            }
#if DEBUG
            if (_element.Children != null)
            {
                Trace.WriteLine("xUI Child Count:" + _element.Children.Count);

            }
#endif
            return _element;
        }
    }
    public class UIContext
    {
        public AbstractRenderEngine Engine;
    }
}
