using xUI.Core.Data;
using xUI.Core.UIElements;

namespace xUI.Core.Helpers
{
    public static class StyleHelper
    {
        static StyleDefinition FindStyleResource(this UIElement element, string name)
        {
            if (element == null) return null;
            foreach (var item in element.StyleResources.Definitions)
            {
                if (item.Name == name) return item;
            }
            return FindStyleResource(element.Parent, name);
        }
        public static void ApplyStyle(this UIElement element, string name)
        {
            var s = FindStyleResource(element, name);
            if (s != null)
            {
                foreach (var item in s.Properties)
                {
                    element.SetProperty(item.Key, item.Value);
                }
            }
        }
    }
}
