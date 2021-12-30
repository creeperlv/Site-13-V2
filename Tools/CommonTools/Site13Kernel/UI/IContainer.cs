using System.Collections.Generic;

namespace Site13Kernel.UI
{
    public interface IContainer
    {
        void AddChild(IVisualElement element);
        void SetChildren(List<IVisualElement> ChildrenList);
        void RemoveChild(IVisualElement element);
    }
}
