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
            float offset=0;
            foreach (var item in Children)
            {
                switch (Orientation)
                {
                    case Orientation.Horizontal:
                        {
                            if(item is IPosition p)
                            {
                                p.SetPosition(new System.Numerics.Vector2(offset,0));
                                if(item is ISize s)
                                {
                                    offset += s.Size.X;
                                }
                            }
                        }
                        break;
                    case Orientation.Vertical:
                        {

                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
    public enum Orientation
    {
        Horizontal, Vertical
    }
}
