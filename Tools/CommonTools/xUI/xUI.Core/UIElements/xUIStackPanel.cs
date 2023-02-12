using xUI.Core.Abstraction;
using xUI.Core.Attributes;

namespace xUI.Core.UIElements
{
    public class xUIStackPanel : UIElement, IxUIContainer
    {
        public Orientation Orientation { get; set; }
        public bool ManagedByImpl;
        public override void Initialize()
        {
            base.Initialize();
            if(ManagedByImpl)
            OrganizeLayout();
        }
        public void Add(object content)
        {
            Children.Add(content as IUIElement);
            if (!ManagedByImpl) if (IsInitialized()) OrganizeLayout();
        }

        public void Remove(object content)
        {
            Children.Remove(content as IUIElement);
            if (!ManagedByImpl) if (IsInitialized()) OrganizeLayout();
        }
        [TODO]
        public void OrganizeLayout()
        {

        }
    }
    public enum Orientation
    {
        Horizontal, Vertical
    }
}
