using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Site13Kernel.UI.Elements
{
    public class Page : IContainer
    {
        public Transform BaseTranform;
        public List<IVisualElement> Children=new List<IVisualElement>();
        public Visibility Visibility { get => _visibility; set { 
            _visibility = value;
                switch (_visibility)
                {
                    case Visibility.Visible:
                        Show();
                        break;
                    case Visibility.Hidden:
                        Hide();
                        break;
                    case Visibility.Collapsed:
                        Collapse();
                        break;
                    default:
                        break;
                }
            } }
        private Visibility _visibility= Visibility.Visible;
        public void AddChild(IVisualElement element)
        {
            Children.Add(element);
        }

        public void Collapse()
        {
            foreach (var item in Children)
            {
                item.Collapse();
            }
        }

        public void Hide()
        {
            foreach (var item in Children)
            {
                item.Hide();
            }
        }

        public void RemoveChild(IVisualElement element)
        {
            Children.Remove(element);
        }

        public void SetChildren(List<IVisualElement> ChildrenList)
        {
            Children = ChildrenList;
        }

        public void Show()
        {
            foreach (var item in Children)
            {
                item.Show();
            }
        }

        public void Size()
        {
        }
    }
}
