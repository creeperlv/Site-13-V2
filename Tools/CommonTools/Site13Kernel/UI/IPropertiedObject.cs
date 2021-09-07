using Site13Kernel.Core;
namespace Site13Kernel.UI
{
    public interface IPropertiedObject
    {
        void InitProperty(Property p)
        {
            SetProperty(p);
        }
        void InitProperty(string name, object value)
        {
            SetProperty(name, value);
        }
        void SetProperty(string name, object value);
        void SetProperty(Property p);
        Property GetProperty(string name);
        object GetPropertyValue(string name);
    }
}
