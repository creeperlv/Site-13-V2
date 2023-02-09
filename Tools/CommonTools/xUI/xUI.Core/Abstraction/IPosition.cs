using System.Numerics;

namespace xUI.Core.Abstraction
{
    public interface IPosition
    {
        Vector2 Position { get; set; }
        void SetIPositionImplementation(IPositionImplementation implementation);
        void SetPosition(Vector2 Position);
        void SetPositionDataOnly(Vector2 Position);
    }
    public interface IPositionImplementation
    {
        void SetPosition(Vector2 Position);
    }
}
