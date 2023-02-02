using System.Numerics;

namespace xUI.Core.Abstraction
{
    public interface ISize
    {
        void SetISizeImplementation(ISizeImplementation implementation);
        Vector2 Size { get; set; }
        void SetSize(Vector2 Size);
        void SetSizeDataOnly(Vector2 Size);
    }
    public interface ISizeImplementation
    {
        void SetSize(Vector2 Size);
    }
}
