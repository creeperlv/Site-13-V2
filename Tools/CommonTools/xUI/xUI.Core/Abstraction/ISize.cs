using System.Numerics;

namespace xUI.Core.Abstraction
{
    public interface ISize
    {
        void SetISizeImplementation(ISizeImplementation implementation);
        Vector2 Size { get; set; }
        /// <summary>
        /// Logical Size
        /// </summary>
        /// <param name="Size"></param>
        void SetSize(Vector2 Size);
        void SetDesireSize(Vector2 Size);
        void SetSizeDataOnly(Vector2 Size);
    }
    public interface ISizeImplementation
    {
        Vector2 GetActualSize();
        void SetSize(Vector2 Size);
    }
}
